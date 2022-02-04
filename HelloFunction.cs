using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using System.IO;
using System.Threading.Tasks;

namespace AzureFunctions.Proxies
{
	public static class HelloFunction
	{
		[FunctionName(nameof(HelloCustomer))]
		public static async Task<IActionResult> HelloCustomer(
			[HttpTrigger(AuthorizationLevel.Function, "get", Route = "v1/hello")] HttpRequest req,
			ILogger log)
		{
			log.LogInformation("C# HTTP trigger function processed a request.");

			string name = req.Query["name"];

			string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
			dynamic data = JsonConvert.DeserializeObject(requestBody);
			name = name ?? data?.name;

			string responseMessage = string.IsNullOrEmpty(name)
				? "Pass a name in the query string or in the request body for a personalized response."
				: $"Hello, {name}.";

			return new OkObjectResult(new { responseMessage });
		}
	}
}
