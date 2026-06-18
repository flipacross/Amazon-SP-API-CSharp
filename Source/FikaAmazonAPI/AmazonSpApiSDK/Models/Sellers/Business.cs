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
    /// The business registration details of the seller account.
    /// </summary>
    [DataContract]
    public partial class Business
    {
        /// <summary>
        /// The business name.
        /// </summary>
        [DataMember(Name = "name", EmitDefaultValue = false)]
        public string Name { get; set; }

        /// <summary>
        /// The non-Latin script business name, when applicable.
        /// </summary>
        [DataMember(Name = "nonLatinName", EmitDefaultValue = false)]
        public string NonLatinName { get; set; }

        /// <summary>
        /// The registered business address.
        /// </summary>
        [DataMember(Name = "registeredBusinessAddress", EmitDefaultValue = false)]
        public Address RegisteredBusinessAddress { get; set; }

        /// <summary>
        /// The company registration number.
        /// </summary>
        [DataMember(Name = "companyRegistrationNumber", EmitDefaultValue = false)]
        public string CompanyRegistrationNumber { get; set; }

        /// <summary>
        /// The company tax identification number.
        /// </summary>
        [DataMember(Name = "companyTaxIdentificationNumber", EmitDefaultValue = false)]
        public string CompanyTaxIdentificationNumber { get; set; }
    }
}
