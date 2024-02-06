using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace KursProjectSP
{
    // ПРИМЕЧАНИЕ. Можно использовать команду "Переименовать" в меню "Рефакторинг", чтобы изменить имя интерфейса "IClient_server" в коде и файле конфигурации.
    [ServiceContract(CallbackContract = typeof(IClientserverCallBack))]
    public interface IClient_server
    {
        [OperationContract]
        int Connect(string name);

        [OperationContract]
        void Disconnect(int id);

        [OperationContract (IsOneWay = true)]
        void SendMessage(string msg, int ID);

        //[OperationContract(IsOneWay = true)]
        //void InvokeSendMessage();
    }

    public interface IClientserverCallBack
    {
        [OperationContract (IsOneWay = true)]
        void MessageCallBack(string msg);
    }
}
