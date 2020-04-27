﻿using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;

namespace MetaWeather.Tests.Common
{
    public class AutoNSubstituteDataAttribute : AutoDataAttribute
    {
        public AutoNSubstituteDataAttribute() : base(() =>
        {
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization {ConfigureMembers = true});

            return fixture;
        })
        {
        }
    }
}