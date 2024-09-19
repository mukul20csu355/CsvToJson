using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using CsvHelper;
using System.Globalization;

public static class CsvToJsonConverter
{
    [FunctionName("CsvToJsonConverter")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
        ILogger log)
    {
        log.LogInformation("C# HTTP trigger function processed a request.");

        // Read CSV content from the request body
        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

        // Convert CSV to JSON
        var jsonResult = ConvertCsvToJson(requestBody);

        // Return JSON response
        return new OkObjectResult(jsonResult);
    }

    private static string ConvertCsvToJson(string csvContent)
    {
        using (var reader = new StringReader(csvContent))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            var records = csv.GetRecords<dynamic>();
            return JsonConvert.SerializeObject(records, Formatting.Indented);
        }
    }
}
