using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omilab.Terminal;
using Omilab.Extensions;
using System.IO;

namespace Omilab.Terminal.InternalCommands
{
    public class Cd : ITerminalPluggin
    {
        public string Execute()
        {
            return Directory.GetCurrentDirectory();
        }

        public string Execute(params string[] parameters)
        {
            string current = Directory.GetCurrentDirectory();
            string newCurrent = "";

            if (parameters[1] == "..")
            {
                int pos = current.LastIndexOf('\\');
                newCurrent = current.Substring(0, pos);
            }
            else
            {
                string dirName = parameters[1];
                newCurrent = current + "\\" + dirName;

            }

            if (!newCurrent.Contains("\\"))
                newCurrent += "\\";

            Directory.SetCurrentDirectory(newCurrent);

            return "";
        }

        public string HelpDescription()
        {
            return "cd - Change directory.";
        }
    }//end class
}//end namespace
