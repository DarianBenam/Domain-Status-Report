/** File Name:     ApplicationConfig.cs
 *  By:            Darian Benam (GitHub: https://github.com/BeardedFish/)
 *  Date:          Sunday, September 17, 2023 */

namespace DomainStatusReport.Services.Configuration;

public sealed class ApplicationConfig : IApplicationConfig
{
    private const string DomainServiceArrayConfigKey = "DomainServices";

    public DomainService[] Services { get; private set; }

    public ApplicationConfig(IConfiguration configuration)
    {
        Services = configuration.GetSection(DomainServiceArrayConfigKey).Get<DomainService[]>();
    }
}
