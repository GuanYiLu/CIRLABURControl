using System;
using System.Net.Sockets;
using System.Net;
using System.Text;


namespace URControler2
{
    public class ListenerBaseSocket
    {
        public static void StartListeningBaseSocket()
        {
            // Data buffer for incoming data.  
            byte[] bytes = new Byte[1024];

            // Establish the local endpoint for the socket.  
            // Dns.GetHostName returns the name of the   
            // host running the application.  
            //IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            //Console.WriteLine(Dns.GetHostName());
            IPAddress ipAddress = IPAddress.Parse("0.0.0.0");
            //foreach(IPAddress ip in ipHostInfo.AddressList)
            //{
            //    Console.WriteLine(ip);
            //}

            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 1000);
            Console.WriteLine(localEndPoint);
            // Create a TCP/IP socket.  
            Socket listener = new Socket(ipAddress.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and   
            // listen for incoming connections.  
            try
            {
                string data;
                listener.Bind(localEndPoint);
                listener.Listen(10);

                // Start listening for connections.  
                while (true)
                {
                    Console.WriteLine("Waiting for a connection...");
                    // Program is suspended while waiting for an incoming connection.  
                    Socket handler = listener.Accept();
                    data = null;

                    // An incoming connection needs to be processed.  
                    int bytesRec = handler.Receive(bytes);
                    data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                    if (data.IndexOf("<EOF>") > -1)
                    {
                        break;
                    }

                    // Show the data on the console.  
                    Console.WriteLine("Text received : \n{0}", data);

                    // Echo the data back to the client.  
                    byte[] msg = Encoding.ASCII.GetBytes("Hello World!!");

                    handler.Send(msg);
                    handler.Shutdown(SocketShutdown.Both);
                    System.Threading.Thread.Sleep(1000);
                    handler.Close();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.Read();

        }
    }
}
