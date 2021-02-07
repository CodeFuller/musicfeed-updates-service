using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UpdatesService.Client;
using UpdatesService.Grpc;

namespace UpdatesService.IntegrationTests
{
	[TestClass]
	public class HealthServiceTests
	{
		[TestMethod]
		public async Task Check_ForEmptyServiceName_ReturnsServingStatus()
		{
			// Arrange

			var request = new HealthCheckRequest
			{
				Service = String.Empty,
			};

			using var factory = new CustomWebApplicationFactory();
			var client = factory.CreateServiceClient<IHealthServiceClient>();

			// Act

			var response = await client.CheckAsync(request);

			// Assert

			Assert.AreEqual(HealthCheckResponse.Types.ServingStatus.Serving, response.Status);
		}

		[TestMethod]
		public async Task Check_ForUpdatesService_ReturnsServingStatus()
		{
			// Arrange

			var request = new HealthCheckRequest
			{
				Service = "UpdatesService",
			};

			using var factory = new CustomWebApplicationFactory();
			var client = factory.CreateServiceClient<IHealthServiceClient>();

			// Act

			var response = await client.CheckAsync(request);

			// Assert

			Assert.AreEqual(HealthCheckResponse.Types.ServingStatus.Serving, response.Status);
		}

		[TestMethod]
		public async Task Check_ForUnknownService_ReturnsServiceUnknownStatus()
		{
			// Arrange

			var request = new HealthCheckRequest
			{
				Service = "SomeService",
			};

			using var factory = new CustomWebApplicationFactory();
			var client = factory.CreateServiceClient<IHealthServiceClient>();

			// Act

			var response = await client.CheckAsync(request);

			// Assert

			Assert.AreEqual(HealthCheckResponse.Types.ServingStatus.ServiceUnknown, response.Status);
		}
	}
}
