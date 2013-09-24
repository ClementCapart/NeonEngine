using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NeonEngine
{
    public class ScriptingEngine
    {
        CodeDomProvider codeProvider;
        CompilerParameters parameters;

        public Assembly ScriptAssembly;

        public ScriptingEngine()
        {
            codeProvider = CodeDomProvider.CreateProvider("CSharp");
            parameters = new CompilerParameters();

            parameters.GenerateInMemory = true;
            
            parameters.OutputAssembly = "NeonScripts";
            parameters.ReferencedAssemblies.Add("NeonEngine.dll");
            parameters.ReferencedAssemblies.Add(@"../Data/XNA/Microsoft.Xna.Framework.dll");
            parameters.ReferencedAssemblies.Add(@"../Data/XNA/Microsoft.Xna.Framework.Game.dll");
            parameters.ReferencedAssemblies.Add(@"../Data/XNA/Microsoft.Xna.Framework.Graphics.dll");
        }

        public Type[] CompileScripts()
        {
            Console.WriteLine("All scripts loading...");
            Console.WriteLine("");

            Type[] componentTypes = null;

            if (!Directory.Exists(@"../Data/Scripts"))
                Directory.CreateDirectory("../Data/Scripts");

            CompilerResults results = codeProvider.CompileAssemblyFromFile(parameters, Directory.EnumerateFiles(@"../Data/Scripts").ToArray());

            if (results.Errors.Count > 0)
            {
                foreach (CompilerError CompErr in results.Errors)
                {
                    Console.WriteLine("File : " + CompErr.FileName.Split('\\')[CompErr.FileName.Split('\\').Length - 1]);
                    Console.WriteLine("Line number " + CompErr.Line + ", Error Number: " + CompErr.ErrorNumber + ", Error Text: " + CompErr.ErrorText + ".");
                }

                ScriptAssembly = null;
            }
            else
            {
                componentTypes = results.CompiledAssembly.GetTypes();
                foreach(Type t in componentTypes)
                    Console.WriteLine(t.Name +" (Script) loaded !");
                Console.WriteLine("");
                Console.WriteLine("All scripts loaded !");
                Console.WriteLine("");
                Console.WriteLine("");

                ScriptAssembly = results.CompiledAssembly;
            }

            if (componentTypes != null)
                Neon.Scripts = componentTypes.ToList<Type>();
            else
            {
                Console.WriteLine("! ! ! Warning : Scripts compiling error ! ! !");
                Console.WriteLine("! ! ! Latest scripts version have been kept in memory ! ! !");
                Console.WriteLine("");
                Console.WriteLine("");
            }


            return componentTypes;
        }

        public Type CompileScript(string path)
        {
            Console.WriteLine("Script loading...");

            Type componentType = null;

            CompilerResults results = codeProvider.CompileAssemblyFromFile(parameters, path);

            if (results.Errors.Count > 0)
            {
                foreach (CompilerError CompErr in results.Errors)
                {
                    Console.WriteLine("Line number " + CompErr.Line + ", Error Number: " + CompErr.ErrorNumber + ", '" + CompErr.ErrorText + ";");
                    Console.WriteLine("");
                    Console.WriteLine("Script not added !");
                }
            }
            else
            {
                
                componentType = results.CompiledAssembly.GetTypes()[0];
                
                bool AlreadyCompiled = false;
                foreach (Type t in Neon.Scripts)
                {
                    if (t.Name == componentType.Name)
                    {
                        AlreadyCompiled = true;
                        break;
                    }
                }

                if (AlreadyCompiled)
                {
                    Console.WriteLine(componentType.Name + " (Script) already compiled !");
                    Console.WriteLine("");
                    Console.WriteLine("");
                }
                else
                {

                    Console.WriteLine(componentType.Name + " (Script) loaded !");

                    Neon.Scripts.Add(componentType);
                    File.Copy(path, @"..\Data\Scripts\" + Path.GetFileName(path), true);
                    Console.WriteLine("Script copied in Scripts directory.");
                    Console.WriteLine("");
                    Console.WriteLine("");
                }
            }

            return componentType;
        }

    }
}
