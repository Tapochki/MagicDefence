namespace GrandDevs.AppName.Common
{
    public class Enumerators
    {
        public enum AppState
        {
            NONE,
            APP_INIT_LOADING,
            MAIN_MENU,
        }

        public enum ButtonState
        {
            ACTIVE,
            DEFAULT
        }
        public enum ElementType
        {
            Water,
            Wind,
            Ground,
            Fire
        }
        public enum EnemyType
        {
            SmallSlime,
            MediumSlime,
            Witch,
            Boss
        }
        public enum SceneType
        {
            MAIN_MENU,
        }

        public enum SoundType : int
        {
          //  CLICK,
          //  OTHER,
         //   BACKGROUND,
        }
        public enum SpawnPosition
        {
            PlayerSpawnPosition,
            UpEnemySpawnPosition,
            LeftEnemySpawnPosition,
            RightEnemySpawnPosition,
            LeftUpEnemySpawnPosition,
            RightUpEnemySpawnPosition
        }
        public enum NotificationType
        {
            LOG,
            ERROR,
            WARNING,
            MESSAGE
        }

        public enum Language
        {
            NONE,

            DE,
            EN,
            UK
        }

        public enum ScreenOrientationMode
        {
            PORTRAIT,
            LANDSCAPE
        }

        public enum CacheDataType
        {
            USER_LOCAL_DATA
        }

        public enum NotificationButtonState
        {
            ACTIVE,
            INACTIVE
        }
    }
}