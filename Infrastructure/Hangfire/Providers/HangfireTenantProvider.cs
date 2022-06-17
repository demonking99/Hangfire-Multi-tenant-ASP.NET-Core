using Core.Hangfire.Interfaces;
namespace Infrastructure.Hangfire.Providers
{
    public class HangfireTenantProvider : IHangfireTenantProvider
    {
        private static System.Threading.AsyncLocal<string> HfTenantId { get; } = new System.Threading.AsyncLocal<string>();

        public void HfSetTenant(string TenantId)
        {
            HfTenantId.Value = TenantId;
        }

        public string HfGetTenantId()
        {
            return HfTenantId.Value;
        }
    }
}
