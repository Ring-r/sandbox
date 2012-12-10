using System;
using System.CodeDom.Compiler;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using Microsoft.CSharp;

namespace ServerReflection
{
    class ServerReflectionService : ServiceBase
    {
        #region Private fields.

        /// <summary>
        /// Thread for listening.
        /// </summary>
        private Thread thread;

        /// <summary>
        /// Shows if need to terminate thread for listening.
        /// </summary>
        private Boolean is_thread_terminate = false;

        #endregion Private fields.

        #region Private methods.

        /// <summary>
        /// Thread for listening start function.
        /// </summary>
        private void ThreadStart()
        {
            TcpListener server = null;
            try
            {
                Int32 port = 13000;
                IPAddress address = IPAddress.Parse("127.0.0.1"); // TODO: Correct to use any address.

                server = new TcpListener(address, port);
                server.Start();

                Byte[] bytes = new Byte[2000];
                String data = null;

                while (!this.is_thread_terminate)
                {
                    using (TcpClient client = server.AcceptTcpClient())
                    {
                        data = null;
                        using (NetworkStream stream = client.GetStream())
                        {
                            int i;
                            while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                            {
                                data = Encoding.Default.GetString(bytes, 0, i);

                                string[] d = data.Split('~');

                                data = d[1];

                                String callback_message;
                                try
                                {
                                    this.CompilAndRun(data);
                                    callback_message = "OK";
                                }
                                catch (Exception exc)
                                {
                                    callback_message = exc.ToString();
                                }

                                byte[] msg = Encoding.Default.GetBytes(data);

                                stream.Write(msg, 0, msg.Length);
                            }
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                // TODO: Write exception to log.
            }
            finally
            {
                server.Stop();
            }
        }

        /// <summary>
        /// Compile code and run result.
        /// </summary>
        /// <param name="code">Code.</param>
        public void CompilAndRun(String code)
        {
            CSharpCodeProvider provider = new CSharpCodeProvider();
            CompilerParameters compiler_params = new CompilerParameters
            {
                GenerateInMemory = true,
                GenerateExecutable = true,
                CompilerOptions = "/target:winexe"
            };
            compiler_params.ReferencedAssemblies.AddRange(new string[] { "System.dll", "System.Windows.Forms.dll" });

            CompilerResults compiler_results = provider.CompileAssemblyFromSource(compiler_params, code);
            Assembly assembly = compiler_results.CompiledAssembly;

            Object instance = assembly.CreateInstance("Program");
            Type type = instance.GetType();
            MethodInfo method_info = type.GetMethod("Main");
            method_info.Invoke(instance, null);
        }

        #endregion Private methods.

        #region Protected methods.

        /// <summary>
        /// On start service method.
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            base.OnStart(args);

            this.thread = new Thread(this.ThreadStart);
            this.thread.Start();
        }

        /// <summary>
        /// On stop service method.
        /// </summary>
        protected override void OnStop()
        {
            base.OnStop();

            this.is_thread_terminate = true;
        }

        #endregion Protected methods.

        #region Public methods.

        /// <summary>
        /// Constructor.
        /// </summary>
        public ServerReflectionService()
        {
            this.ServiceName = "ServerReflectionService";
        }

        #endregion Public methods.
    }
}
