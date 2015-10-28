﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using EcConfig.Core;
using EcConfig.Core.Exceptions;
using EcConfig.Core.Facades;
using EcConfig.Core.Resources;
using NUnit.Framework;

namespace EcConfig.Tests
{
    public class PropertiesExtractorTests : BaseTest
    {
        private readonly EcGlobalConfigurations _ecConfigs = new EcGlobalConfigurations { ConfigFilesPath = "", CurrentConfigFileName = "dev", IsCaseSensitive = true};

        [Test]
        public void GetProperties_FromCache()
        {
            MemoryCache.Default.Add(EcConfigResources.CacheKey_Properties, new Dictionary<string, string> { { "key", "value" }, { "key1", "value1" } },
                DateTime.Now.AddDays(1));
            var properties = PropertiesExtractor.GetProperties(_ecConfigs);
            Assert.AreEqual("value", properties.First(x => x.Key == "key").Value);
        }

        [Test]
        public void GetProperties_FromFile()
        {
            var properties = PropertiesExtractor.GetProperties(_ecConfigs);
            Assert.AreEqual("dev", properties.First(x => x.Key == "key").Value);
        }

        [Test]
        [ExpectedException(typeof(EcConfigException), ExpectedMessage = "EcConfig is not able to find config file .\\test\\notexist.config")]
        public void GetProperties_ErrorConfigFileNotFound()
        {
            EcGlobalConfigurations temp = new EcGlobalConfigurations { CurrentConfigFileName = "notexist", ConfigFilesPath = ".\\test" };
            PropertiesExtractor.GetProperties(temp);
        }
    }
}