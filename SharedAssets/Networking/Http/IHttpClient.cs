using System.Threading.Tasks;

namespace Assets.SharedAssets.Networking.Http
{
    public interface IHttpClient
    {
        Task<TResponse> GetAsync<TResponse>(string endpoint, string token = null);
        Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest body, string token = null);
        Task<TResponse> PutAsync<TRequest, TResponse>(string endpoint, TRequest body, string token = null);
        Task<bool> DeleteAsync(string endpoint, string token = null);
    }
}