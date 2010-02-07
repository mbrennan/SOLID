using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DirectorySizeCounter
{
    class Program
    {
        static int readonlyFiles = 0;
        static void Main(string[] args)
        {
            if (args.Length == 0 || args.Length > 3)
            {
                Console.WriteLine("Usage:  DirectorySizeSummary <directory> [/filetype /showreadonly]");
                Console.WriteLine("    /filetype      -  Show by file type instead of directory");
                Console.WriteLine("    /showreadonly  -  Shows the number of files that are readonly");
                return;
            }

            int c = 10;
            string dir = args[0];
            string[] dirs = Directory.GetDirectories(dir);
            SortedDictionary<uint, List<string>> subDirSizes = new SortedDictionary<uint, List<string>>(new DescendingComparer<uint>());
            Dictionary<string, uint> categorySizes = new Dictionary<string, uint>();

            bool fileType = false;

            foreach (string arg in args)
            {
                if (arg == "/filetype")
                    fileType = true;
            }

            bool showRO = false;
            foreach (string arg in args)
            {
                if (arg == "/showreadonly")
                    showRO = true;
            }

            if (!fileType)
            {
                foreach (string subDir in dirs)
                {
                    uint dirSize = GetDirSize(subDir);
                    if (!subDirSizes.ContainsKey(dirSize))
                    {
                        subDirSizes[dirSize] = new List<string>();
                    }

                    subDirSizes[dirSize].Add(subDir);
                }

                int i = 0;
                foreach (uint size in subDirSizes.Keys)
                {
                    foreach (string subDir in subDirSizes[size])
                    {
                        ++i;
                        string output = string.Format("{0} - {1}", subDir, size);
                        Console.WriteLine(output);

                        if (i == c)
                            break;
                    }

                    if (i == c)
                        break;
                }

                if (showRO)
                {
                    Console.WriteLine(string.Format("Total readonly files {0}", readonlyFiles));
                }
            }
            else
            {
                foreach (string subDir in dirs)
                {
                    GetDirSizeByCategory(subDir, categorySizes);
                }

                SortedDictionary<uint, List<string>> topCats = new SortedDictionary<uint, List<string>>(new DescendingComparer<uint>());
                foreach (KeyValuePair<string, uint> kvp in categorySizes)
                {
                    if (!topCats.ContainsKey(kvp.Value))
                    {
                        topCats[kvp.Value] = new List<string>();
                    }

                    topCats[kvp.Value].Add(kvp.Key);
                }

                int i = 0;
                foreach (uint size in topCats.Keys)
                {
                    foreach (string category in topCats[size])
                    {
                        ++i;
                        string output = string.Format("{0} - {1}", category, size);
                        Console.WriteLine(output);

                        if (i == c)
                            break;
                    }

                    if (i == c)
                        break;
                }

                if (showRO)
                {
                    Console.WriteLine(string.Format("Total readonly files {0}", readonlyFiles));
                }
            }
        }

        static private void GetDirSizeByCategory(string dir, Dictionary<string, uint> sizes)
        {
            foreach (string subDir in Directory.GetDirectories(dir))
            {
                GetDirSizeByCategory(subDir, sizes);
            }

            foreach (string file in Directory.GetFiles(dir))
            {
                FileInfo fi = new FileInfo(file);
                if (fi.IsReadOnly)
                {
                    readonlyFiles++;
                }

                if (!sizes.ContainsKey(fi.Extension))
                {
                    sizes[fi.Extension] = 0;
                }

                sizes[fi.Extension] += (uint) fi.Length;
            }
        }

        static private uint GetDirSize(string dir)
        {
            uint size = 0;

            foreach (string subDir in Directory.GetDirectories(dir))
            {
                size += GetDirSize(subDir);
            }

            foreach (string file in Directory.GetFiles(dir))
            {
                FileInfo fi = new FileInfo(file);
                if (fi.IsReadOnly)
                {
                    readonlyFiles++;
                }

                size += (uint) fi.Length;
            }

            return size;
        }
    }

    class DescendingComparer<T> : IComparer<T> where T : IComparable<T>
    {
        public int Compare(T x, T y)
        {
            return y.CompareTo(x);
        }
    }
}