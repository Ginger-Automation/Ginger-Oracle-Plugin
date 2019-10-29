using Amdocs.Ginger.CoreNET.Drivers.CommunicationProtocol;
using Amdocs.Ginger.Plugin.Core;
using System;

namespace Oracle
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Oracle Database plugin";
            Console.WriteLine("Starting Oracle Database Plugin");

            using (GingerNodeStarter gingerNodeStarter = new GingerNodeStarter())
            {
                if (args.Length > 0)
                {
                    gingerNodeStarter.StartFromConfigFile(args[0]);  // file name 
                }
                else
                {                    
                    gingerNodeStarter.StartNode("Oracle Service 1", new GingerOracleConnection(), SocketHelper.GetLocalHostIP(), 15001);                    
                }
                gingerNodeStarter.Listen();
            }

        }
    }
}
