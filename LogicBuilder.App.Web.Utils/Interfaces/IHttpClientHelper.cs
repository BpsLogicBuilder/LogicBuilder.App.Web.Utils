using System.Text.Json;
using System.Threading.Tasks;

namespace LogicBuilder.App.Web.Utils.Interfaces
{
    public interface IHttpClientHelper
    {
        Task<TResult> GetAsync<TResult>(string url, JsonSerializerOptions? options = null);
        Task<TResult> PostAsync<TResult>(string url, string jsonObject, JsonSerializerOptions? options = null);
        Task<TResult> PutAsync<TResult>(string url, string jsonObject, JsonSerializerOptions? options = null);
    }
}
