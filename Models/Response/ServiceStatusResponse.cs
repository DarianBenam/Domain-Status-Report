/** File Name:     ServiceStatusResponse.cs
 *  By:            Darian Benam (GitHub: https://github.com/BeardedFish/)
 *  Date:          Sunday, September 17, 2023 */

using DomainStatusReport.Services;

namespace DomainStatusReport.Models.Response;

public sealed class ServiceStatusResponse
{
    private readonly int _reachableServiceCount;

    private readonly int _unreachableServiceCount;

    public int TotalServiceCount => ServiceRangeResult.Count;

    public int ReachableServiceCount => _reachableServiceCount;

    public int UnreachableServiceCount => _unreachableServiceCount;

    public bool RetrievedFromCache { get; private set; }

    public DateTime? CacheExpirationTimestamp { get; private set; }

    public Dictionary<string, DomainStatus> ServiceRangeResult { get; private set; }

    public ServiceStatusResponse(bool retrievedFromCache, DateTime? cacheExpirationTimestamp, Dictionary<string, DomainStatus> serviceRangeResult)
    {
        RetrievedFromCache = retrievedFromCache;
        CacheExpirationTimestamp = cacheExpirationTimestamp;
        ServiceRangeResult = serviceRangeResult;

        _reachableServiceCount = serviceRangeResult.Count(keyValuePair => !keyValuePair.Value.IsUnreachable);
        _unreachableServiceCount = TotalServiceCount - ReachableServiceCount;
    }
}
