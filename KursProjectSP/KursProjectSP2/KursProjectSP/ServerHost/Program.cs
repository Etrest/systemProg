﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace ServerHost
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (var host = new ServiceHost(typeof(KursProjectSP.Client_server)))
            {
                host.Open();
                Console.WriteLine("Host has been started");
                Console.ReadLine();
            }
        }
    }
}
