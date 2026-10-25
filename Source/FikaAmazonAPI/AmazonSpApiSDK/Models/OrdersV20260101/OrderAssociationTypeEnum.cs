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
    /// The relationship between the current order and the associated order. Possible values: REPLACEMENT_ORIGINAL_ID, EXCHANGE_ORIGINAL_ID
    /// </summary>
    /// <value>The relationship between the current order and the associated order. Possible values: REPLACEMENT_ORIGINAL_ID, EXCHANGE_ORIGINAL_ID</value>
    [JsonConverter(typeof(SafeStringEnumConverter))]
    public enum OrderAssociationTypeEnum
    {
        /// <summary>
        /// Value for REPLACEMENT_ORIGINAL_ID
        /// </summary>
        [EnumMember(Value = "REPLACEMENT_ORIGINAL_ID")]
        REPLACEMENT_ORIGINAL_ID = 1,

        /// <summary>
        /// Value for EXCHANGE_ORIGINAL_ID
        /// </summary>
        [EnumMember(Value = "EXCHANGE_ORIGINAL_ID")]
        EXCHANGE_ORIGINAL_ID = 2,

    }
}
