using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GrandDevs.AppName
{
    public interface IRequest
    {
        void SendRequest(string[] obj);
        bool GetResponse();
    }
}
