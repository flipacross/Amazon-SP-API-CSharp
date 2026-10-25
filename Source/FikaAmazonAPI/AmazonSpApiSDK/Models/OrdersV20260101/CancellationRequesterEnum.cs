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
    /// CancellationRequesterEnum
    /// </summary>
    /// <value>ExceptionDateTypeEnum</value>
    [JsonConverter(typeof(SafeStringEnumConverter))]
    public enum CancellationRequesterEnum
    {
        /// <summary>
        /// Enum value for type BUYER
        /// </summary>
        [EnumMember(Value = "BUYER")]
        BUYER = 1,

    }

}
