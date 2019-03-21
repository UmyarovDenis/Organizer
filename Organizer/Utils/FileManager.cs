using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organizer.Utils
{
    internal static class FileManager
    {
        public static List<FileInfo> GetFileInfos(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            string[] files = Directory.GetFiles(directoryPath);
            List<FileInfo> fileInfos = new List<FileInfo>();

            if (files?.Length > 0)
            {
                foreach(string file in files)
                {
                    fileInfos.Add(new FileInfo(file));
                }
            }

            return fileInfos;
        }
    }
}
