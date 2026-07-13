using FikaAmazonAPI.AmazonSpApiSDK.Models.Finances.Model;
using FikaAmazonAPI.Parameter.Finance;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace Tests
{
    [TestFixture]
    public class FinancesV20240619Tests
    {
        // Schema-shaped listTransactions payload (transaction-level breakdowns is an ARRAY per
        // the swagger schema; the docs example showing an object wrapper is a known docs bug).
        private const string FullPayloadJson = @"{
  ""payload"": {
    ""nextToken"": ""NEXT_TOKEN_1"",
    ""transactions"": [
      {
        ""sellingPartnerMetadata"": { ""sellingPartnerId"": ""A3TH9S8BH6GOGM"", ""accountType"": ""PAYABLE"", ""marketplaceId"": ""ATVPDKIKX0DER"" },
        ""relatedIdentifiers"": [
          { ""relatedIdentifierName"": ""ORDER_ID"", ""relatedIdentifierValue"": ""8129762527551"" },
          { ""relatedIdentifierName"": ""SETTLEMENT_ID"", ""relatedIdentifierValue"": ""SETTLE-77"" }
        ],
        ""transactionType"": ""Shipment"",
        ""transactionId"": ""TX-001"",
        ""transactionStatus"": ""RELEASED"",
        ""description"": ""Order Payment"",
        ""postedDate"": ""2020-07-14T03:35:13.214Z"",
        ""totalAmount"": { ""currencyAmount"": 10.5, ""currencyCode"": ""USD"" },
        ""marketplaceDetails"": { ""marketplaceId"": ""ATVPDKIKX0DER"", ""marketplaceName"": ""Amazon.com"" },
        ""items"": [
          {
            ""description"": ""Item title"",
            ""totalAmount"": { ""currencyAmount"": 10.5, ""currencyCode"": ""USD"" },
            ""relatedIdentifiers"": [ { ""itemRelatedIdentifierName"": ""ORDER_ADJUSTMENT_ITEM_ID"", ""itemRelatedIdentifierValue"": ""81297625-121-27551"" } ],
            ""breakdowns"": [
              { ""breakdownType"": ""Product Charges"", ""breakdownAmount"": { ""currencyAmount"": 10.5, ""currencyCode"": ""USD"" },
                ""breakdowns"": [ { ""breakdownType"": ""Principle"", ""breakdownAmount"": { ""currencyAmount"": 10.5, ""currencyCode"": ""USD"" }, ""breakdowns"": [] } ] }
            ],
            ""contexts"": [ { ""contextType"": ""ProductContext"", ""asin"": ""B07FGXZQZ1"", ""sku"": ""sku-12"", ""quantityShipped"": 1, ""fulfillmentNetwork"": ""MFN"" } ]
          }
        ],
        ""breakdowns"": [
          { ""breakdownType"": ""Sales"", ""breakdownAmount"": { ""currencyAmount"": 10.5, ""currencyCode"": ""USD"" },
            ""breakdowns"": [ { ""breakdownType"": ""Product Charges"", ""breakdownAmount"": { ""currencyAmount"": 10.5, ""currencyCode"": ""USD"" }, ""breakdowns"": [] } ] }
        ],
        ""contexts"": [
          { ""contextType"": ""AmazonPayContext"", ""storeName"": ""Store 1"", ""orderType"": ""Order Type"", ""channel"": ""MFN"" },
          { ""contextType"": ""DeferredContext"", ""deferralReason"": ""B2B"", ""maturityDate"": ""2024-07-14T00:00:00Z"" }
        ]
      }
    ]
  }
}";

        [Test]
        public void Deserialize_FullPayload_MapsAllFields()
        {
            var response = JsonConvert.DeserializeObject<ListTransactionsResponse>(FullPayloadJson);

            Assert.That(response, Is.Not.Null);
            Assert.That(response!.Payload.NextToken, Is.EqualTo("NEXT_TOKEN_1"));
            Assert.That(response.Payload.Transactions, Has.Count.EqualTo(1));

            var tx = response.Payload.Transactions[0];
            Assert.That(tx.TransactionId, Is.EqualTo("TX-001"));
            Assert.That(tx.TransactionType, Is.EqualTo("Shipment"));
            Assert.That(tx.TransactionStatus, Is.EqualTo("RELEASED"));
            Assert.That(tx.Description, Is.EqualTo("Order Payment"));
            Assert.That(tx.PostedDate, Is.EqualTo(new DateTime(2020, 7, 14, 3, 35, 13, 214, DateTimeKind.Utc)));
            Assert.That(tx.TotalAmount.CurrencyAmount, Is.EqualTo(10.5m));
            Assert.That(tx.TotalAmount.CurrencyCode, Is.EqualTo("USD"));
            Assert.That(tx.SellingPartnerMetadata.AccountType, Is.EqualTo("PAYABLE"));
            Assert.That(tx.MarketplaceDetails.MarketplaceName, Is.EqualTo("Amazon.com"));

            Assert.That(tx.RelatedIdentifiers[0].RelatedIdentifierName, Is.EqualTo(RelatedIdentifier.RelatedIdentifierNameEnum.ORDERID));
            Assert.That(tx.RelatedIdentifiers[0].RelatedIdentifierValue, Is.EqualTo("8129762527551"));
            Assert.That(tx.RelatedIdentifiers[1].RelatedIdentifierName, Is.EqualTo(RelatedIdentifier.RelatedIdentifierNameEnum.SETTLEMENTID));

            var item = tx.Items[0];
            Assert.That(item.RelatedIdentifiers[0].ItemRelatedIdentifierName, Is.EqualTo(ItemRelatedIdentifier.ItemRelatedIdentifierNameEnum.ORDERADJUSTMENTITEMID));
            Assert.That(item.Breakdowns[0].Breakdowns[0].BreakdownType, Is.EqualTo("Principle"));
            Assert.That(item.Contexts[0].ContextType, Is.EqualTo("ProductContext"));
            Assert.That(item.Contexts[0].Asin, Is.EqualTo("B07FGXZQZ1"));
            Assert.That(item.Contexts[0].QuantityShipped, Is.EqualTo(1));

            // Transaction-level breakdowns: array shape, recursive nesting.
            Assert.That(tx.Breakdowns, Has.Count.EqualTo(1));
            Assert.That(tx.Breakdowns[0].BreakdownType, Is.EqualTo("Sales"));
            Assert.That(tx.Breakdowns[0].Breakdowns[0].BreakdownType, Is.EqualTo("Product Charges"));

            // Context discriminator survives deserialization (added for spec parity).
            Assert.That(tx.Contexts[0].ContextType, Is.EqualTo("AmazonPayContext"));
            Assert.That(tx.Contexts[1].ContextType, Is.EqualTo("DeferredContext"));
            Assert.That(tx.Contexts[1].DeferralReason, Is.EqualTo("B2B"));
            Assert.That(tx.Contexts[1].MaturityDate, Is.EqualTo(new DateTime(2024, 7, 14, 0, 0, 0, DateTimeKind.Utc)));
        }

        [Test]
        public void Deserialize_UnknownIdentifierEnums_DoesNotThrow()
        {
            // Amazon has shipped identifier names before documenting them (SAFET_CLAIM_ID,
            // DEAL_ID); unknown values must not fail the whole response.
            const string json = @"{ ""payload"": { ""transactions"": [ {
                ""transactionId"": ""TX-002"",
                ""relatedIdentifiers"": [ { ""relatedIdentifierName"": ""FUTURE_UNKNOWN_ID"", ""relatedIdentifierValue"": ""X-1"" } ],
                ""items"": [ { ""relatedIdentifiers"": [ { ""itemRelatedIdentifierName"": ""FUTURE_ITEM_ID"", ""itemRelatedIdentifierValue"": ""Y-1"" } ] } ]
            } ] } }";

            ListTransactionsResponse? response = null;
            Assert.DoesNotThrow(() => response = JsonConvert.DeserializeObject<ListTransactionsResponse>(json));

            var tx = response!.Payload.Transactions[0];
            Assert.That(tx.RelatedIdentifiers[0].RelatedIdentifierName, Is.Null);
            Assert.That(tx.RelatedIdentifiers[0].RelatedIdentifierValue, Is.EqualTo("X-1"));
            Assert.That(tx.Items[0].RelatedIdentifiers[0].ItemRelatedIdentifierName, Is.Null);
            Assert.That(tx.Items[0].RelatedIdentifiers[0].ItemRelatedIdentifierValue, Is.EqualTo("Y-1"));
        }

        [Test]
        public void Serialize_Roundtrip_PreservesContextTypeAndEnumWireValues()
        {
            // flipacross's TransactionSnapshotMapper consumes the SDK response re-serialized to
            // JSON, so the round-trip must keep the wire names.
            var response = JsonConvert.DeserializeObject<ListTransactionsResponse>(FullPayloadJson);

            var roundtripped = JsonConvert.SerializeObject(response);

            StringAssert.Contains(@"""contextType"":""ProductContext""", roundtripped);
            StringAssert.Contains(@"""contextType"":""DeferredContext""", roundtripped);
            StringAssert.Contains(@"""relatedIdentifierName"":""ORDER_ID""", roundtripped);
            StringAssert.Contains(@"""itemRelatedIdentifierName"":""ORDER_ADJUSTMENT_ITEM_ID""", roundtripped);
        }

        [Test]
        public void GetParameters_IdentifierOnly_OmitsPostedAfter()
        {
            // Spec: postedAfter is only required when no related identifier is given. An unset
            // postedAfter must not be serialized (a 0001-01-01 value trips the 180-day window
            // rule and returns empty pages).
            var parameter = new ParameterListFinancialTransactions20240619()
            {
                relatedIdentifierName = RelatedIdentifier.RelatedIdentifierNameEnum.ORDERID,
                relatedIdentifierValue = "902-1845936-5435065",
            };

            var queryParameters = parameter.getParameters();
            var keys = queryParameters.Select(kv => kv.Key).ToList();

            Assert.That(keys, Does.Not.Contain("postedAfter"));
            Assert.That(keys, Does.Not.Contain("postedBefore"));
            Assert.That(keys, Does.Not.Contain("nextToken"));
            Assert.That(queryParameters.Single(kv => kv.Key == "relatedIdentifierName").Value, Is.EqualTo("ORDER_ID"));
            Assert.That(queryParameters.Single(kv => kv.Key == "relatedIdentifierValue").Value, Is.EqualTo("902-1845936-5435065"));
        }

        [Test]
        public void GetParameters_WithPostedAfter_UsesIso8601()
        {
            var parameter = new ParameterListFinancialTransactions20240619()
            {
                postedAfter = new DateTime(2026, 6, 1, 0, 0, 0, DateTimeKind.Utc),
            };

            var queryParameters = parameter.getParameters();

            Assert.That(queryParameters.Single(kv => kv.Key == "postedAfter").Value, Is.EqualTo("2026-06-01T00:00:00.000Z"));
        }
    }
}
