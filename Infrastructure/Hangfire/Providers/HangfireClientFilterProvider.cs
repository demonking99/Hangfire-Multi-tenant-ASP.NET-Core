using Core.Interfaces;
using Hangfire;
using Hangfire.Common;
using Infrastructure.Hangfire.Filters;
using System.Collections.Generic;

namespace Infrastructure.Hangfire.Providers
{
    public class HangfireClientFilterProvider : IJobFilterProvider
    {
        private readonly ITenantService _multiTenant;
        public HangfireClientFilterProvider(ITenantService multiTenant)
        {
            _multiTenant = multiTenant;
        }
        public IEnumerable<JobFilter> GetFilters(Job job)
        {
            return new JobFilter[]
            {
            new JobFilter(new CaptureCultureAttribute(), JobFilterScope.Global, null),
            new JobFilter(new HangfireClientTenantFilter(_multiTenant),JobFilterScope.Global, null)
            };
        }
    }
}
