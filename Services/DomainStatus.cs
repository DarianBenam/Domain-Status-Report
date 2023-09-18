/** File Name:     DomainStatus.cs
 *  By:            Darian Benam (GitHub: https://github.com/BeardedFish/)
 *  Date:          Tuesday, August 30, 2022 */

using System.Net;

namespace DomainStatusReport.Services;

public sealed record DomainStatus
{
    private HttpStatusCode? ExpectedStatusCode { get; }

    public HttpStatusCode? StatusCode { get; private set; }
        
    public DateTime PingTimestamp { get; private set; }

    public bool IsUnreachable => StatusCode is null || ExpectedStatusCode != StatusCode;

    public DomainStatus(HttpStatusCode? expectedStatusCode, HttpStatusCode? statusCode, DateTime pingTimestamp)
    {
        ExpectedStatusCode = expectedStatusCode;
        StatusCode = statusCode;
        PingTimestamp = pingTimestamp;
    }
}
