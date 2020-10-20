using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Reflection.Emit;
using Omilab.Extensions;

namespace Omilab.Terminal
{
    public static class Terminal
    {
        /// <summary>
        /// el namespace externo el de la aplicacion que quieres crear
        /// </summary>
        public static string AppNameSpace { get; set; }
        public static string InternalNamespace = "Omilab.Terminal.InternalCommands";
        public static string Title { get; set; }

        public static void Run()
        {
            bool running = true;
            Console.Title = Terminal.Title;

            while (running)
            {
                Console.Write("Terminal:/> ");
                string cmd = Console.ReadLine();

                if (cmd == "exit" || cmd == "quit")
                    running = false;
                else
                {
                    string data = ReadCommand(cmd);

                    Console.WriteLine(data);
                }

            }
        }

        private static string ReadCommand(string cmd)
        {
            string output = "";

            if (cmd == "")
                return "";

            char[] sep1 = { '"' };
            char[] sep2 = { ' ' };
            string[] parameters;

            if (cmd.Contains('"'))
                parameters = cmd.Split(sep1, StringSplitOptions.RemoveEmptyEntries);
            else
                parameters = cmd.Split(sep2, StringSplitOptions.RemoveEmptyEntries);


            if (parameters.Length == 0)
            {
                return "";
            }
            else if (parameters.Length == 1)
            {
                output = RunCommand(cmd);
            }
            else
            {
                output = RunCommand(parameters);
            }

            return output;
        }

        private static string RunCommand(string className)
        {

            string cmdClass = className.Capitalize();

            if (cmdClass == "?")
                cmdClass = "Help";


            //primero los de la applicacion por ejemplo el help de esta aplicacion
            var objectType = Type.GetType(AppNameSpace + "." + cmdClass);

            //si no existe, tratar con los que viene por defectos, 
            //por si hay override de comandos como el help :)
            if (objectType == null)
            {
                objectType = Type.GetType(InternalNamespace + "." + cmdClass);
                if (objectType == null)
                {
                   // Console.WriteLine("Error: Command not found.");
                    return "Error: Command not found.";
                }
            }

            var instantiatedObject = Activator.CreateInstance(objectType) as ITerminalPluggin;
            return instantiatedObject.Execute();
        }

        private static string RunCommand(params string[] parameters)
        {

            if (parameters[0] == "?")
                parameters[0] = "Help";

            string cmdClass = parameters[0].Capitalize();
                       

            //primero los de la applicacion por ejemplo el help de esta aplicacion
            var objectType = Type.GetType(AppNameSpace + "." + cmdClass);

            //si no existe, tratar con los que viene por defectos, 
            //por si hay override de comandos como el help :)
            if (objectType == null)
            {
                objectType = Type.GetType(InternalNamespace + "." + cmdClass);
                if (objectType == null)
                {
                    //Console.WriteLine("Error: Command not found.");
                    return "Error: Command not found.";
                }
            }

            var instantiatedObject = Activator.CreateInstance(objectType) as ITerminalPluggin;
            return instantiatedObject.Execute(parameters);
        }

    } //end class
} //end namespace
