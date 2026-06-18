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
    /// The primary contact details of the seller account.
    /// </summary>
    [DataContract]
    public partial class PrimaryContact
    {
        /// <summary>
        /// The primary contact name.
        /// </summary>
        [DataMember(Name = "name", EmitDefaultValue = false)]
        public string Name { get; set; }

        /// <summary>
        /// The non-Latin script primary contact name, when applicable.
        /// </summary>
        [DataMember(Name = "nonLatinName", EmitDefaultValue = false)]
        public string NonLatinName { get; set; }

        /// <summary>
        /// The primary contact address.
        /// </summary>
        [DataMember(Name = "address", EmitDefaultValue = false)]
        public Address Address { get; set; }
    }
}
