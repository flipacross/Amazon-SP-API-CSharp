using FikaAmazonAPI.AmazonSpApiSDK.Models.OrdersV20260101;
using Newtonsoft.Json;
using System.IO;

namespace Tests
{
    [TestFixture]
    public class OrdersV20260101Tests
    {
        static string Fixture(string name) =>
            File.ReadAllText(Path.Combine(TestContext.CurrentContext.TestDirectory, "Fixtures", name));

        [Test]
        public void UnknownEnumValues_DoNotFailDeserialization()
        {
            var json = Fixture("orders-v2026-getorder-uk-doc-example.json")
                .Replace("\"programs\": [\n      \"PRIME\"\n    ]", "\"programs\": [\"SOME_FUTURE_PROGRAM\", \"PRIME\"]")
                .Replace("\"fulfillmentServiceLevel\": \"STANDARD\"", "\"fulfillmentServiceLevel\": \"HYPERDRIVE\"")
                .Replace("\"fulfillmentStatus\": \"SHIPPED\"", "\"fulfillmentStatus\": \"WARPED\"");
            GetOrderResponse? response = null;
            Assert.DoesNotThrow(() => response = JsonConvert.DeserializeObject<GetOrderResponse>(json));
            var order = response!.Order;
            Assert.That(order.Programs, Does.Contain(ProgramEnum.PRIME));
            Assert.That(order.Programs, Does.Contain(default(ProgramEnum)));          // unknown -> 0
            Assert.That(order.Fulfillment.FulfillmentServiceLevel, Is.Null);        // nullable -> null
            Assert.That(order.Fulfillment.FulfillmentStatus, Is.EqualTo(default(FulfillmentStatusEnum)));
        }
    }
}
