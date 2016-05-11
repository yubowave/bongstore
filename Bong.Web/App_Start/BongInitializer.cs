using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Reflection;
using System.Web.Compilation;

// [assembly: PreApplicationStartMethod(typeof(Bong.Web.BongInitializer), "Start")]

namespace Bong.Web
{
    public static class BongInitializer
    {
        private const string _pluginPath = "~/plugins";
        public static void Start()
        {
            // loading all dll in plugin folder
            var pluginFolder = new DirectoryInfo(_pluginPath);
            var pluginFiles = pluginFolder.GetFiles("*.dll", SearchOption.AllDirectories);


            foreach (var pluginFile in pluginFiles)
            {
                var assembly = Assembly.LoadFrom(pluginFile.FullName);
                BuildManager.AddReferencedAssembly(assembly);
            }
        }
        public static void Stop()
        {

        }
    }
}