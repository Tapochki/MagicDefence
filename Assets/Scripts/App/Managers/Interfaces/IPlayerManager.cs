using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GrandDevs.AppName
{
    public interface IPlayerManager
    {
        Player LocalPlayer { get; set; }
    }
}