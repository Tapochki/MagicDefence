namespace GrandDevs.AppName
{
    public class ServerConfiguration
    {
#if DEVELOPMENT
        private const string DOMAIN = "http://rome2018.cyberrit.net";
#else
        private const string DOMAIN = "http://104.155.48.251/";
#endif

        private const float RESPONSE_CHECKTIME = 0.5f;
        private const float REQUEST_TIMEOUT = RESPONSE_CHECKTIME * 15f;

        public static string GetServerDomain()
        {
            return DOMAIN;
        }
        public static float CheckResponseTime()
        {
            return RESPONSE_CHECKTIME;
        }
        public static float GetRequestTimeout()
        {
            return REQUEST_TIMEOUT;
        }
    }
}
