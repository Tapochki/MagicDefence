using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrandDevs.AppName
{
    public interface INetworkManager
    {
        void CleanRequests();
        void SendWWWRequest(IRequest request, string[] obj);
        bool IsResponseFailed(string data);
        bool IsHasInternetConnection();
    }
}
