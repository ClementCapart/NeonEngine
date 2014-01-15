using System;
using System.Threading;
using NeonEngine;
using System.Globalization;

namespace NeonStar
{
#if WINDOWS || XBOX
    class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 
        [STAThread]
        static void Main(string[] args)
        {
            new Program().Run();
        }

        void Run()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            Console.SetOut(new LogWriter("log.txt", false, Console.Out.Encoding, Console.BufferWidth * Console.BufferHeight, Console.Out));

            using (Game1 game = new Game1())
            {
                game.Run();
            }
        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Console.WriteLine(e.ExceptionObject.ToString());
        }
    }
#endif
}

