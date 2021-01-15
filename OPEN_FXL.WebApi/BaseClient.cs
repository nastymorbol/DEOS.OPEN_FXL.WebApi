using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Caching;
using System.Threading.Tasks;

namespace FXL.WebApi
{
    public class BaseClient
    {
        protected readonly FxlApiV1.Client client;
        protected readonly MemoryCache cache = MemoryCache.Default;

        public BaseClient(FxlApiV1.Client client)
        {
            this.client = client;
        }

        protected async Task<T1> GetFromCache<T1, T2>(string id, Func<T2, T1> generatorTask, Task<T2> requestTask)
        {
            var data = cache[id];

            if (data == null || data == default)
            {
                try
                {
                    var reqdata = await requestTask;
                    data = generatorTask(reqdata);
                }
                catch (Exception e)
                {
                    var t = requestTask.GetType().GetGenericArguments().ElementAtOrDefault(0);
                    Debug.WriteLine($"Error: {t?.Name} > {e.InnerException?.Message}");                    
                }

                if (data == null)
                    return default;

                var policy = new CacheItemPolicy();
                policy.SlidingExpiration = TimeSpan.FromSeconds(300);

                cache.Set(id, data, policy);
            }

            return (T1)data;
        }

        public FxlApiV1.Client ApiV1Client => client;
    }
}