using System;

namespace GrandDevs.AppName
{
    public class Request : RequestBase
    {
        private Action<string> _responseCallback;

        public Request(string url, Action<string> response) : base(url)
        {
            _responseCallback = response;
        }

        protected override void ResponseHandler(string data)
        {
            if (_responseCallback != null)
                _responseCallback(data);
        }
    }
}
