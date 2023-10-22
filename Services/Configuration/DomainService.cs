/** File Name:     DomainService.cs
 *  By:            Darian Benam (GitHub: https://github.com/BeardedFish/)
 *  Date:          Sunday, September 17, 2023 */

using System.Net;

namespace DomainStatusReport.Services.Configuration;

public sealed record DomainService
{
    public string Domain { get; set; } = string.Empty;

    public HttpStatusCode ExpectedHttpResponseCode { get; set; } = HttpStatusCode.OK;
}
