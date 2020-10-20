using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omilab.Terminal.InternalCommands
{
    public class Cls : ITerminalPluggin
    {
        public string Execute()
        {
            Console.Clear();
            return "";
        }

        public string Execute(params string[] parameters)
        {
            Console.Clear();
            return "";
        }

        public string HelpDescription()
        {
            return "cls - Clear the console screen.";
        }
    }
}
