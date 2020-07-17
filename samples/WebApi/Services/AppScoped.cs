using System;
using CodEaisy.TinySaas.Samples.WebApi.Options;
using Microsoft.Extensions.Options;

namespace CodEaisy.TinySaas.Samples.WebApi.Services
{
    public class AppScoped
    {
        private readonly AppOption _appOption;
        private readonly TenantOption _tenantOption;

        public AppScoped(IOptions<AppOption> appOption, IOptions<TenantOption> tenantOption)
        {
            _appOption = appOption.Value;
            _tenantOption = tenantOption.Value;
        }

        public OptionValues GetValues()
        {
            return new OptionValues {
                AppOption = _appOption.Value,
                TenantOption = _tenantOption.Value
            };
        }
    }

    public class OptionValues
    {
        public Guid AppOption { get; set; }
        public Guid TenantOption { get; set; }
    }
}
