using System;
using System.Net.Http;
using Grpc.Net.ClientFactory;
using Microsoft.Extensions.DependencyInjection;

namespace MusicFeed.UpdatesService.Client
{
	/// <summary>
	/// Extension methods for adding client to UpdatesService.
	/// </summary>
	public static class ServiceCollectionExtensions
	{
		/// <summary>
		/// Adds services required for client to UpdatesService.
		/// </summary>
		/// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
		/// <param name="configureClient">A delegate that is used to configure the gRPC client.</param>
		/// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
		public static IServiceCollection AddUpdatesServiceClient(this IServiceCollection services, Action<GrpcClientFactoryOptions> configureClient)
		{
			services.RegisterGrpcClient<IUpdatesServiceClient, Grpc.UpdatesService.UpdatesServiceClient>(configureClient);

			return services;
		}

		private static void RegisterGrpcClient<TClientInterface, TClientImplementation>(this IServiceCollection services, Action<GrpcClientFactoryOptions> configureClient)
			where TClientInterface : class
			where TClientImplementation : class, TClientInterface
		{
			services.AddGrpcClient<TClientImplementation>(configureClient)

				// By default AddGrpcClient will use SocketsHttpHandler - https://github.com/grpc/grpc-dotnet/blob/08024e350d39394db6982f65528fb2e3653c7666/src/Shared/HttpHandlerFactory.cs#L27
				// This will break dependency detection by Application Insights.
				.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler());

			services.AddSingleton<TClientInterface, TClientImplementation>(sp => sp.GetRequiredService<TClientImplementation>());
		}
	}
}
