using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Omilab.Terminal;

namespace Omilab.Terminal.InternalCommands
{
    public class Pwd : ITerminalPluggin
    {
        public string HelpDescription()
        {
            return "pwd - Print working directory.";
        }

        public string Execute()
        {
            //Console.WriteLine("Current Directory is: " + Directory.GetCurrentDirectory());
            return "Current Directory is: " + Directory.GetCurrentDirectory();
        }

        public string Execute(params string[] parameters)
        {
            return "Current Directory is: " + Directory.GetCurrentDirectory();
        }
    }
}
