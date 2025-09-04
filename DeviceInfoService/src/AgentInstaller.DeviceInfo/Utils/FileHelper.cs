using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AgentInstaller.DeviceInfo.Utils
{
    public class FileHelper
    {
        /**
         * Notes:
         *  Naive Idea - accept enum as AlgoName and create a switch statement
         *               switch statement is in charge of create the appropriate hash algo (i.e. SHA256, MD5, etc.)
         *               Sample api below
         *               public static string CreateFileHash(string filePath, string algorithmName = "SHA256")
         *               
         * **/
        public static string CreateFileHash(string filePath)
        {
            using var algo = SHA256.Create();
            //using var algoChatGPT = HashAlgorithm.Create(algorithmName);

            using var stream = File.OpenRead(filePath);

            var hash = algo.ComputeHash(stream);

            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }
    }
}
