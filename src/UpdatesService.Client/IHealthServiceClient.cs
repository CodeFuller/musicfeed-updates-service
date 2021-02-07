using System;
using UpdatesService.Grpc;

using grpc = Grpc.Core;

namespace UpdatesService.Client
{
	/// <summary>
	/// Client for HealthService.
	/// </summary>
	public interface IHealthServiceClient
	{
		/// <summary>
		/// Returns health check for the service.
		/// </summary>
		/// <param name="request">The request to send to the server.</param>
		/// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
		/// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
		/// <param name="cancellationToken">An optional token for canceling the call.</param>
		/// <returns>The call object.</returns>
		grpc::AsyncUnaryCall<HealthCheckResponse> CheckAsync(HealthCheckRequest request, grpc::Metadata headers = null, DateTime? deadline = null, System.Threading.CancellationToken cancellationToken = default);
	}
}
