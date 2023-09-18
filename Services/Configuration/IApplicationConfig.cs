/** File Name:     IApplicationConfig.cs
 *  By:            Darian Benam (GitHub: https://github.com/BeardedFish/)
 *  Date:          Sunday, September 17, 2023 */

namespace DomainStatusReport.Services.Configuration;

public interface IApplicationConfig
{
    DomainService[] Services { get; }
}
