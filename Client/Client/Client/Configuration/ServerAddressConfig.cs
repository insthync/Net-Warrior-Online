using System;
using System.Collections.Generic;
using System.Text;

namespace MMORPGCopierClient
{
    public class ServerAddressConfig
    {
        private string ip;
        private int port;
        public ServerAddressConfig(string ip, int port)
        {
            this.ip = ip;
            this.port = port;
        }

        public string getIP()
        {
            return ip;
        }

        public int getPort()
        {
            return port;
        }
    }
}
