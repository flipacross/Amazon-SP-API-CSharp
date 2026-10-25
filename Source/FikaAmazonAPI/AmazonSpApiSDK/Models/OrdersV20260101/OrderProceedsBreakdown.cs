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
    /// A single order-level component of the proceeds, such as items, shipping, tax or a delivery tip.
    /// </summary>
    [DataContract]
    public partial class OrderProceedsBreakdown : IEquatable<OrderProceedsBreakdown>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OrderProceedsBreakdown" /> class.
        /// </summary>
        public OrderProceedsBreakdown()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderProceedsBreakdown" /> class.
        /// </summary>
        /// <param name="type">Category classification of the proceeds breakdown.</param>
        /// <param name="status">Status of the proceeds component. Currently only returned for DELIVERY_TIP breakdowns (PENDING or FINALIZED).</param>
        /// <param name="subtotal">An amount of money, including units in the form of currency.</param>
        public OrderProceedsBreakdown(ProceedsBreakdownTypeEnum? type, string status, Money subtotal)
        {
            this.Type = type;
            this.Status = status;
            this.Subtotal = subtotal;
        }

        /// <summary>
        /// Category classification of the proceeds breakdown.
        /// </summary>
        /// <value>Category classification of the proceeds breakdown.</value>
        [DataMember(Name = "type", EmitDefaultValue = false)]
        public ProceedsBreakdownTypeEnum? Type { get; set; }

        /// <summary>
        /// Status of the proceeds component. Currently only returned for DELIVERY_TIP breakdowns (PENDING or FINALIZED).
        /// </summary>
        /// <value>Status of the proceeds component. Currently only returned for DELIVERY_TIP breakdowns (PENDING or FINALIZED).</value>
        [DataMember(Name = "status", EmitDefaultValue = false)]
        public string Status { get; set; }

        /// <summary>
        /// An amount of money, including units in the form of currency.
        /// </summary>
        /// <value>An amount of money, including units in the form of currency.</value>
        [DataMember(Name = "subtotal", EmitDefaultValue = false)]
        public Money Subtotal { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class OrderProceedsBreakdown {\n");
            sb.Append("  Type: ").Append(Type).Append("\n");
            sb.Append("  Status: ").Append(Status).Append("\n");
            sb.Append("  Subtotal: ").Append(Subtotal).Append("\n");
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
            return this.Equals(obj as OrderProceedsBreakdown);
        }

        /// <summary>
        /// Returns true if OrderProceedsBreakdown instances are equal
        /// </summary>
        /// <param name="input">Instance of OrderProceedsBreakdown to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(OrderProceedsBreakdown input)
        {
            if (input == null)
                return false;

            return
                (
                    this.Type == input.Type ||
                    (this.Type != null &&
                    this.Type.Equals(input.Type))
                ) &&
                (
                    this.Status == input.Status ||
                    (this.Status != null &&
                    this.Status.Equals(input.Status))
                ) &&
                (
                    this.Subtotal == input.Subtotal ||
                    (this.Subtotal != null &&
                    this.Subtotal.Equals(input.Subtotal))
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
                if (this.Type != null)
                    hashCode = hashCode * 59 + this.Type.GetHashCode();
                if (this.Status != null)
                    hashCode = hashCode * 59 + this.Status.GetHashCode();
                if (this.Subtotal != null)
                    hashCode = hashCode * 59 + this.Subtotal.GetHashCode();
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
