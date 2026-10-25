namespace FikaAmazonAPI.Utils
{
    /// <summary>
    /// Raw 2xx response handed to <see cref="AmazonCredential.OnRawResponse"/>.
    /// <see cref="Content"/> is the unparsed response body and may contain buyer PII.
    /// </summary>
    public sealed class RawResponseContext
    {
        /// <summary>Request resource (relative path, without the base URL or query string).</summary>
        public string Resource { get; set; }

        /// <summary>HTTP method of the request (e.g. "Get", "Post").</summary>
        public string Method { get; set; }

        /// <summary>HTTP status code of the response (always 2xx).</summary>
        public int StatusCode { get; set; }

        /// <summary>Raw response body; may be null or empty, and may contain PII.</summary>
        public string Content { get; set; }
    }
}
