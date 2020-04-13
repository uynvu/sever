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
            {"1","Thomas"},
            {"2","John"},
            {"3","Niki"},
            {"4","Tom"},
            {"5","Rose"},
        };
        public static void Main()
        {

            IPAddress address = IPAddress.Parse("127.0.0.1");

            TcpListener listener = new TcpListener(address, PORT_NUMBER);

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
                try
                {
                    var stream = new NetworkStream(socket);
                    var reader = new StreamReader(stream);
                    var writer = new StreamWriter(stream);
                    writer.AutoFlush = true;
                    writer.WriteLine("Welcom to server");
                    writer.WriteLine("Please enter id:");
                    while (true)
                    {
                        // 2. receive
                        string str = reader.ReadLine();
                        if (String.IsNullOrEmpty(str))
                            break;
                        if (_data.ContainsKey(str))
                        {
                            if (str.ToUpper() == "EXIT")
                            {
                                writer.WriteLine("bye");
                                break;
                            }
                            // 3. send
                            switch (str)
                            {
                                case "0":
                                    writer.WriteLine("Zero");
                                    break;
                                case "1":
                                    writer.WriteLine("One");
                                    break;
                                case "2":
                                    writer.WriteLine("Two");
                                    break;
                                case "3":
                                    writer.WriteLine("Three");
                                    break;
                                case "4":
                                    writer.WriteLine("Four");
                                    break;
                                case "5":
                                    writer.WriteLine("Five");
                                    break;
                                case "6":
                                    writer.WriteLine("Six");
                                    break;
                                case "7":
                                    writer.WriteLine("Seven");
                                    break;
                                case "8":
                                    writer.WriteLine("Eight");
                                    break;
                                case "9":
                                    writer.WriteLine("Nine");
                                    break;
                            }
                        }
                        else writer.WriteLine("Can't find id");
                    }
                    // 4. close
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
