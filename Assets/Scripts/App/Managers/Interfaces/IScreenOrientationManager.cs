using GrandDevs.AppName.Common;
using UnityEngine.UI;

namespace GrandDevs.AppName
{
    public interface IScreenOrientationManager
    {
        void SwitchOrientation(Enumerators.ScreenOrientationMode mode);
        Enumerators.ScreenOrientationMode GetOrientation();
    }
}