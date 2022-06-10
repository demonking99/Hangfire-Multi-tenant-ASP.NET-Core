using Core.Interfaces;
using Hangfire.Client;
using Hangfire.Common;
using System;

namespace Infrastructure.Hangfire.Filters
{
    public class HangfireClientTenantFilter : JobFilterAttribute,IClientFilter
    {
        private readonly ITenantService _multiTenant;
        public HangfireClientTenantFilter(ITenantService multiTenant)
        {
            _multiTenant = multiTenant;
        }
        public void OnCreating(CreatingContext filterContext)
        {
            if (filterContext == null) throw new ArgumentNullException(nameof(filterContext));

            var tenantConfig = _multiTenant.GetTenant();
            filterContext.SetJobParameter("TenantId", tenantConfig.TID);
        }

        public void OnCreated(CreatedContext filterContext)
        {
            if (filterContext == null) throw new ArgumentNullException(nameof(filterContext));
        }
    }
}
