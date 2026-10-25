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
    /// Detailed information about the execution of a cancellation for a specific order item.
    /// </summary>
    [DataContract]
    public partial class CancellationExecution : IEquatable<CancellationExecution>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CancellationExecution" /> class.
        /// </summary>
        public CancellationExecution()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CancellationExecution" /> class.
        /// </summary>
        /// <param name="cancelledBy">Entity that executed the cancellation of this item.</param>
        /// <param name="cancelReason">Explanation provided for why the cancellation was executed.</param>
        public CancellationExecution(CancelledByEnum? cancelledBy, string cancelReason)
        {
            this.CancelledBy = cancelledBy;
            this.CancelReason = cancelReason;
        }

        /// <summary>
        /// Entity that executed the cancellation of this item.
        /// </summary>
        /// <value>Entity that executed the cancellation of this item.</value>
        [DataMember(Name = "cancelledBy", EmitDefaultValue = false)]
        public CancelledByEnum? CancelledBy { get; set; }

        /// <summary>
        /// Explanation provided for why the cancellation was executed.
        /// </summary>
        /// <value>Explanation provided for why the cancellation was executed.</value>
        [DataMember(Name = "cancelReason", EmitDefaultValue = false)]
        public string CancelReason { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class CancellationExecution {\n");
            sb.Append("  CancelledBy: ").Append(CancelledBy).Append("\n");
            sb.Append("  CancelReason: ").Append(CancelReason).Append("\n");
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
            return this.Equals(obj as CancellationExecution);
        }

        /// <summary>
        /// Returns true if CancellationExecution instances are equal
        /// </summary>
        /// <param name="input">Instance of CancellationExecution to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(CancellationExecution input)
        {
            if (input == null)
                return false;

            return
                (
                    this.CancelledBy == input.CancelledBy ||
                    (this.CancelledBy != null &&
                    this.CancelledBy.Equals(input.CancelledBy))
                ) &&
                (
                    this.CancelReason == input.CancelReason ||
                    (this.CancelReason != null &&
                    this.CancelReason.Equals(input.CancelReason))
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
                if (this.CancelledBy != null)
                    hashCode = hashCode * 59 + this.CancelledBy.GetHashCode();
                if (this.CancelReason != null)
                    hashCode = hashCode * 59 + this.CancelReason.GetHashCode();
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
