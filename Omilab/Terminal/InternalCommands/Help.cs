using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omilab.Terminal;
using System.Reflection;
using System.Reflection.Emit;
using Omilab.Extensions;

namespace Omilab.Terminal.InternalCommands
{
    public class Help : ITerminalPluggin
    {
        public virtual string HelpDescription()
        {
            return "help - Print this help.";
        }

        public virtual string Execute()
        {
            //Console.WriteLine("el help por defecto");

            string nspace = Terminal.InternalNamespace; // "Omilab.Terminal.InternalCommands";

            var q = from t in Assembly.GetExecutingAssembly().GetTypes()
                    where t.IsClass && t.Namespace == nspace
                    select t;

            //q.ToList().ForEach(
            //    t => Console.WriteLine(t.Name)
            // );         

            string output = "";

            q.ToList().ForEach(
                t =>
                {
                    var objectType = Type.GetType(nspace + "." + t.Name.Capitalize());

                    if (objectType != null)
                    {
                        var instantiatedObject = Activator.CreateInstance(objectType) as ITerminalPluggin;
                        // Console.WriteLine(instantiatedObject.HelpDescription());

                        output += instantiatedObject.HelpDescription() + Environment.NewLine;
                    }
                }
            );

            return output;
        }

        public virtual string Execute(params string[] parameters)
        {
            string nspace = Terminal.InternalNamespace; //"Omilab.Terminal.InternalCommands";

            //Console.WriteLine("el help de {0} por defecto", parameters[1]);

            var objectType = Type.GetType(nspace + "." + parameters[1].Capitalize());

            string output = "";

            if (objectType != null)
            {
                var instantiatedObject = Activator.CreateInstance(objectType) as ITerminalPluggin;
                //Console.WriteLine(instantiatedObject.HelpDescription());
                output = instantiatedObject.HelpDescription();
            }
            else
            {
                //Console.WriteLine("Help for that command not found");
                output = "Help for that command not found";
            }

            return output;
        }
    } //end class
}//end namespace
