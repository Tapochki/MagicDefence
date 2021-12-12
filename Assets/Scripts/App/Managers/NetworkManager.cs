using System.Collections.Generic;

namespace GrandDevs.AppName
{
    public class NetworkManager : IService, INetworkManager
    {
        private List<IRequest> requests;
        private List<IRequest> requestsToRemove;
        private List<IRequest> requestsToAdd;

        public void Dispose()
        {
        }

        public void Init()
        {
            requests = new List<IRequest>();
            requestsToRemove = new List<IRequest>();
            requestsToAdd = new List<IRequest>();
            
            StartResponseTimer();
        }

        public void Update()
        {

        }

        public void StartResponseTimer()
        {
            GameClient.Get<ITimerManager>().AddTimer(CheckResponse, null, ServerConfiguration.CheckResponseTime(), true);
        }

        public void SendWWWRequest(IRequest request, string[] obj)
        {
            request.SendRequest(obj);
            requestsToAdd.Add(request);
        }

        public void CleanRequests()
        {
            requestsToAdd.Clear();
            requests.Clear();
            requestsToRemove.Clear();
        }

        private void CheckResponse(object obj)
        {
            if (requestsToAdd.Count > 0)
            {
                foreach (var request in requestsToAdd)
                {
                    requests.Add(request);
                }
            }
            requestsToAdd.Clear();

            if (requests.Count > 0)
            {
                foreach (var request in requests)
                {
                    if (request.GetResponse())
                    {
                        requestsToRemove.Add(request);
                    }
                }
            }
            if (requestsToRemove.Count > 0)
            {
                foreach (var request in requestsToRemove)
                {
                    requests.Remove(request);
                }
                requestsToRemove.Clear();
            }
        }

        public bool IsResponseFailed(string response)
        {
            if (response.Substring(@"""result"":".Length + 1, false.ToString().Length) == false.ToString().ToLower())
                return true;
            return false;
        }

        public bool IsHasInternetConnection()
        {
            return !(UnityEngine.Application.internetReachability == UnityEngine.NetworkReachability.NotReachable);
        }
    }
}