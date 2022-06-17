using Core.Interfaces;
using Hangfire.Client;
using Hangfire.Common;
using System;

namespace Infrastructure.Hangfire.Filters
{
    public class HangfireClientTenantFilter : IClientFilter
    {
        private readonly ITenantService _tenantService;
        public HangfireClientTenantFilter(ITenantService tenantService)
        {
            _tenantService = tenantService;
        }
        public void OnCreating(CreatingContext filterContext)
        {
            if (filterContext == null) throw new ArgumentNullException(nameof(filterContext));

            var tenantConfig = _tenantService.GetTenant();
            filterContext.SetJobParameter("TenantId", tenantConfig.TID);
        }

        public void OnCreated(CreatedContext filterContext)
        {
            if (filterContext == null) throw new ArgumentNullException(nameof(filterContext));
        }
    }
}
