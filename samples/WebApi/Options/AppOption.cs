using System;
namespace CodEaisy.TinySaas.Samples.WebApi.Options
{
    public class AppOption
    {
        public const string Key = nameof(AppOption);

        public Guid Value { get; set; }
    }
}
