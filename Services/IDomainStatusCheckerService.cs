/** File Name:     IDomainStatusCheckerService.cs
 *  By:            Darian Benam (GitHub: https://github.com/BeardedFish/)
 *  Date:          Tuesday, August 30, 2022 */

using DomainStatusReport.Services.Configuration;

namespace DomainStatusReport.Services;

public interface IDomainStatusCheckerService
{
    Task<(bool RetrievedFromCache, DateTime? CacheExpirationTimestamp, Dictionary<string, DomainStatus> DomainStatusDictionary)> GetDomainRangeStatus(DomainService[] domainServiceRange);
}
