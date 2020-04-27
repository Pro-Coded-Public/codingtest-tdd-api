﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using MetaWeather.Core.Entities;

using Newtonsoft.Json;

public class TestWeatherResponseBuilder
{
    WeatherResponse _weatherResponse;

    void CreateFiveForecasts()
    {
        for(var i = 0; i < 5; i++)
        {
            _weatherResponse.Forecasts
                .Add(new Forecast
                {
                    ApplicableDate = new DateTimeOffset(DateTime.UtcNow).AddDays(i),
                    TheTemp = 10 + i,
                    WeatherStateAbbr = "s",
                    WeatherStateImageURL = string.Empty,
                    WeatherStateName = "Snow"
                });
        }
    }

    public WeatherResponse Build() => _weatherResponse;

    public async Task<HttpResponseMessage> BuildHttpResponse() =>
                                           await Task.FromResult(new HttpResponseMessage(_weatherResponse.StatusCode)
                                               {
                                                   Content =
                                               new StringContent(JsonConvert.SerializeObject(_weatherResponse))
                                               });

    public TestWeatherResponseBuilder Default()
    {
        _weatherResponse = new WeatherResponse { StatusCode = HttpStatusCode.OK, Forecasts = new List<Forecast>() };

        return this;
    }

    public TestWeatherResponseBuilder WithBelfast()
    {
        // Set the city data and time
        _weatherResponse.WoeId = 44544;
        _weatherResponse.Title = "Belfast";
        _weatherResponse.Time = new DateTimeOffset(DateTime.UtcNow);
        _weatherResponse.TimeZone = "LMT";
        _weatherResponse.TimeZoneName = "Europe/London";


        /// Set the Forecasts using a data generator method
        /// TODO: Configure AutoFixcture to do this from a known dataset
        /// <see cref="AutoNSubstituteDataAttribute"/>
        CreateFiveForecasts();

        return this;
    }

    public TestWeatherResponseBuilder WithBirmingham()
    {
        CreateFiveForecasts();

        return this;
    }

    public TestWeatherResponseBuilder WithStatusCode(HttpStatusCode httpStatusCode)
    {
        _weatherResponse.StatusCode = httpStatusCode;

        return this;
    }
}

