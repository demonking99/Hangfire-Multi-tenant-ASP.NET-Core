namespace Core.Hangfire.Interfaces
{
    public interface IHangfireTenantProvider
    {
        void HfSetTenant(string TenantCode);
        string HfGetTenantId();
    }
}
