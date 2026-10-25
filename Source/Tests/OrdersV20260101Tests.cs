using FikaAmazonAPI.AmazonSpApiSDK.Models.OrdersV20260101;
using FikaAmazonAPI.Services;
using Newtonsoft.Json;
using System.IO;

namespace Tests
{
    [TestFixture]
    public class OrdersV20260101Tests
    {
        static string Fixture(string name) =>
            File.ReadAllText(Path.Combine(TestContext.CurrentContext.TestDirectory, "Fixtures", name));

        [Test]
        public void UnknownEnumValues_DoNotFailDeserialization()
        {
            var json = Fixture("orders-v2026-getorder-uk-doc-example.json")
                .Replace("\"programs\": [\n      \"PRIME\"\n    ]", "\"programs\": [\"SOME_FUTURE_PROGRAM\", \"PRIME\"]")
                .Replace("\"fulfillmentServiceLevel\": \"STANDARD\"", "\"fulfillmentServiceLevel\": \"HYPERDRIVE\"")
                .Replace("\"fulfillmentStatus\": \"SHIPPED\"", "\"fulfillmentStatus\": \"WARPED\"");
            GetOrderResponse? response = null;
            Assert.DoesNotThrow(() => response = JsonConvert.DeserializeObject<GetOrderResponse>(json));
            var order = response!.Order;
            Assert.That(order.Programs, Does.Contain(ProgramEnum.PRIME));
            Assert.That(order.Programs, Does.Contain(default(ProgramEnum)));          // unknown -> 0
            Assert.That(order.Fulfillment.FulfillmentServiceLevel, Is.Null);        // nullable -> null
            Assert.That(order.Fulfillment.FulfillmentStatus, Is.EqualTo(default(FulfillmentStatusEnum)));
        }

        [Test]
        public void UkDocExample_DeserializesOrderLevelProceedsBreakdowns()
        {
            var order = JsonConvert.DeserializeObject<GetOrderResponse>(Fixture("orders-v2026-getorder-uk-doc-example.json"))!.Order;
            Assert.That(order.Proceeds.GrandTotal.Amount, Is.EqualTo(103.97m));
            Assert.That(order.Proceeds.Breakdowns, Has.Count.EqualTo(3));
            Assert.That(order.Proceeds.Breakdowns.Single(b => b.Type == ProceedsBreakdownTypeEnum.ITEM).Subtotal.Amount, Is.EqualTo(89.97m));
            Assert.That(order.Proceeds.Breakdowns.Single(b => b.Type == ProceedsBreakdownTypeEnum.TAX).Subtotal.Amount, Is.EqualTo(4.00m));
        }

        [Test]
        public void CancellationExecution_Deserializes()
        {
            const string json = @"{""cancellationRequest"":{""requester"":""BUYER"",""cancelReason"":""Changed mind""},""cancellationExecution"":{""cancelledBy"":""MERCHANT"",""cancelReason"":""Out of stock""}}";
            var cancellation = JsonConvert.DeserializeObject<Cancellation>(json)!;
            Assert.That(cancellation.CancellationRequest.Requester, Is.EqualTo(CancellationRequesterEnum.BUYER));
            Assert.That(cancellation.CancellationExecution.CancelledBy, Is.EqualTo(CancelledByEnum.MERCHANT));
            Assert.That(cancellation.CancellationExecution.CancelReason, Is.EqualTo("Out of stock"));
        }

        [Test]
        public void DeliveryTipBreakdown_Deserializes()
        {
            const string json = @"{""grandTotal"":{""amount"":""5.00"",""currencyCode"":""USD""},""breakdowns"":[{""type"":""DELIVERY_TIP"",""status"":""PENDING"",""subtotal"":{""amount"":""5.00"",""currencyCode"":""USD""}}]}";
            var proceeds = JsonConvert.DeserializeObject<OrderProceeds>(json)!;
            Assert.That(proceeds.Breakdowns[0].Type, Is.EqualTo(ProceedsBreakdownTypeEnum.DELIVERY_TIP));
            Assert.That(proceeds.Breakdowns[0].Status, Is.EqualTo("PENDING"));
        }

        [TestCase("0.0056", 0.0056)]
        [TestCase("0.5", 0.5)]
        [TestCase("abc", null)]
        [TestCase("", null)]
        public void ParseRateLimitHeader_ParsesInvariantOrNull(string value, double? expected)
        {
            decimal? expectedValue = expected.HasValue ? (decimal?)(decimal)expected.Value : null;
            Assert.That(RequestService.ParseRateLimitHeader(value), Is.EqualTo(expectedValue));
        }
    }
}
