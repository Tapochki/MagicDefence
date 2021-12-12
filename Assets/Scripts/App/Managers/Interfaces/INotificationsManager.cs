using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GrandDevs.AppName.Common;

namespace GrandDevs.AppName
{
    public interface INotificationManager
    {
        event Action<Notification> DrawNotificationEvent;

        void DrawNotification(Enumerators.NotificationType type, string message);

    }
}