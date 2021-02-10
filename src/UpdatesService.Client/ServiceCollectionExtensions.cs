using System;
using Grpc.Net.ClientFactory;
using Microsoft.Extensions.DependencyInjection;

namespace UpdatesService.Client
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
			services.AddGrpcClient<TClientImplementation>(configureClient);
			services.AddSingleton<TClientInterface, TClientImplementation>(sp => sp.GetRequiredService<TClientImplementation>());
		}
	}
}
