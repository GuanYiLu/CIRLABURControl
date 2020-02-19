using System;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Text;


namespace CIRLABURControl
{

    public class ListenerBaseTcpListener : IDisposable
    {
        
        TcpListener _server;
        NetworkStream _stream;
        bool _isServerRun;
        public ListenerBaseTcpListener(int port, string address)
        {
            
            _server = new TcpListener(IPAddress.Parse(address), port);

            _server.Start();

        }

        public void RunServer()
        {
            _isServerRun = true;
            // 持續監聽
            while (_isServerRun)
            {
                

                // Perform a blocking call to accept requests.
                // You could also user server.AcceptSocket() here.
                TcpClient client = _server.AcceptTcpClient();
                


                // Get a stream object for reading and writing
                _stream = client.GetStream();
                //_stream.Write(new byte[] { 3 });
                // 只要還連著就不會離開
                while (client.Connected)
                {
                    // 空轉起來！！
                    
                }

                client.Close();
                Console.WriteLine("DisConnected!");
            }
        }
        public void CloseServer()
        {
            _isServerRun = false;
        }

        

        public TcpListener GetTcpListener()
        {
            return _server;
        }
        public NetworkStream GetNetworkStream()
        {
            return _stream;
        }

        public void Dispose()
        {
            CloseServer();
        }

        //void KeepRead(NetworkStream stream)
        //{
        //    byte[] myReadBuffer = new byte[1024];

        //    if (stream.CanRead)
        //    {

        //        StringBuilder myCompleteMessage = new StringBuilder();

        //        // Incoming message may be larger than the buffer size.

        //        int numberOfBytesRead = stream.Read(myReadBuffer, 0, myReadBuffer.Length);

        //        _ = myCompleteMessage.AppendFormat("{0}", Encoding.ASCII.GetString(myReadBuffer, 0, numberOfBytesRead));



        //        // Print out the received message to the console.
        //        Console.WriteLine("You received the following message : " + myCompleteMessage);
        //        byte[] StatusCode;
        //        if (myCompleteMessage.ToString().Length < 2)
        //        {
        //            StatusCode = Encoding.UTF8.GetBytes("(0)");
        //            stream.Write(StatusCode, 0, StatusCode.Length);
        //        }
        //        var pose = URHandler.PoseToFloatList(myCompleteMessage.ToString());

        //        if (pose[1] <= -20)
        //        {
        //            StatusCode = Encoding.UTF8.GetBytes("(1)");

        //        }
        //        else
        //        {
        //            StatusCode = Encoding.UTF8.GetBytes("(0)");
        //        }
        //        stream.Write(StatusCode, 0, StatusCode.Length);

        //    }
        //    else
        //    {
        //        Console.WriteLine("Sorry.  You cannot read from this NetworkStream.");

        //        return;
        //    }
        //}
    }


}