using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ClientServerTest
{
	class MainClass
	{
		public static bool IsExit = false;

		public static void Main (string[] args)
		{
			Thread threadListener = new Thread (new ThreadStart (() => {
				TcpListener tcpListener = new TcpListener (10000);

				try {
					tcpListener.Start ();

					Byte[] bytes = new Byte[256];
					String data = null;
					int timeout = 100;

					while (!MainClass.IsExit) {
						if (tcpListener.Pending ()) {
							using (TcpClient client = tcpListener.AcceptTcpClient ()) {
								data = null;
								using (NetworkStream stream = client.GetStream ()) {
									int i;
									while ((i = stream.Read(bytes, 0, bytes.Length))!=0) {   
										data = System.Text.Encoding.ASCII.GetString (bytes, 0, i);
										Console.WriteLine ("<Server>: {0}", data);
//										byte[] msg = System.Text.Encoding.ASCII.GetBytes (data);
//										stream.Write (msg, 0, msg.Length);
//										Console.WriteLine ("Sent: {0}", data);            
									}
								}
							}
						} else {
							Thread.Sleep (timeout);
						}
					}
				} catch (SocketException e) {
					Console.WriteLine ("SocketException: {0}", e);
				} finally {
					tcpListener.Stop ();
				}
			}
			)
			);
			threadListener.Start ();

			Console.WriteLine ("Hello World!");
			string cmd = null;
			do {
				Console.Write ("<Client>:");
				cmd = Console.ReadLine ();
				if (cmd.Contains ("send ")) {
					string message = cmd.Replace ("send ", "");
					MainClass.SendMessage (message);
					Console.WriteLine ("<Client>:Done.");
				}
			} while(cmd != "exit");
			MainClass.IsExit = true;
			threadListener.Join ();
		}

		private static void SendMessage (string message)
		{
			try {
				Int32 port = 10000;
				using (TcpClient client = new TcpClient("127.0.0.1", port)) {
					Byte[] data = Encoding.ASCII.GetBytes (message);
					using (NetworkStream stream = client.GetStream ()) {
						stream.Write (data, 0, data.Length);
						data = new Byte[256];
//						String responseData = String.Empty;
//						Int32 bytes = stream.Read (data, 0, data.Length);
//						responseData = Encoding.ASCII.GetString (data, 0, bytes);
					}
				}
			} catch (ArgumentNullException e) {
				Console.WriteLine ("ArgumentNullException: {0}", e);
			} catch (SocketException e) {
				Console.WriteLine ("SocketException: {0}", e);
			}
		}
	}
}
