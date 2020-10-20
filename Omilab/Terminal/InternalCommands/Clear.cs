using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omilab.Terminal.InternalCommands
{
    public class Clear : ITerminalPluggin
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
            return "clear - Clear the console screen.";
        }
    }
}
