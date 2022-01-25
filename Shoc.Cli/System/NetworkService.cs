using System.Net;
using System.Net.Sockets;

namespace Shoc.Cli.System
{
    /// <summary>
    /// The network service 
    /// </summary>
    public class NetworkService
    {
        /// <summary>
        /// Gets the next available port from the system
        /// </summary>
        /// <returns></returns>
        public int GetNextAvailablePort()
        {
            // new listener on loopback
            var listener = new TcpListener(IPAddress.Loopback, 0);

            // start listening
            listener.Start();

            // take out the port that was open
            var port = ((IPEndPoint)listener.LocalEndpoint).Port;

            // make sure to stop the listener
            listener.Stop();

            // use the available port 
            return port;
        }
    }
}