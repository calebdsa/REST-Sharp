// using Microsoft.VisualStudio.TestTools.UnitTesting;
// using RestSharp;
// using Newtonsoft.Json;
// using System.Net;

// namespace ApiTestingDemo
// {
//     // ✅ POCO class for single user
//     public class User
//     {
//         public int Id { get; set; }
//         public string? Email { get; set; }

//         [JsonProperty("first_name")]
//         public string? First_Name { get; set; }

//         [JsonProperty("last_name")]
//         public string? Last_Name { get; set; }

//         public string? Avatar { get; set; }
//     }

//     // ✅ Wrapper class for GET user response
//     public class UserResponse
//     {
//         public User? Data { get; set; }
//     }

//     [TestClass]
//     public class Test1
//     {
//         private RestClient client = null!;

//         [TestInitialize]
//         public void Setup()
//         {
//             // ✅ Base URL for Reqres API
//             client = new RestClient("https://reqres.in");
//         }

//         // ✅ Test 1 - GET User
//         [TestMethod]
//         public void GetUser_ShouldReturnValidData()
//         {
//             var request = new RestRequest("/api/users/2", Method.Get);
//             var response = client.Execute(request);

//             var userResponse = JsonConvert.DeserializeObject<UserResponse>(response.Content!)!;

//             Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "Status code is not 200 OK");
//             Assert.IsNotNull(userResponse.Data, "User data is null");
//             Assert.AreEqual(2, userResponse.Data!.Id, "User ID does not match");

//             Console.WriteLine($"✅ GET Test Passed: {userResponse.Data.First_Name} {userResponse.Data.Last_Name}");
//         }

//         // ✅ Test 2 - POST Create User
//         [TestMethod]
//         public void CreateUser_ShouldReturn201()
//         {
//             var request = new RestRequest("/api/users", Method.Post);
//             request.AddHeader("Content-Type", "application/json");
//             request.AddHeader("Authorization", "reqres-free-v1");
//             request.AddJsonBody(new { name = "Jane Doe", job = "QA Tester" });

//             var response = client.Execute(request);
//             Console.WriteLine("Response: " + response.Content);

//             Assert.AreEqual(HttpStatusCode.Created, response.StatusCode, "Status code is not 201 Created");

//             dynamic data = JsonConvert.DeserializeObject(response.Content!)!;
//             Assert.AreEqual("Jane Doe", (string)data.name, "Name mismatch");
//             Assert.AreEqual("QA Tester", (string)data.job, "Job mismatch");

//             Console.WriteLine($"✅ POST Test Passed: ID = {data.id}, CreatedAt = {data.createdAt}");
//         }

//         // ✅ Test 3 - GET All Users
//         [TestMethod]
//         public void GetAllUsers_ShouldContainData()
//         {
//             var request = new RestRequest("/api/users?page=2", Method.Get);
//             var response = client.Execute(request);

//             dynamic data = JsonConvert.DeserializeObject(response.Content!)!;

//             Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "Status code is not 200 OK");
//             Assert.IsTrue(data.data.Count > 0, "No users found in response");

//             Console.WriteLine($"✅ GET All Users Passed: Found {data.data.Count} users");
//         }
//     }
// }
