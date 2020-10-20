using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omilab.Terminal
{
    interface ITerminalPluggin
    {
        string HelpDescription();
        string Execute();
        string Execute(params string[] parameters);
    }
}
