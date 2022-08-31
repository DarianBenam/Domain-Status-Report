/** File Name:     DomainStatus.cs
 *  By:            Darian Benam (GitHub: https://github.com/BeardedFish/)
 *  Date:          Tuesday, August 30, 2022 */

using System.Net;

namespace DomainStatusReport.Services;

public record DomainStatus
{
    public HttpStatusCode? StatusCode { get; private set; }
        
    public DateTime PingTimestamp { get; private set; }

    public bool IsUnreachable => StatusCode is null;

    public DomainStatus(HttpStatusCode? statusCode, DateTime pingTimestamp)
    {
        StatusCode = statusCode;
        PingTimestamp = pingTimestamp;
    }
}
