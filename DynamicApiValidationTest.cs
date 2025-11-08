using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ApiTestingDemo
{
    [TestClass]
    public class DynamicApiValidationTest
    {
        private static readonly HttpClient client = new HttpClient();
        private const string baseUrl = "https://api.restful-api.dev/objects";
        private const string apiKey = "reqres-free-v1";
        
        private static string createdObjectId = string.Empty;

        [TestInitialize]
        public void Setup()
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("x-api-key", apiKey);
        }

        [TestMethod]
        public async Task Dynamic_POST_GET_PUT_Validation()
        {
            // 🔹 Step 1: POST → Create a new object
            var newObject = new
            {
                name = "Dynamic Validation Object",
                data = new { brand = "HP", year = 2025, price = 999.99 }
            };

            var postContent = new StringContent(System.Text.Json.JsonSerializer.Serialize(newObject), Encoding.UTF8, "application/json");
            var postResponse = await client.PostAsync(baseUrl, postContent);
            var postBody = await postResponse.Content.ReadAsStringAsync();
            System.Console.WriteLine($"POST Response: {postBody}");

            Assert.IsTrue(postResponse.StatusCode == System.Net.HttpStatusCode.Created ||
                          postResponse.StatusCode == System.Net.HttpStatusCode.OK,
                          $"POST failed - Expected 201 Created or 200 OK, but got {postResponse.StatusCode}");

            var postJson = JObject.Parse(postBody ?? "{}");
            createdObjectId = postJson["id"]?.ToString() ?? string.Empty;
            Assert.IsFalse(string.IsNullOrEmpty(createdObjectId), "❌ Object ID was not captured from POST response");

            // 🔹 Step 2: GET → Validate that the object exists
            var getResponse = await client.GetAsync($"{baseUrl}/{createdObjectId}");
            var getBody = await getResponse.Content.ReadAsStringAsync();
            System.Console.WriteLine($"GET Response: {getBody}");

            Assert.AreEqual(System.Net.HttpStatusCode.OK, getResponse.StatusCode, "GET failed - Expected 200 OK");

            var getJson = JObject.Parse(getBody ?? "{}"); 
            Assert.AreEqual("Dynamic Validation Object", getJson["name"]?.ToString(), "❌ Object name does not match POST data");

            // 🔹 Step 3: PUT → Update the same object
            var updatedObject = new
            {
                name = "Dynamic Validation Object - Updated",
                data = new { brand = "HP", year = 2026, price = 1099.99 }
            };

            var putContent = new StringContent(System.Text.Json.JsonSerializer.Serialize(updatedObject), Encoding.UTF8, "application/json");
            var putResponse = await client.PutAsync($"{baseUrl}/{createdObjectId}", putContent);
            var putBody = await putResponse.Content.ReadAsStringAsync();
            System.Console.WriteLine($"PUT Response: {putBody}");

            Assert.AreEqual(System.Net.HttpStatusCode.OK, putResponse.StatusCode, "PUT failed - Expected 200 OK");

            var putJson = JObject.Parse(putBody ?? "{}");
            Assert.AreEqual("Dynamic Validation Object - Updated", putJson["name"]?.ToString(), "❌ Object name not updated successfully");
        }
    }
}
