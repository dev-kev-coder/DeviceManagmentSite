using AgentInstaller.DeviceInfo.Utils;
using System.Diagnostics;
using System.Security.Cryptography;

namespace AgentInstaller.DeviceInfo
{
    internal class Program
    {
        private static string _DevelopmentTestRootFolder = "C:\\Users\\dev\\OnlineRepos\\Github\\WebDev\\DeviceManagmentSite\\DeviceInfoService\\src\\BuildTestingGrounds";
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");


            // TODO: need to test against failures
            try
            {
                var fileCount = 0;
                var programFilesDirectory = Path.Join(_DevelopmentTestRootFolder, "ProgramFiles");
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var filesToHash = Directory
                    //.EnumerateFileSystemEntries(programFilesDirectory) // use to get files and directories
                    .EnumerateFiles(programFilesDirectory)
                    .Aggregate(new Dictionary<string, string>(), (map, filePath) =>
                    {
                        fileCount++;
                        map.Add(Path.GetFileName(filePath), FileHelper.CreateFileHash(filePath));
                        return map;
                    });
                stopwatch.Stop();
                Console.WriteLine($"# of Files Hashed: {fileCount}");
                Console.WriteLine($"Time elapsed in ms: {stopwatch.ElapsedMilliseconds}");
                //var fileCount = 0;
                //var programFilesDirectory = Path.Join(_DevelopmentTestRootFolder, "ProgramFiles");
                //var stopwatch = new Stopwatch();
                //var files = Directory.GetFiles(programFilesDirectory);
                //stopwatch.Start();
                //var fileNameToHash = files.Aggregate(new Dictionary<string, string>(),
                //(filePathHashMap, filePath) =>
                //{
                //    var fileHash = FileHelper.CreateFileHash(filePath);

                //    var fileName = Path.GetFileName(filePath);
                //    filePathHashMap.Add(fileName, fileHash);

                //    return filePathHashMap;
                //});
                //stopwatch.Stop();

            }
            catch (Exception ex) 
            {

            }
        }
    }
}
