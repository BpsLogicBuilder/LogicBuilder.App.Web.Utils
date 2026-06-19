using System.Threading.Tasks;

namespace LogicBuilder.App.Web.Utils.Interfaces
{
    public interface IHttpClientHelper
    {
        Task<TResult> GetAsync<TResult>(string url);
        Task<TResult> PostAsync<TResult>(string url, string jsonObject);
        Task<TResult> PutAsync<TResult>(string url, string jsonObject);
    }
}
