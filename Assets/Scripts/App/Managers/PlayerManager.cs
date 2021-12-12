using Newtonsoft.Json;
using System;
using GrandDevs.AppName.Common;
using UnityEngine;
using System.Collections.Generic;

namespace GrandDevs.AppName
{
    public class PlayerManager : IService, IPlayerManager
    {
        private INetworkManager _networkManager;
        private IDataManager _dataManager;


        public Player LocalPlayer { get; set; }


        public void Dispose()
        {
        }
        public void Init()
        {
            _networkManager = GameClient.Get<INetworkManager>();
            _dataManager = GameClient.Get<IDataManager>();

           // LocalPlayer = new Player();
        }
        public void Update()
        {
        }
    }
}