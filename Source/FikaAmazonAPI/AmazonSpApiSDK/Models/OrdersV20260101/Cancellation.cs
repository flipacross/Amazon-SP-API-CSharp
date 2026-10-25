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
    /// The cancellation information of the order item.
    /// </summary>
    [DataContract]
    public partial class Cancellation : IEquatable<Cancellation>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Cancellation" /> class.
        /// </summary>
        public Cancellation()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Cancellation" /> class.
        /// </summary>
        /// <param name="cancellationRequest">Detailed information about a cancellation request submitted for a specific order item.</param>
        /// <param name="cancellationExecution">Detailed information about the execution of a cancellation for a specific order item.</param>
        public Cancellation(CancellationRequest cancellationRequest, CancellationExecution cancellationExecution)
        {
            this.CancellationRequest = cancellationRequest;
            this.CancellationExecution = cancellationExecution;
        }

        /// <summary>
        /// Detailed information about a cancellation request submitted for a specific order item.
        /// </summary>
        /// <value>Detailed information about a cancellation request submitted for a specific order item.</value>
        [DataMember(Name = "cancellationRequest", EmitDefaultValue = false)]
        public CancellationRequest CancellationRequest { get; set; }

        /// <summary>
        /// Detailed information about the execution of a cancellation for a specific order item.
        /// </summary>
        /// <value>Detailed information about the execution of a cancellation for a specific order item.</value>
        [DataMember(Name = "cancellationExecution", EmitDefaultValue = false)]
        public CancellationExecution CancellationExecution { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Cancellation {\n");
            sb.Append("  CancellationRequest: ").Append(CancellationRequest).Append("\n");
            sb.Append("  CancellationExecution: ").Append(CancellationExecution).Append("\n");
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
            return this.Equals(obj as Cancellation);
        }

        /// <summary>
        /// Returns true if Cancellation instances are equal
        /// </summary>
        /// <param name="input">Instance of Cancellation to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(Cancellation input)
        {
            if (input == null)
                return false;

            return
                (
                    this.CancellationRequest == input.CancellationRequest ||
                    (this.CancellationRequest != null &&
                    this.CancellationRequest.Equals(input.CancellationRequest))
                ) &&
                (
                    this.CancellationExecution == input.CancellationExecution ||
                    (this.CancellationExecution != null &&
                    this.CancellationExecution.Equals(input.CancellationExecution))
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
                if (this.CancellationRequest != null)
                    hashCode = hashCode * 59 + this.CancellationRequest.GetHashCode();
                if (this.CancellationExecution != null)
                    hashCode = hashCode * 59 + this.CancellationExecution.GetHashCode();
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