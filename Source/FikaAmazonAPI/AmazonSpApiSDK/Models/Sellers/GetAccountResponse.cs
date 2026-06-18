/*
 * Selling Partner API for Sellers
 *
 * The Selling Partner API for Sellers lets you retrieve information on behalf of sellers about their seller account.
 *
 * OpenAPI spec version: v1
 */

using System.Runtime.Serialization;

namespace FikaAmazonAPI.AmazonSpApiSDK.Models.Sellers
{
    /// <summary>
    /// The response schema for the getAccount operation.
    /// </summary>
    [DataContract]
    public partial class GetAccountResponse
    {
        /// <summary>
        /// The payload for the getAccount operation.
        /// </summary>
        [DataMember(Name = "payload", EmitDefaultValue = false)]
        public Account Payload { get; set; }

        /// <summary>
        /// Encountered errors for the getAccount operation.
        /// </summary>
        [DataMember(Name = "errors", EmitDefaultValue = false)]
        public ErrorList Errors { get; set; }
    }
}
