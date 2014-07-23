using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
namespace FGWEB.BLL
{
    public class FileStorageService
    {
        public readonly static string C_Root = @"E:\craftfiles\";

        public string  SaveFile(byte[] bytes)
        {
            var path = GetPath();
            EnsureDirExists(C_Root + path);
            File.WriteAllBytes(C_Root +path, bytes);
            return path;
        }
        public byte[] ReadFile(string path)
        {
            var filename = C_Root + path;
            return File.ReadAllBytes(filename);
        }
        public static string GetPath()
        {
            string guid = Guid.NewGuid().ToString("N");
            var hash = guid.GetHashCode();
            int dir1 = hash & 0xf;
            int dir2 = (hash >> 4) & 0xf;
            int dir3 = (hash >> 8) & 0xf;
            return Path.Combine(dir1.ToString(), dir2.ToString(), dir3.ToString(), guid + ".d");
        }
        public static void EnsureDirExists(string filename)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(Path.GetDirectoryName(filename));
            var dir = dirInfo;
            Stack<DirectoryInfo> needCreatedDirs = new Stack<DirectoryInfo>();
            while (!dir.Exists)
            {
                needCreatedDirs.Push(dir);
                dir = dir.Parent;
            }
            while (needCreatedDirs.Count > 0)
            {
                needCreatedDirs.Pop().Create();
            }

        }
    }
}