using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.Networking.Http
{
    public class UnityHttpClient : IUnityHttpClient
    {
        private readonly string _baseUrl;
        private readonly int _timeoutSeconds;

        public UnityHttpClient(string baseUrl, int timeoutSeconds = 10)
        {
            _baseUrl = baseUrl.TrimEnd('/');
            _timeoutSeconds = timeoutSeconds;
        }

        public UnityWebRequest CreateRequest(string method, string endpoint, string token = null, object body = null)
        {
            method = method.ToUpper();

            var url = _baseUrl + endpoint;

            UnityWebRequest request = new UnityWebRequest(url, method);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Accept", "application/json");
            request.timeout = _timeoutSeconds;

            if (method == UnityWebRequest.kHttpVerbPOST ||
                method == UnityWebRequest.kHttpVerbPUT)
            { 
                var json = body != null ? JsonConvert.SerializeObject(body) : "{}";
                var bytes = Encoding.UTF8.GetBytes(json);
                request.uploadHandler = new UploadHandlerRaw(bytes);
            }

            if (!string.IsNullOrEmpty(token))
                request.SetRequestHeader("Authorization", $"Bearer {token}");

            return request;
        }

        public async Task<TResponse> SendRequestAsync<TResponse>(UnityWebRequest request)
        {
            var operation = request.SendWebRequest();

            while (!operation.isDone)
                await Task.Yield();

            if (request.result == UnityWebRequest.Result.ConnectionError ||
                request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"HTTP Error: {request.method} {request.url} -> {request.responseCode} {request.error}");
                throw new Exception($"HTTP Error {request.responseCode}: {request.error}");
            }

            var responseContent = request.downloadHandler.text;

            if (string.IsNullOrWhiteSpace(responseContent))
                return default;

            try
            { 
                return JsonConvert.DeserializeObject<TResponse>(responseContent);
            }
            catch (Exception e)
            {
                Debug.LogError($"JSON Deserialize error: {e.Message}\n{responseContent}");
                throw;
            }
        }

        public async Task<TResponse> GetAsync<TResponse>(string endpoint, string token = null)
        {
            var request = CreateRequest(UnityWebRequest.kHttpVerbGET, endpoint, token);
            return await SendRequestAsync<TResponse>(request);
        }

        public async Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest body, string token = null)
        {
            var request = CreateRequest(UnityWebRequest.kHttpVerbPOST, endpoint, token, body);
            return await SendRequestAsync<TResponse>(request);
        }

        public async Task<TResponse> PutAsync<TRequest, TResponse>(string endpoint, TRequest body, string token = null)
        {
            var request = CreateRequest(UnityWebRequest.kHttpVerbPUT, endpoint, token, body);
            return await SendRequestAsync<TResponse>(request);
        }

        public async Task<bool> DeleteAsync(string endpoint, string token = null)
        {
            var request = CreateRequest(UnityWebRequest.kHttpVerbDELETE, endpoint, token);
            await SendRequestAsync<object>(request);
            return true;
        }
    }
}