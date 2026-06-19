using LogicBuilder.App.Web.Utils.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LogicBuilder.App.Web.Utils
{
    public class HttpClientHelper : IHttpClientHelper
    {
        public Task<TResult> GetAsync<TResult>(string url)
        {
            throw new NotImplementedException();
        }

        public Task<TResult> PostAsync<TResult>(string url, string jsonObject)
        {
            throw new NotImplementedException();
        }

        public Task<TResult> PutAsync<TResult>(string url, string jsonObject)
        {
            throw new NotImplementedException();
        }
    }
}
