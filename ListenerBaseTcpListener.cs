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
        TcpClient _client;
        bool _isServerRun;
        public ListenerBaseTcpListener(int port, string address)
        {
            
            _server = new TcpListener(IPAddress.Parse(address), port);

            _server.Start();

        }
        public void RunServer()
        {
            _isServerRun = true;

            try
            {
                // 持續監聽
                while (_isServerRun)
                {


                    // Perform a blocking call to accept requests.
                    // You could also user server.AcceptSocket() here.
                    _client = _server.AcceptTcpClient();



                    // Get a stream object for reading and writing
                    _stream = _client.GetStream();
                    //_stream.Write(new byte[] { 3 });
                    // 只要還連著就不會離開
                    while (_client.Connected)
                    {
                        // 空轉起來！！

                    }

                    _client.Dispose();
                    Console.WriteLine("DisConnected!");
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                _server.Stop();
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
            _server.Stop();
            if (_client.Connected)
                _stream.Dispose();
            CloseServer();
        }
    }


}