using FikaAmazonAPI.Search;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using static FikaAmazonAPI.AmazonSpApiSDK.Models.Finances.Model.RelatedIdentifier;

namespace FikaAmazonAPI.Parameter.Finance
{
    public class ParameterListFinancialTransactions20240619 : ParameterBased
    {
        /// <summary>
        /// The response includes financial events posted on or after this date (ISO 8601).
        /// Required if you do not specify relatedIdentifierName/relatedIdentifierValue;
        /// leave null for identifier-only lookups (e.g. ORDER_ID). If postedAfter and
        /// postedBefore are more than 180 days apart, the response is empty.
        /// </summary>
        public DateTime? postedAfter { get; set; }
        public DateTime? postedBefore { get; set; }
        public string? marketplaceId { get; set; }
        /// <summary>
        /// The status of the transaction.Possible values:
        /// `DEFERRED`: the transaction is currently deferred.
        /// `RELEASED`: the transaction is currently released. 
        /// `DEFERRED_RELEASED`: the transaction was deferred in the past, but is now released. The status of a deferred transaction is updated to `DEFERRED_RELEASED` when the transaction is released.
        /// </summary>
        public string? transactionStatus { get; set; }
        /// <summary>
        /// The identifier name to filter by. FINANCIAL_EVENT_GROUP_ID and ORDER_ID have filtering
        /// capability; other values appear in response payloads but cannot be used as query filters.
        /// </summary>
        public RelatedIdentifierNameEnum? relatedIdentifierName { get; set; }
        public string? relatedIdentifierValue { get; set; }
        public string nextToken { get; set; }
        public int? MaxNumberOfPages { get; set; }
    }
}
