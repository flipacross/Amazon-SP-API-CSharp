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
    /// The payload for the getAccount operation: the seller's account information.
    /// </summary>
    [DataContract]
    public partial class Account
    {
        /// <summary>
        /// The type of business registered for the seller account (for example PRIVATE_LIMITED).
        /// </summary>
        [DataMember(Name = "businessType", EmitDefaultValue = false)]
        public string BusinessType { get; set; }

        /// <summary>
        /// The list of marketplaces the seller participates in.
        /// </summary>
        [DataMember(Name = "marketplaceParticipationList", EmitDefaultValue = false)]
        public MarketplaceParticipationList MarketplaceParticipationList { get; set; }

        /// <summary>
        /// The selling plan of the seller account (for example PROFESSIONAL or INDIVIDUAL).
        /// </summary>
        [DataMember(Name = "sellingPlan", EmitDefaultValue = false)]
        public string SellingPlan { get; set; }

        /// <summary>
        /// The business registration details of the seller account.
        /// </summary>
        [DataMember(Name = "business", EmitDefaultValue = false)]
        public Business Business { get; set; }

        /// <summary>
        /// The primary contact details of the seller account.
        /// </summary>
        [DataMember(Name = "primaryContact", EmitDefaultValue = false)]
        public PrimaryContact PrimaryContact { get; set; }
    }
}
