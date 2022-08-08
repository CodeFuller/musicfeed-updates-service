using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using MusicFeed.UpdatesService.Client;

namespace MusicFeed.UpdatesService.IntegrationTests
{
	public class CustomWebApplicationFactory : WebApplicationFactory<Startup>
	{
		private ServiceProvider ServiceProvider { get; set; }

		public TService CreateServiceClient<TService>()
		{
			InitializeServiceProvider();

			return ServiceProvider.GetRequiredService<TService>();
		}

		private void InitializeServiceProvider()
		{
			if (ServiceProvider != null)
			{
				return;
			}

			var services = new ServiceCollection();

			var httpClient = CreateClient();

			services.AddUpdatesServiceClient(factoryOptions =>
			{
				factoryOptions.Address = httpClient.BaseAddress;
				factoryOptions.ChannelOptionsActions.Add(channelOptions => channelOptions.HttpClient = httpClient);
			});

			ServiceProvider = services.BuildServiceProvider();
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

			if (disposing)
			{
				ServiceProvider?.Dispose();
			}
		}
	}
}
