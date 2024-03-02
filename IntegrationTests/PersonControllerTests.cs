using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;
using Skills.Models.Dto;
using Skills.Models;
using System.Net.Http.Json;



namespace Skills.IntegrationTests
{
    [TestFixture]
    public class PersonControllerTests
    {
        private readonly string _controllerRoute = "api/v1/persons";

        private WebApplicationFactory<Startup> _host;

        private HttpClient _httpClient;

        [OneTimeSetUp]
        public void Init()
        {
            _host = new WebApplicationFactory<Startup>().WithWebHostBuilder(w => { });
            _httpClient = _host.CreateClient();

            Console.WriteLine("INIT INIT INIT INIT INIT INIT INIT INIT ");
        }

        [Test]
        public async Task GetAllPersons_ShouldReturnOK()
        {
            // Act

            var response = await _httpClient.GetAsync($"{_controllerRoute}");

            // Assert

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task GetPersonById_ShouldReturnBadRequest()
        {
            var response = await _httpClient.GetAsync($"{_controllerRoute}/HHFHF");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task GetPersonById_ShouldReturnNotFound()
        {
            var response = await _httpClient.GetAsync($"{_controllerRoute}/{long.MaxValue}");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task DeletePerson_ShouldReturnNotFound()
        {
            var response = await _httpClient.DeleteAsync($"{_controllerRoute}/{long.MaxValue}");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task CreatePerson_ShouldReturnOK()
        {
            var person = new PersonDto
            {
                Name = "testName",
                DisplayName = "testDisplayName",
                Skills = new List<Skill> { new Skill { Name = "PHP", Level = 10 } }
            };

            var content = JsonContent.Create(person);

            var response = await _httpClient.PostAsync($"{_controllerRoute}", content);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task CreatePerson_SendRequestWithoutSkills_ShouldReturnOK()
        {
            var person = new PersonDto
            {
                Name = "testName",
                DisplayName = "testDisplayName"
            };

            var content = JsonContent.Create(person);

            var response = await _httpClient.PostAsync($"{_controllerRoute}", content);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task CreatePerson_SkillLevelGreaterThanTen_ShouldReturnBadRequest()
        {
            var person = new PersonDto
            {
                Name = "testName",
                DisplayName = "testDisplayName",
                Skills = new List<Skill> { new Skill { Name = "PHP", Level = 11 } }
            };

            var content = JsonContent.Create(person);

            var response = await _httpClient.PostAsync($"{_controllerRoute}", content);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task CreatePerson_SkillLevelLessThanOne_ShouldReturnBadRequest()
        {
            var person = new PersonDto
            {
                Name = "testName",
                DisplayName = "testDisplayName",
                Skills = new List<Skill> { new Skill { Name = "PHP", Level = 0 } }
            };

            var content = JsonContent.Create(person);

            var response = await _httpClient.PostAsync($"{_controllerRoute}", content);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }
    }
}