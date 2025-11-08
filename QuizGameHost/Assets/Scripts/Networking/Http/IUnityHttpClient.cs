using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Networking;

namespace Assets.Scripts.Networking.Http
{
    public interface IUnityHttpClient : IHttpClient
    {
        UnityWebRequest CreateRequest(string method, string endpoint, string token = null, object body = null);
        Task<TResponse> SendRequestAsync<TResponse>(UnityWebRequest request);
    }
}
