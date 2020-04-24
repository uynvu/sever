using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace sever
{
    class Program
    {

        private const int BUFFER_SIZE = 1024;
        private const int PORT_NUMBER = 9999;
        private const int MAX_CONNECTION = 10;
        static TcpListener listener;
        static ASCIIEncoding encoding = new ASCIIEncoding();
        static Dictionary<string, string> _data = new Dictionary<string, string>
        {
            {"0","Zero"},
            {"1","One"},
            {"2","Two"},
            {"3","Three"},
            {"4","Four"},
            {"5","Five"},
            {"6","Six"},
            {"7","Seven"},
            {"8","Eight"},
            {"9","Nine"},   
            {"10","Ten"},
        };
        public static void Main()
        {

            IPAddress address = IPAddress.Parse("127.0.0.1");

            listener = new TcpListener(address, PORT_NUMBER);

            // 1. listen
            listener.Start();

            Console.WriteLine("Server started on " + listener.LocalEndpoint);
            Console.WriteLine("Waiting for a connection...");
            for (int i = 0; i < MAX_CONNECTION; i++)
            {
                new Thread(DoWork).Start();
            }
        }
        static void DoWork()
        {
            while (true)
            {
                Socket socket = listener.AcceptSocket();
                Console.WriteLine("Connection received from: {0}! " + socket.RemoteEndPoint);
                StringBuilder sb = new StringBuilder();
                sb.Append("IP:Port of Client: "+socket.RemoteEndPoint+";"+"Connect At: "+DateTime.Now);
                File.AppendAllText("D://Access.log", sb.ToString());
                sb.Clear();
                try
                {
                    var stream = new NetworkStream(socket);
                    var reader = new StreamReader(stream);
                    var writer = new StreamWriter(stream);
                    writer.AutoFlush = true;
                    
                    while (true)
                    {
                        string str = reader.ReadLine();
                        
                     
                        if (String.IsNullOrEmpty(str))
                            break;
                        if (str.ToUpper() == "EXIT")
                            {
                                writer.WriteLine("bye");
                                sb.Append(";Disconnect At: " + DateTime.Now+";"+"Reason: Closed By Client\n");
                                File.AppendAllText("D://access.log", sb.ToString());
                                sb.Clear();
                                break;
                            }
                            // 3. send
                        if (_data.ContainsKey(id))
                            writer.WriteLine("Number you've entered: '{0}'", _data[id]);
                        else
                            writer.WriteLine("Number is not valid !");
                        }
                       stream.Close(); 
                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex);
                }
                Console.WriteLine("Client disconnected: {0}", socket.RemoteEndPoint);
                socket.Close();
            }
        }
    }
}
