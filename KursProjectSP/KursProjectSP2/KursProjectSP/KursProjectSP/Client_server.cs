﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace KursProjectSP
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени класса "Client_server" в коде и файле конфигурации.
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class Client_server : IClient_server
    {
        List<ServerUser> users = new List<ServerUser>();
        int nextId = 1;
        public int Connect(string name)
        {
            ServerUser user = new ServerUser()
            {
                ID = nextId,
                Name = name,
                operationContext = OperationContext.Current
            };

            nextId++;
            SendMessage($"{user.Name}, welcome to your new session", -1);
            users.Add(user);
            return user.ID;
        }

        public void Disconnect(int id)
        {
            var user = users.FirstOrDefault(i => i.ID == id);
            if (user != null)
            {
                users.Remove(user);
                SendMessage($"{user.Name} has been disconected", -1);
            }
        }

        public void SendMessage(string msg, int id)
        {
            var answer = "";
            var userByID = users.FirstOrDefault(i => i.ID == id);
            Console.WriteLine(userByID is null);
            if (userByID != null)
            {
                if (id < 1)
                {
                    answer = msg;
                }
                else
                {
                    answer = BellmanFord();
                }
                userByID.operationContext.GetCallbackChannel<IClientserverCallBack>().MessageCallBack(answer);
            }
        }


        private static List<string> vertices = new List<string>() { "x1", "x2", "x3", "x4", "x5", "x6", "x7", "x8", "x9" };

        static Dictionary<string, double> memo = new Dictionary<string, double>()
            {
                { "x1", 0 },
                {"x2", double.MaxValue },
                {"x3", double.MaxValue },
                {"x4", double.MaxValue },
                {"x5", double.MaxValue },
                {"x6", double.MaxValue },
                {"x7", double.MaxValue },
                {"x8", double.MaxValue },
                {"x9", double.MaxValue },
            };

        static List<Path> graph = new List<Path>()
            {
                // Values given in original JavaScript version in book are wrong!
                new Path("x1", "x2", (11.0/15)),
                new Path("x1", "x3", (1.0/2)),
                new Path("x1", "x5", (14.0 / 8)),
                //x2
                new Path("x2", "x3", (8.0 / 9)),
                new Path("x2", "x4", (16.0 / 15)),
                //x3
                new Path("x3", "x5", (3.0 / 9)),
                new Path("x3", "x4", (10.0 / 12)),
                new Path("x3", "x6", (15.0 / 8)),
                //x4
                new Path("x4", "x8", (12.0 / 10)),
                //x5
                new Path("x5", "x7", (8.0 / 9)),
                //x6
                new Path("x6", "x7", (5.0 / 6)),
                new Path("x6", "x9", (4.0 / 7)),
                //x7
                new Path("x7", "x9", (15.0 / 10)),
                //x8
                new Path("x8", "x9", (14.0 / 11)),

            };

        static string BellmanFord()
        {

            foreach (string vertex in vertices)
            {
                if (!Iterate())
                    break;
            }

            var serverAnswer = "";
            foreach (var item in memo)
            {
                serverAnswer += $"key:{item.Key}, value: {item.Value}\n\r";
            }

            return serverAnswer;

        }

        private static bool Iterate()
        {
            // Do we need another iteration? Decided below
            bool doItAgain = false;

            // Loop all vertices
            foreach (string fromVertex in vertices)
            {
                Path[] edges = graph.Where(x => x.From == fromVertex).ToArray();

                foreach (Path edge in edges)
                {
                    double potentialCost = memo[edge.From];
                    if (potentialCost != double.MaxValue)
                        potentialCost += edge.Cost;

                    if (potentialCost < memo[edge.To])
                    {
                        memo[edge.To] = potentialCost;
                        doItAgain = true;
                    }
                }
            }

            return doItAgain;


        }
    }
}
