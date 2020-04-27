﻿using System;
using System.Threading.Tasks;

using MetaWeather.Api.Models;
using MetaWeather.Core.Entities;
using MetaWeather.Core.Interfaces;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MetaWeather.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        readonly ApiOptions _apiOptions;
        IApiProxy _apiProxy;
        readonly ILogger<LocationController> _logger;

        public LocationController(ILogger<LocationController> logger, ApiOptions apiOptions, IApiProxy apiProxy)
        {
            _logger = logger;
            _apiOptions =
                apiOptions ?? throw new ArgumentNullException(nameof(apiOptions));
            _apiProxy = apiProxy;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [HttpPost]
        public async Task<ActionResult> Post(LocationRequest locationRequest)
        {
            // TODO: Add logic to validate and return correct StatusCode

            var result = await _apiProxy.SubmitLocationRequest(locationRequest);

            return Ok(result);
        }
    }
}
