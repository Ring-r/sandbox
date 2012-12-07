using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.ServiceProcess;
using System.Threading;

namespace ServerReflection
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] commandLineArgs)
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] { new ServerReflectionService() };
            if (Environment.UserInteractive)
            {
                if (commandLineArgs.Length > 0)
                {
                    Process process = new Process();
                    string outString = "";
                    process.StartInfo.FileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Microsoft.NET", "Framework", "v4.0.30319", "InstallUtil.exe");
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.UseShellExecute = false;
                    switch (commandLineArgs[0])
                    {
                        case "-i":
                            process.StartInfo.Arguments = string.Format("\"{0}\"", Assembly.GetExecutingAssembly().Location);
                            process.Start();
                            using (StreamReader sr = process.StandardOutput)
                            {
                                outString = sr.ReadToEnd();
                            }
                            Console.WriteLine(outString);
                            Console.ReadKey();
                            break;
                        case "-u":
                            process.StartInfo.Arguments = string.Format("-u \"{0}\"", Assembly.GetExecutingAssembly().Location);
                            process.StartInfo.RedirectStandardOutput = true;
                            process.Start();
                            using (StreamReader sr = process.StandardOutput)
                            {
                                outString = sr.ReadToEnd();
                            }
                            Console.WriteLine(outString);
                            Console.ReadKey();
                            break;
                        case "-d":
                            while (!Debugger.IsAttached)
                            {
                                Thread.Sleep(100);
                            }
                            break;
                    }
                }
                else
                {
                    RunInteractive(ServicesToRun);
                }
            }
            else
            {
                ServiceBase.Run(ServicesToRun);
            }
        }

        /// <summary>
        /// Start services as a console application.
        /// </summary>
        /// <param name="servicesToRun">Services.</param>
        private static void RunInteractive(ServiceBase[] servicesToRun)
        {
            Console.WriteLine("Services running in interactive mode.");
            Console.WriteLine();

            MethodInfo onStartMethod = typeof(ServiceBase).GetMethod("OnStart", BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (ServiceBase service in servicesToRun)
            {
                try
                {
                    Console.WriteLine("Starting {0}...", service.ServiceName);
                    onStartMethod.Invoke(service, new object[] { new string[] { } });
                }
                catch (Exception exc)
                {
                    Console.WriteLine(exc);
                }
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Press any key to stop the services and end the process...");
            Console.ReadKey();
            Console.WriteLine();

            MethodInfo onStopMethod = typeof(ServiceBase).GetMethod("OnStop", BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (ServiceBase service in servicesToRun)
            {
                try
                {
                    Console.Write("Stopping {0}...", service.ServiceName);
                    onStopMethod.Invoke(service, null);
                    Console.WriteLine("Stopped");
                }
                catch (Exception exc)
                {
                    Console.WriteLine(exc);
                }
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("All services stopped. Press any key to continue...");
            // Keep the console alive to allow the user to see the message.
            Console.ReadKey();
        }
    }
}
