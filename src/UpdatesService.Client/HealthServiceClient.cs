﻿using UpdatesService.Client;

namespace UpdatesService.Grpc
{
#pragma warning disable CA1724 // Type names should not match namespaces
	public static partial class Health
#pragma warning restore CA1724 // Type names should not match namespaces
	{
#pragma warning disable CA1034 // Nested types should not be visible
		public partial class HealthClient : IHealthServiceClient
#pragma warning restore CA1034 // Nested types should not be visible
		{
		}
	}
}
