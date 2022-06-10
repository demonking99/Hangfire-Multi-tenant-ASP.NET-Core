using Core.Hangfire.Interfaces;
using Hangfire.Server;
using System;

namespace Infrastructure.Hangfire.Filters
{
    public class HangfireServerTenantFilter : IServerFilter
    {
        private readonly IHangfireTenantProvider _hfTenantProvider;
        public HangfireServerTenantFilter(IHangfireTenantProvider hfTenantProvider)
        {
            _hfTenantProvider = hfTenantProvider;
        }

        public void OnPerforming(PerformingContext filterContext)
        {
            if (filterContext == null) throw new ArgumentNullException(nameof(filterContext));

            var tenantId = filterContext.GetJobParameter<string>("TenantId");
            // need to get the tenantId passed to the method that calls the creation of the DbContext
            _hfTenantProvider.HfSetTenant(tenantId);
        }

        public void OnPerformed(PerformedContext filterContext)
        {
            if (filterContext == null) throw new ArgumentNullException(nameof(filterContext));
        }
    }
}
