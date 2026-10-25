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
    /// Entity that executed the cancellation of this item.
    /// </summary>
    /// <value>Entity that executed the cancellation of this item.</value>
    [JsonConverter(typeof(SafeStringEnumConverter))]
    public enum CancelledByEnum
    {
        /// <summary>
        /// Enum value for type BUYER
        /// </summary>
        [EnumMember(Value = "BUYER")]
        BUYER = 1,

        /// <summary>
        /// Enum value for type MERCHANT
        /// </summary>
        [EnumMember(Value = "MERCHANT")]
        MERCHANT = 2,

        /// <summary>
        /// Enum value for type AMAZON
        /// </summary>
        [EnumMember(Value = "AMAZON")]
        AMAZON = 3,

    }

}
