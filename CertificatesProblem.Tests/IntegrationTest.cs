using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CertificatesProblem.Dtos.Results;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;
using NUnit.Framework;

namespace CertificatesProblem.Tests
{
    /// <summary>
    /// Для проверки регресса. Запускает набор подготовленных запросов по одному на каждую стратегию решения. Проверяет правильность ответов.
    /// Запустить приложение, в тестах указать BaseAddress (по умолчанию указан http://localhost:5000/), убрать Ignore, запустить тесты
    /// </summary>
    [TestFixture]
    //[Ignore("Integration")]
    public class IntegrationTest
    {
        private readonly HttpClient _httpClient;

        public IntegrationTest()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5000/"),
                Timeout = TimeSpan.FromSeconds(3)
            };
        }

        [Test]
        [TestCase("LessNodesToVisitRequestBody", "LessNodesToVisitResponse")]
        [TestCase("LessMoneyCostRequestBody", "LessMoneyCostResponse")]
        [TestCase("LessTimeCostSerialVisitsRequestBody", "LessTimeCostSerialVisitsResponse")]
        [TestCase("LessTimeCostParallelVisitsRequestBody", "LessTimeCostParallelVisitsResponse")]
        public async Task Solve_ForEachStrategy_CorrectSolution(string requestBodyJsonName, string responseJsonName)
        {
            var requestUri = "certificatesproblem/solve";

            var requestBody = GetEmbeddedResource($"IntegrationTestJson\\{requestBodyJsonName}.json");
            var expectedJson = GetEmbeddedResource($"IntegrationTestJson\\{responseJsonName}.json");
            var expected = JsonConvert.DeserializeObject<CertificatesProblemSolution>(expectedJson);

            var response = await _httpClient.PostAsync(requestUri, new StringContent(requestBody, Encoding.UTF8, "application/json"));

            var responseJson = await response.Content.ReadAsStringAsync();
            var result= JsonConvert.DeserializeObject<CertificatesProblemSolution>(responseJson);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(string.Join("; ", expected.SolutionsAsEquation.OrderBy(x => x.CertificateId)), 
                string.Join("; ", result.SolutionsAsEquation.OrderBy(x => x.CertificateId)));
        }

        private string GetEmbeddedResource(string embeddedResourceName)
        {
            var embeddedProvider = new EmbeddedFileProvider(typeof(IntegrationTest).GetTypeInfo().Assembly);
            using (var stream = embeddedProvider.GetFileInfo(embeddedResourceName).CreateReadStream())
                using (var reader = new StreamReader(stream, Encoding.UTF8))
                    return reader.ReadToEnd();
        }
    }
}