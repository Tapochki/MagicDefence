using GrandDevs.AppName.Common;

namespace GrandDevs.AppName
{
    public class UserLocalData
    {
        public Enumerators.Language appLanguage;


        public UserLocalData()
        {
            Reset();
        }

        public void Reset()
        {
            appLanguage = Enumerators.Language.NONE;
        }
    }
}