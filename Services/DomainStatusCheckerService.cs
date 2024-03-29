/** File Name:     DomainStatusCheckerService.cs
 *  By:            Darian Benam (GitHub: https://github.com/BeardedFish/)
 *  Date:          Tuesday, August 30, 2022 */

using DomainStatusReport.Services.Configuration;
using Microsoft.Extensions.Caching.Memory;
using System.Net;

namespace DomainStatusReport.Services;

public sealed class DomainStatusCheckerService : IDomainStatusCheckerService
{
    private const int CacheLifetimeMinutes = 30;
    private const string DomainStatusCacheKey = "domainStatus";

    private static DateTime? CacheExpirationTimestamp = null;

    private readonly ILogger<DomainStatusCheckerService> _logger;
    private readonly IMemoryCache _memoryCache;

    public DomainStatusCheckerService(ILogger<DomainStatusCheckerService> logger, IMemoryCache memoryCache)
    {
        _logger = logger;
        _memoryCache = memoryCache;
    }

    public async Task<(bool RetrievedFromCache, DateTime? CacheExpirationTimestamp, Dictionary<string, DomainStatus> DomainStatusDictionary)> GetDomainRangeStatus(DomainService[] domainServiceRange)
    {
        object cache = _memoryCache.Get(DomainStatusCacheKey);

        if (cache is not null and Dictionary<string, DomainStatus>)
        {
            return (true, CacheExpirationTimestamp, (Dictionary<string, DomainStatus>)cache);
        }

        Dictionary<string, DomainStatus> domainOnlineStatusDictionary = new();

        foreach (DomainService service in domainServiceRange)
        {
            DateTime pingTimestamp = DateTime.Now;
            HttpResponseMessage? httpResponseMessage = null;

            try
            {
                using HttpClient httpClient = new();

                httpClient.DefaultRequestHeaders.UserAgent.Add(new("DomainStatusReport", "1.2.0"));
                httpClient.DefaultRequestHeaders.UserAgent.Add(new(".NET", Environment.Version.ToString()));
                httpClient.DefaultRequestHeaders.UserAgent.Add(new("(+https://www.status.darianbenam.com)"));

                using HttpRequestMessage request = new(HttpMethod.Head, service.Domain);
                httpResponseMessage = await httpClient.SendAsync(request);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError("An exception occured in the DomainStatusCheckerService: {exceptionMessage}", ex.Message);
            }
            finally
            {
                HttpStatusCode? httpStatusCode = httpResponseMessage?.StatusCode;

                domainOnlineStatusDictionary[service.Domain] = new(service.ExpectedHttpResponseCode, httpStatusCode, pingTimestamp);

                if (httpResponseMessage is not null)
                {
                    httpResponseMessage.Dispose();
                }
            }
        }

        CacheExpirationTimestamp = DateTime.Now.AddMinutes(CacheLifetimeMinutes);

        MemoryCacheEntryOptions memoryCacheEntryOptions = new();

        if (CacheExpirationTimestamp.HasValue)
        {
            memoryCacheEntryOptions.SetAbsoluteExpiration(CacheExpirationTimestamp.Value);
        }

        _memoryCache.Set(DomainStatusCacheKey, domainOnlineStatusDictionary, memoryCacheEntryOptions);

        return (false, CacheExpirationTimestamp, domainOnlineStatusDictionary);
    }
}
