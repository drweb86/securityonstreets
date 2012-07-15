using System;
using System.Net;

namespace MessageRouter.Common
{
    public class DnsHelper
    {
        public static IPAddress[] GetAddresses(string host)
        {
            if (string.Compare(host, "localhost", StringComparison.OrdinalIgnoreCase) == 0)
            {
                return new[] { IPAddress.Loopback };
            }

            return Dns.GetHostAddresses(host);
        }
    }
}