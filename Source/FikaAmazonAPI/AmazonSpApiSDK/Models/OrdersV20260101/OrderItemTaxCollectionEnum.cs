using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using FikaAmazonAPI.Utils;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace FikaAmazonAPI.AmazonSpApiSDK.Models.OrdersV20260101
{
    /// <summary>
    /// Specifies the tax collection model applied to this order item.
    /// </summary>
    [JsonConverter(typeof(SafeStringEnumConverter))]
    public enum OrderItemTaxCollectionEnum
    {
        /// <summary>
        /// Tax is collected by the marketplace facilitator.
        /// </summary>
        [EnumMember(Value = "MARKETPLACE_FACILITATOR")]
        MARKETPLACE_FACILITATOR = 1
    }
}
