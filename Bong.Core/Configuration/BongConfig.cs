using System;
using System.Configuration;
using System.Xml;

namespace Bong.Core.Configuration
{
    /// <summary>
    /// Represent the Bong section of config file
    /// </summary>
    public class BongConfig : IConfigurationSectionHandler
    {
        /// <summary>
        /// Create a configuration section handler
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="configContext"></param>
        /// <param name="section"></param>
        /// <returns></returns>
        public object Create(object parent, object configContext, XmlNode section)
        {
            var config = new BongConfig();
            var testAttrNode = section.SelectSingleNode("SampleData");
            if (testAttrNode != null && testAttrNode.Attributes != null)
            {
                var attribute = testAttrNode.Attributes["Install"];
                if (attribute != null)
                    config.IsInstallData = Convert.ToBoolean(attribute.Value);
            }
            var themeAttrNode = section.SelectSingleNode("Themes");
            if (themeAttrNode != null && themeAttrNode.Attributes != null)
            {
                var attribute = themeAttrNode.Attributes["basePath"];
                if (attribute != null)
                    config.ThemeBasePath = attribute.Value;
            }

            return config;
        }

        public bool IsInstallData { get; private set; }
        public string ThemeBasePath { get; private set; }
    }
}
