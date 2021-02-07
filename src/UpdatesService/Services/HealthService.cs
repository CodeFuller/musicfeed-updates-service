using System;
using System.Threading.Tasks;
using Grpc.Core;
using UpdatesService.Grpc;

namespace UpdatesService.Services
{
	internal class HealthService : Health.HealthBase
	{
		public override Task<HealthCheckResponse> Check(HealthCheckRequest request, ServerCallContext context)
		{
			if (String.IsNullOrEmpty(request.Service))
			{
				return GetOverallHealthStatus();
			}

			if (String.Equals(request.Service, "UpdatesService", StringComparison.OrdinalIgnoreCase))
			{
				return GetUpdatesServiceHealthStatus();
			}

			return Task.FromResult(new HealthCheckResponse
			{
				Status = HealthCheckResponse.Types.ServingStatus.ServiceUnknown,
			});
		}

		private static Task<HealthCheckResponse> GetOverallHealthStatus()
		{
			return GetUpdatesServiceHealthStatus();
		}

		private static Task<HealthCheckResponse> GetUpdatesServiceHealthStatus()
		{
			return Task.FromResult(new HealthCheckResponse
			{
				Status = HealthCheckResponse.Types.ServingStatus.Serving,
			});
		}
	}
}
