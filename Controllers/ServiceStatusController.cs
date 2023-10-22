/** File Name:     ServiceStatusController.cs
 *  By:            Darian Benam (GitHub: https://github.com/BeardedFish/)
 *  Date:          Sunday, September 17, 2023 */

using DomainStatusReport.Models.Response;
using DomainStatusReport.Services;
using DomainStatusReport.Services.Configuration;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace DomainStatusReport.Controllers;

[Route("api/service-status")]
[ApiController]
public sealed class ServiceStatusController : ControllerBase
{
    private readonly IApplicationConfig _appConfig;

    private readonly IDomainStatusCheckerService _domainStatusCheckerService;

    public ServiceStatusController(IApplicationConfig appConfig, IDomainStatusCheckerService domainStatusCheckerService)
    {
        _appConfig = appConfig;
        _domainStatusCheckerService = domainStatusCheckerService;
    }

    [HttpGet]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ServiceStatusResponse))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetServiceStatus()
    {
        if (_appConfig.Services is null)
        {
            return StatusCode(500);
        }

        (bool retrievedFromCache, DateTime? cacheExpirationTimestamp, Dictionary<string, DomainStatus> domainStatusDictionary) = await _domainStatusCheckerService.GetDomainRangeStatus(_appConfig.Services);

        return Ok(new ServiceStatusResponse(retrievedFromCache, cacheExpirationTimestamp, domainStatusDictionary));
    }
}
