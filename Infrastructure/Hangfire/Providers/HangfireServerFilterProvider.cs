using Core.Hangfire.Interfaces;
using Hangfire;
using Hangfire.Common;
using Infrastructure.Hangfire.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Hangfire.Providers
{
    public class HangfireServerFilterProvider : IJobFilterProvider
    {
        private readonly IHangfireTenantProvider _hfTenantProvider;
        public HangfireServerFilterProvider(IHangfireTenantProvider hfTenantProvider)
        {
            _hfTenantProvider = hfTenantProvider;
        }
        public IEnumerable<JobFilter> GetFilters(Job job)
        {
            return new JobFilter[]
            {
            new JobFilter(new CaptureCultureAttribute(), JobFilterScope.Global, null),
            new JobFilter(new HangfireServerTenantFilter(_hfTenantProvider), JobFilterScope.Global,  null),
            };
        }
    }
}
