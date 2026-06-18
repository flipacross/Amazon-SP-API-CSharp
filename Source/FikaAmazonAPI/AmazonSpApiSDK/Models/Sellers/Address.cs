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
    /// A physical address (used for the registered business address and the primary contact address).
    /// </summary>
    [DataContract]
    public partial class Address
    {
        /// <summary>
        /// The street address.
        /// </summary>
        [DataMember(Name = "addressLine1", EmitDefaultValue = false)]
        public string AddressLine1 { get; set; }

        /// <summary>
        /// Additional street address information.
        /// </summary>
        [DataMember(Name = "addressLine2", EmitDefaultValue = false)]
        public string AddressLine2 { get; set; }

        /// <summary>
        /// The city.
        /// </summary>
        [DataMember(Name = "city", EmitDefaultValue = false)]
        public string City { get; set; }

        /// <summary>
        /// The state or province code.
        /// </summary>
        [DataMember(Name = "stateOrProvinceCode", EmitDefaultValue = false)]
        public string StateOrProvinceCode { get; set; }

        /// <summary>
        /// The postal code.
        /// </summary>
        [DataMember(Name = "postalCode", EmitDefaultValue = false)]
        public string PostalCode { get; set; }

        /// <summary>
        /// The ISO 3166-1 alpha-2 format country code.
        /// </summary>
        [DataMember(Name = "countryCode", EmitDefaultValue = false)]
        public string CountryCode { get; set; }
    }
}
