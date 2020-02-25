using System.Collections.Generic;
using RestSharp;
using RestSharp.Authenticators;
using System.Collections;
using System.Collections.Specialized;
using System;
using Newtonsoft.Json;
using System.Threading.Tasks;
using GoodCrud.Contract.Dtos;

namespace GoodCrud.Client
{
    public class CrudClient<T, CreateT, UpdateT, FilterT>
    where T : EntityDto
    where CreateT : class
    where UpdateT : class
    where FilterT : FilterDto
    {
        public string Website = "https://api.mailgun.net/";
        public string Prefix;
        public string Controller;

        public string Username;
        public string Password;
        public CrudClient(string website, string prefix, string controller, string username = null, string password = null)
        {
            Website = website;
            Prefix = prefix;
            Controller = controller;
            Username = username;
            Password = password;

            // todo
            // options.JsonSerializerOptions.IgnoreNullValues = true;
            // options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
        }

        public virtual async Task<PagedListDto<T>> GetsAsync(FilterT filter = null)
        {
            var options = QueryOptions(filter);
            return await RequestResult<PagedListDto<T>>(
                $"{Prefix}/{Controller}", Method.GET, options);
        }

        public async Task<ResultDto<T>> GetAsync(int id)
        {
            return await RequestResult<ResultDto<T>>(
                $"{Prefix}/{Controller}/{id}", Method.GET);
        }

        public async Task<ResultDto<T>> UpdateAsync(int id, UpdateT dto)
        {
            return await RequestResult<ResultDto<T>>(
                $"{Prefix}/{Controller}/{id}", Method.PUT, null, dto);
        }

        public async Task<ResultDto<T>> CreateAsync(CreateT dto)
        {
            return await RequestResult<ResultDto<T>>(
                $"{Prefix}/{Controller}/", Method.POST, null, dto);
        }

        public async Task<List<ResultDto<T>>> BulkCreateAsync(List<CreateT> dtoList)
        {
            return await RequestResult<List<ResultDto<T>>>(
                $"{Prefix}/{Controller}/Bulk", Method.POST, null, dtoList);
        }

        public async Task<ResultDto<T>> DeleteAsync(int id)
        {
            return await RequestResult<ResultDto<T>>(
                $"{Prefix}/{Controller}/{id}", Method.DELETE);
        }

        public virtual async Task<string> Request(string url, Method method, StringDictionary options = null, dynamic data = null)
        {
            var client = new RestClient(Website)
            {
                Authenticator = new HttpBasicAuthenticator(Username, Password)
            };

            var request = new RestRequest(url, method);
            if (options != null)
            {
                foreach (DictionaryEntry item in options)
                {
                    request.AddParameter((string)item.Key, item.Value);
                }
            }

            if (data != null) { request.AddJsonBody(data); }

            IRestResponse response = await client.ExecuteAsync(request);
            var content = response.Content;
            return content;
        }

        protected StringDictionary QueryOptions<C>(C c) where C : class
        {
            if (c == null) { return null; }

            var options = new StringDictionary();
            foreach (var property in typeof(C).GetProperties())
            {
                options[property.Name] = Convert.ToString(property.GetValue(c));
            }
            return options;
        }

        public virtual async Task<CT> RequestResult<CT>(string url, Method method, StringDictionary options = null, dynamic data = null)
        {
            var content = await Request(url, method, options, data);
            var result = JsonConvert.DeserializeObject<CT>(content);
            return result;
        }
    }
}