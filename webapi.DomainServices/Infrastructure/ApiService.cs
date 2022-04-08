using Fusillade;
using ModernHttpClient;
using Refit;
using System;
using System.Net.Http;

namespace webapi.DomainServices.Infrastructure.ApiServices
{
    public class ApiService<T> : IApiService<T>
    {
        private readonly Func<HttpMessageHandler, T> _createClient;

        public ApiService(string apiBaseAddress, int apiPort)
        {
            _createClient = messageHandler =>
            {
                var client = new HttpClient(messageHandler)
                {
                    //BaseAddress = new Uri(apiBaseAddress),
                    BaseAddress = (apiPort == 0) ? new Uri(apiBaseAddress) : new Uri($"{apiBaseAddress}:{apiPort}"),
                    DefaultRequestHeaders = { ConnectionClose = true }
                };

                return RestService.For<T>(client);
            };
        }

        private T Background
        {
            get
            {
                return new Lazy<T>(() => _createClient(
                    new RateLimitedHttpMessageHandler(new NativeMessageHandler(), Priority.Background))).Value;
            }
        }

        private T UserInitiated
        {
            get
            {
                return new Lazy<T>(() => _createClient(
                    new RateLimitedHttpMessageHandler(new NativeMessageHandler(), Priority.UserInitiated))).Value;
            }
        }

        private T Speculative
        {
            get
            {
                return new Lazy<T>(() => _createClient(
                    new RateLimitedHttpMessageHandler(new NativeMessageHandler(), Priority.Speculative))).Value;
            }
        }

        public T GetApi(Priority priority)
        {
            switch (priority)
            {
                case Priority.Background:
                    return Background;
                case Priority.UserInitiated:
                    return UserInitiated;
                case Priority.Speculative:
                    return Speculative;
                default:
                    return UserInitiated;
            }
        }
    }
}
