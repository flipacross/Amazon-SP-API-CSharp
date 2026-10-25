using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace FikaAmazonAPI.AmazonSpApiSDK.Models.OrdersV20260101
{
    /// <summary>
    /// Details about how the tax for an order item was calculated, such as the VAT reporting scheme that applies.
    /// </summary>
    [DataContract]
    public partial class OrderItemTaxCalculationBreakdown : IEquatable<OrderItemTaxCalculationBreakdown>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OrderItemTaxCalculationBreakdown" /> class.
        /// </summary>
        public OrderItemTaxCalculationBreakdown()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderItemTaxCalculationBreakdown" /> class.
        /// </summary>
        /// <param name="reportingScheme">The VAT reporting scheme that applies to the tax calculation. Possible values: UOSS, IOSS.</param>
        public OrderItemTaxCalculationBreakdown(string reportingScheme)
        {
            this.ReportingScheme = reportingScheme;
        }

        /// <summary>
        /// The VAT reporting scheme that applies to the tax calculation. Possible values: UOSS, IOSS.
        /// </summary>
        /// <value>The VAT reporting scheme that applies to the tax calculation. Possible values: UOSS, IOSS.</value>
        [DataMember(Name = "reportingScheme", EmitDefaultValue = false)]
        public string ReportingScheme { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class OrderItemTaxCalculationBreakdown {\n");
            sb.Append("  ReportingScheme: ").Append(ReportingScheme).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public virtual string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="obj">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object obj)
        {
            return this.Equals(obj as OrderItemTaxCalculationBreakdown);
        }

        /// <summary>
        /// Returns true if OrderItemTaxCalculationBreakdown instances are equal
        /// </summary>
        /// <param name="input">Instance of OrderItemTaxCalculationBreakdown to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(OrderItemTaxCalculationBreakdown input)
        {
            if (input == null)
                return false;

            return
                (
                    this.ReportingScheme == input.ReportingScheme ||
                    (this.ReportingScheme != null &&
                    this.ReportingScheme.Equals(input.ReportingScheme))
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hashCode = 41;
                if (this.ReportingScheme != null)
                    hashCode = hashCode * 59 + this.ReportingScheme.GetHashCode();
                return hashCode;
            }
        }

        /// <summary>
        /// To validate all properties of the instance
        /// </summary>
        /// <param name="validationContext">Validation context</param>
        /// <returns>Validation Result</returns>
        IEnumerable<System.ComponentModel.DataAnnotations.ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            yield break;
        }
    }

}
