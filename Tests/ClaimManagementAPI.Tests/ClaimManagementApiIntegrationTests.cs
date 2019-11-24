using ClaimManagement.Application.Claims.Commands;
using ClaimManagement.Domain;
using InsuranceClaimHandler.WriteAPI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ClaimManagement.API.Tests
{
    public class ClaimManagementApiIntegrationTests
    {
        [Test]
        public async Task Test_GetAll_ReturnListOfClaimsIfAny()
        {
            var server = new TestServer(new WebHostBuilder().UseStartup<Startup>());

            var client = server.CreateClient();

            var response = await client.GetAsync($"/api/claim/all");

            response.EnsureSuccessStatusCode();

            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.OK);
        }

        [Test]
        public async Task Test_Get_ShouldReturnClaimIfExist()
        {
            var server = new TestServer(new WebHostBuilder().UseStartup<Startup>());

            var client = server.CreateClient();

            var response = await client.GetAsync($"/api/claim/2");

            response.EnsureSuccessStatusCode();

            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.OK);
        }

        [Test]
        public async Task Test_Get_ShouldReturnNotFoundClaimIfNotExist()
        {
            var server = new TestServer(new WebHostBuilder().UseStartup<Startup>());

            var client = server.CreateClient();

            var response = await client.GetAsync($"/api/claim/222");

            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.NotFound);
        }

        [Test]
        public async Task Test_Add_ShouldAddClaimAndReturnsSuccessCode()
        {
            var server = new TestServer(new WebHostBuilder().UseStartup<Startup>());

            var client = server.CreateClient();

            AddClaimCommand command = new AddClaimCommand()
            {
                Name = "Integration Test Claim",
                Year = 2014,
                DamageCost = 13M,
                Type = ClaimType.Grounding
            };

            var content = IntegrationTestHelper.GetRequestContent(command);

            var response = await client.PostAsync($"/api/claim/add", content);

            response.EnsureSuccessStatusCode();
        }

        [Test]
        public async Task Test_Add_ShouldAddClaimAndReturnsBadRequestIfNotValidData()
        {
            var server = new TestServer(new WebHostBuilder().UseStartup<Startup>());

            var client = server.CreateClient();

            AddClaimCommand command = new AddClaimCommand()
            {
                Name = "Integration Test Claim",
                Year = 2011,
                DamageCost = 130M,
                Type = ClaimType.Grounding
            };

            var content = IntegrationTestHelper.GetRequestContent(command);

            var response = await client.PostAsync($"/api/claim/add", content);

            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task Test_Remove_ShouldDeleteClaimAndReturnsSuccessCode()
        {
            var server = new TestServer(new WebHostBuilder().UseStartup<Startup>());

            var client = server.CreateClient();

            var response = await client.DeleteAsync($"/api/claim/5");

            response.EnsureSuccessStatusCode();
        }

        [Test]
        public async Task Test_Remove_ShouldDeleteClaimAndReturnsNotFound()
        {
            var server = new TestServer(new WebHostBuilder().UseStartup<Startup>());

            var client = server.CreateClient();

            var response = await client.DeleteAsync($"/api/claim/2515");

            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.NotFound);
        }


    }
}
