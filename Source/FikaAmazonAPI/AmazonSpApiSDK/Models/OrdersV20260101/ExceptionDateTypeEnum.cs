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
    /// ExceptionDateTypeEnum
    /// </summary>
    /// <value>ExceptionDateTypeEnum</value>
    [JsonConverter(typeof(SafeStringEnumConverter))]
    public enum ExceptionDateTypeEnum
    {
        /// <summary>
        /// Enum value for type CLOSED
        /// </summary>
        [EnumMember(Value = "CLOSED")]
        CLOSED = 1,

        /// <summary>
        /// Enum value for type OPEN
        /// </summary>
        [EnumMember(Value = "OPEN")]
        OPEN = 2,

    }

}
