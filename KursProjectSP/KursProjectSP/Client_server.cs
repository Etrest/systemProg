using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

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
            SendMessage( $"{user.Name}, welcome to your new session", -1);
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
        public void SendMessage(string msg, int ID)
        {
           
            var answer = "";

            var userByID = users.FirstOrDefault(i => i.ID == ID);
            if (userByID != null)
            {
                if (ID < 0)
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
                new Path("x1", "x2", (11/15)),
                new Path("x1", "x3", (1/2)),
                new Path("x1", "x5", (14 / 8)),
                //x2
                new Path("x2", "x3", (8 / 9)),
                new Path("x2", "x4", (16 / 15)),
                //x3
                new Path("x3", "x5", (3 / 9)),
                new Path("x3", "x4", (10 / 12)),
                new Path("x3", "x6", (15 / 8)),
                //x4
                new Path("x4", "x8", (12 / 10)),
                //x5
                new Path("x5", "x7", (8 / 9)),
                //x6
                new Path("x6", "x7", (5 / 6)),
                new Path("x6", "x9", (4 / 7)),
                //x7
                new Path("x7", "x9", (15 / 10)),
                //x8
                new Path("x8", "x9", (14 / 11)),

            };

        static string BellmanFord()
        {
            // Implementation of Bellman Ford algorithm based on Rob Conery's "Imposters Handbook"
            // This is my conversion of JavaScript original in book


            foreach (string vertex in vertices)
            {
                if (!Iterate())
                    break;
            }

            var serverAnswer = string.Join(";\n\r", memo.Select(x => $"{x.Key} {x.Value}").ToList());

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
                    // If from is maxvalue, it's wrapping around, so handle that!
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
