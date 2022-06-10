using Core.Hangfire.Interfaces;
namespace Infrastructure.Hangfire.Providers
{
    public class HangfireTenantProvider : IHangfireTenantProvider
    {
        private static System.Threading.AsyncLocal<string> HfTenantCode { get; } = new System.Threading.AsyncLocal<string>();

        public void HfSetTenant(string TenantCode)
        {
            HfTenantCode.Value = TenantCode;
        }

        public string HfGetTenantId()
        {
            return HfTenantCode.Value;
        }
    }
}
