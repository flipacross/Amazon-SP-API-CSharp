using FikaAmazonAPI.AmazonSpApiSDK.Models.OrdersV20260101;
using FikaAmazonAPI.Parameter.Order.V20260101;

namespace FikaAmazonAPI.SampleCode
{
    /// <summary>
    /// Orders API v2026-01-01 samples (amazonConnection.OrdersV20260101).
    /// Order-level financial data (proceeds, expenses, promotions, taxes) is only returned
    /// when the matching <see cref="IncludedDataEnum"/> value is requested.
    /// </summary>
    public class OrdersV20260101Sample
    {
        AmazonConnection amazonConnection;
        public OrdersV20260101Sample(AmazonConnection amazonConnection)
        {
            this.amazonConnection = amazonConnection;
        }

        /// <summary>
        /// Gets a single order with every included dataset and prints the order-level proceeds breakdowns and packages.
        /// </summary>
        public void GetOrderWithAllData(string orderId)
        {
            var parameter = new ParameterGetOrder
            {
                OrderId = orderId,
                IncludedData = new List<IncludedDataEnum>
                {
                    IncludedDataEnum.BUYER,
                    IncludedDataEnum.RECIPIENT,
                    IncludedDataEnum.PROCEEDS,
                    IncludedDataEnum.EXPENSE,
                    IncludedDataEnum.PROMOTION,
                    IncludedDataEnum.CANCELLATION,
                    IncludedDataEnum.FULFILLMENT,
                    IncludedDataEnum.PACKAGES,
                    IncludedDataEnum.TAX
                }
            };

            var order = amazonConnection.OrdersV20260101.GetOrder(parameter);
            if (order == null)
            {
                Console.WriteLine("Order not found: " + orderId);
                return;
            }

            // Order-level proceeds: grand total plus one entry per breakdown type (for example PRODUCT, SHIPPING, DELIVERY_TIP).
            Console.WriteLine("Proceeds grand total: " + order.Proceeds?.GrandTotal?.Amount + " " + order.Proceeds?.GrandTotal?.CurrencyCode);
            if (order.Proceeds?.Breakdowns != null)
            {
                foreach (var breakdown in order.Proceeds.Breakdowns)
                {
                    Console.WriteLine("  " + breakdown.Type + " [" + breakdown.Status + "] " + breakdown.Subtotal?.Amount + " " + breakdown.Subtotal?.CurrencyCode);
                }
            }

            // Packages are only present when IncludedData contains PACKAGES.
            if (order.Packages != null)
            {
                foreach (var package in order.Packages)
                {
                    Console.WriteLine("Package " + package.PackageReferenceId
                        + " status=" + package.PackageStatus?.Status
                        + " carrier=" + package.Carrier
                        + " tracking=" + package.TrackingNumber);
                }
            }
        }

        /// <summary>
        /// Fetches one page at a time with SearchOrdersByNextTokenAsync and follows Pagination.NextToken.
        /// Use this when you want to process or persist each page before requesting the next one.
        /// </summary>
        public async Task SearchOrdersOnePageAtATime(DateTime lastUpdatedAfterUtc)
        {
            var parameter = new ParameterSearchOrders
            {
                LastUpdatedAfter = lastUpdatedAfterUtc,
                // SearchOrdersByNextToken does not default the marketplace like SearchOrders does.
                MarketplaceIds = new List<string> { amazonConnection.Credentials.MarketPlace.ID },
                FulfilledBy = new List<FulfilledByEnum> { FulfilledByEnum.MERCHANT },
                IncludedData = new List<IncludedDataEnum> { IncludedDataEnum.PROCEEDS, IncludedDataEnum.PACKAGES, IncludedDataEnum.TAX },
                MaxResultsPerPage = 100
            };

            int page = 0;
            string? nextToken;
            do
            {
                var response = await amazonConnection.OrdersV20260101.SearchOrdersByNextTokenAsync(parameter);
                page++;
                Console.WriteLine("Page " + page + ": " + (response?.Orders?.Count ?? 0) + " orders");

                // The last page has no NextToken. Otherwise send the same filters again with the token.
                nextToken = response?.Pagination?.NextToken;
                parameter.PaginationToken = nextToken;
            } while (!string.IsNullOrEmpty(nextToken));
        }

        /// <summary>
        /// Registers a hook that receives every successful (2xx) raw response body before it is deserialized.
        /// Useful for storing the original payload next to the parsed result. The content can contain buyer PII.
        /// </summary>
        public void CaptureRawPayloads()
        {
            amazonConnection.Credentials.OnRawResponse = ctx => Console.WriteLine(ctx.Resource + " " + (ctx.Content?.Length ?? 0));
        }
    }
}
