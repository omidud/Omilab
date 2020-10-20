using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Omilab.Terminal;

namespace Omilab.Terminal.InternalCommands
{
    public struct DirData
    {
        public string Name;
        public string IsDir;
        public string Modified;
        public string Size;
    }

    public class Dir : ITerminalPluggin
    {
        public string HelpDescription()
        {
            return "dir - Print the current directory.";
        }

        public string Execute()
        {
            string[] files = Directory.GetFiles(Directory.GetCurrentDirectory());
            string[] directories = Directory.GetDirectories(Directory.GetCurrentDirectory());

            return PrintInfo(directories, files);

        }

        public string Execute(params string[] parameters)
        {
            string searchPattern = parameters[1];
            string[] files = Directory.GetFiles(Directory.GetCurrentDirectory(), searchPattern);
            string[] directories = Directory.GetDirectories(Directory.GetCurrentDirectory(), searchPattern);

            return PrintInfo(directories, files);
        }

        private string PrintInfo(string[] directories, string[] files)
        {
            Dictionary<string, DirData> DirList = new Dictionary<string, DirData>();

            foreach (string folder in directories)
            {
                DirectoryInfo info = new DirectoryInfo(folder);

                DirData data;
                data.Name = info.Name;
                data.Modified = FormatDateTime(info.LastWriteTime).PadRight(23, ' ');
                data.IsDir = "<DIR> ";
                data.Size = "".PadLeft(9, ' ');
                DirList.Add(info.Name, data);
            }

            foreach (string filename in files)
            {
                FileInfo info = new FileInfo(filename);
                DirData data;
                data.Name = info.Name;
                data.Modified = FormatDateTime(info.LastWriteTime).PadRight(23, ' ');
                data.IsDir = "      ";
                data.Size = String.Format("{0:n0}", info.Length).PadLeft(9, ' ');
                DirList.Add(info.Name, data);
            }

            // Acquire keys and sort them.
            var list = DirList.Keys.ToList();
            list.Sort();


            string output = "";
            // Loop through keys.
            foreach (var key in list)
            {
                //Console.WriteLine(DirList[key].Modified + DirList[key].IsDir + DirList[key].Size + " " + DirList[key].Name);
                output += DirList[key].Modified + DirList[key].IsDir + DirList[key].Size + " " + DirList[key].Name + Environment.NewLine;
            }

            return output;
        }

        private string FormatDateTime(DateTime dateTime)
        {
            string output = "";

            string YYYY = dateTime.Year.ToString();
            string MM = dateTime.Month.ToString().PadLeft(2, '0');
            string DD = dateTime.Day.ToString().PadLeft(2, '0');
            int hInt = dateTime.Hour;
            string am_pm = "";
            string mm = dateTime.Minute.ToString().PadLeft(2, '0');

            if (hInt > 12)
            {
                am_pm = "PM";
                hInt = hInt - 12;
            }
            else
            {
                am_pm = "AM";
            }


            string hh = hInt.ToString().PadLeft(2, '0');

            output = MM + "/" + DD + "/" + YYYY + " " + hh + ":" + mm + " " + am_pm;

            return output;
        }
    }
}
