using Fusillade;
using Newtonsoft.Json;
using Polly;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using webapi.Data.Entities;
using webapi.DomainServices.Infrastructure.ApiServices;
using webapi.DomainServices.Interfaces;
using webapi.DomainServices.Interfaces.ApiServices;
using webapi.DTOs;
using webapi.Utilities.Http;

namespace webapi.DomainServices.Infrastructure
{
    public abstract class BaseAPI<T> where T: class
    {
        #region Privates
        protected readonly IApiService<T> _remoteApi;
        private bool IsConnected { get; set; }
        private bool IsReachable { get; set; }
        private readonly Dictionary<int, CancellationTokenSource> _runningTasks = new Dictionary<int, CancellationTokenSource>();

        #endregion

        #region Constructors

        public BaseAPI(IApiService<T> cocktailAPI)
        {
            _remoteApi = cocktailAPI;
        }

        #endregion

        #region Internal Methods
        protected async Task<TData> RemoteRequestAsync<TData>(Task<TData> task) where TData : HttpResponseMessage, new()
        {
            var data = new TData();
            IsConnected = await HttpHelpers.IsConnectedtoInternet();

            if (!IsConnected)
            {
                var strngResponse = "Connection Error";
                data.StatusCode = HttpStatusCode.BadRequest;
                data.Content = new StringContent(strngResponse);
                return data;
            }

            IsReachable = true;

            if (!IsReachable)
            {
                var strngResponse = "Cant connect to server";
                data.StatusCode = HttpStatusCode.BadRequest;
                data.Content = new StringContent(strngResponse);

                return data;
            }

            data = await Policy
            .Handle<WebException>()
            .Or<ApiException>()
            .Or<TaskCanceledException>()
            .WaitAndRetryAsync
            (
                retryCount: 2,   //Retry policy in case call fails
                sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
            )
            .ExecuteAsync(async () =>
            {
                var result = await task;

                if (result.StatusCode == HttpStatusCode.Unauthorized)
                {
                    //TODO Force a logout if needed
                }
                _runningTasks.Remove(task.Id);

                return result;
            });

            return data;
        }
        protected async Task<APIResponseDTO<T>> CreateRemoteCall<T>(Task<HttpResponseMessage> taskToExecute) where T : class, new()
        {
            var cts = new CancellationTokenSource();
            _runningTasks.Add(taskToExecute.Id, cts);
            var response = await taskToExecute;
            var newResponse = new APIResponseDTO<T>() { Status = response.StatusCode, Success = response.IsSuccessStatusCode };
            if (!response.IsSuccessStatusCode)
            {
                newResponse.Status = response.StatusCode;
                newResponse.Success = response.IsSuccessStatusCode;
                return newResponse;
            }
            var result = await response.Content.ReadAsStringAsync();
            var resultDeserialize = await Task.Run(() => JsonConvert.DeserializeObject<T>(result), cts.Token);
            newResponse.Response = resultDeserialize;

            return newResponse;
        }
        #endregion
    }
}
