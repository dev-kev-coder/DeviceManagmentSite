using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentInstaller.Service.utils.ProgramFilesUpdater
{
    public static class ProgramFilesUpdater
    {
        public static void Main(IConfiguration config)
        {
            var dummyTargetTextFile = config.GetValue<string>("ProgramFilesPath");

            if (dummyTargetTextFile == null) 
            {
                throw new Exception("File path came out as null. Tried to get ProgramFilesPath from settings.json file");
            }

            // Basic way to Read files
            using (var streamReader = new StreamReader(dummyTargetTextFile))
            {
                var fileConents = streamReader.ReadToEnd();
            }
        }
    }
}
