using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Caching;
using System.Threading.Tasks;

namespace FXL.WebApi
{
    public class Controller : BaseClient
    {
        private readonly FxlApiV1.Controller apiController;
        private readonly Lazy<FxlApiV1.ControllerInfo> apiControllerInfo;

        public Controller(FxlApiV1.Client client, FxlApiV1.Controller apiController) : base(client)
        {
            this.apiController = apiController;
            this.apiControllerInfo = new Lazy<FxlApiV1.ControllerInfo>( () => {
                try
                {
                    var info = GetFromCache($"{apiController.Id}_Info", (x) => x, client.GetControllerInfoAsync(apiController.Id)).ConfigureAwait(false).GetAwaiter().GetResult();
                    return info;
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"{e}");
                }

                return null;
            });
        }

        public string Name => apiController.Name;

        public string Ip => apiControllerInfo.Value?.Ip;

        public string Id => apiController.Id;

        public async IAsyncEnumerable<FupFile> GetFupFilesAsync()
        {
            foreach (var fupPage in await GetFupFileRequestFromCache())
            {
                yield return await GetFupFileFromCache(fupPage.Id);
            }
        }

        public Task BuildAsync(TimeSpan timeout = default, bool debug = false, bool onlyObj = false)
        {
            return Task.Run(async () =>
            {
                var timeouts = DateTime.Now + (timeout == default ? TimeSpan.FromSeconds(300) : timeout);

                FxlApiV1.Process process = default;

                while (DateTime.Now < timeouts)
                {
                    try
                    {
                        process = await client.BuildControllerProgramAsync(Id, new FxlApiV1.BuildParameters
                        {
                            Debug = debug,
                            OnlyObj = onlyObj
                        });
                        break;
                    }
                    catch (FxlApiV1.ApiException api)
                    {
                        switch (api.StatusCode)
                        {
                            case 202:
                                break;
                            case 302:
                                return;
                            default:
                                break;
                        }
                        await Task.Delay(2000);

                        continue;
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }

                while (DateTime.Now < timeouts)
                {
                    try
                    {
                        await client.GetProcessStatusAsync(process.Id);
                        return;
                    }
                    catch(FxlApiV1.ApiException api)
                    {
                        switch (api.StatusCode)
                        {
                            case 202:
                                break;
                            case 302:
                                return;
                            default:
                                break;
                        }
                        await Task.Delay(2000);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }

                throw new TimeoutException($"Process BuildController timed out");
            });
        }

        private async Task<FupFile> CreateFupFileAsync(string name)
        {
            var fupFile = await GetFupFileAsync(name);
            if (fupFile != default)
                return fupFile;

            var info = await client.InsertFupPageAsync(Id, new FxlApiV1.FupPageInfo
            {
                Name = name,                
            });

            cache.Remove($"{Id}_FupFileRequest");

            fupFile = await GetFupFileFromCache(info.Id);

            return fupFile;
        }

        public async Task<IEnumerable<Fup.FupIo>> GetIosAsync()
        {
            var ios = new List<Fup.FupIo>();
            var fupPages = await GetFupFileRequestFromCache();

            foreach (var fupPage in fupPages)
            {
                var fupFile = await GetFupFileFromCache(fupPage.Id);
                if (!fupFile.IsIoBlatt)
                    continue;
                var mios = fupFile.GetIos();
                ios.AddRange(mios);
            }

            return ios;
        }

        public async Task DeleteFupFile(FupFile fupFile)
        {
            await client.DeleteFupPageAsync(fupFile.Id);

            cache.Remove(fupFile.Id);
            cache.Remove($"{Id}_FupFileRequest");
        }

        public async Task<FupFile> CopyMacroAsync(string sourceId, string name, bool macroStatus = true)
        {
            var fupFile = await GetFupFileAsync(name);
            if (fupFile != default)
                return fupFile;

            var info = await client.InsertFupPageAsync(Id, new FxlApiV1.FupPageInfo
            {
                SourceId = sourceId,
                Name = name,
                MacroStatus = macroStatus                
            });

            cache.Remove($"{Id}_FupFileRequest");

            fupFile = await GetFupFileFromCache(info.Id);

            return fupFile;
        }

        public async Task<FupFile> GetFupFileAsync(string fupFilename)
        {
            var fupPages = await GetFupFileRequestFromCache();
            var fupPage = fupPages.FirstOrDefault(f => f.Name.Equals(fupFilename, StringComparison.InvariantCultureIgnoreCase));
            if (fupPage == default)
            {
                return default;
            }

            return await GetFupFileFromCache(fupPage.Id);
        }
    
        private async Task<FupFile> GetFupFileFromCache(string id)
        {
            var fupFile = cache[id] as FupFile;
            if(fupFile == default)
            {
                var fupPages = await GetFupFileRequestFromCache();
                var fupPage = fupPages.FirstOrDefault(f => f.Id == id);
                if(fupPage == default)
                {
                    Debug.WriteLine($"FupPage {id} not found");
                    return default;
                }

                fupFile = new FupFile(client, fupPage);
                var policy = new CacheItemPolicy();
                policy.SlidingExpiration = TimeSpan.FromSeconds(300);

                cache.Set(fupFile.Id, fupFile, policy);
            }

            return fupFile;
        }

        private async Task<ICollection<FxlApiV1.FupPage>> GetFupFileRequestFromCache()
        {
            var fupFiles = cache[$"{Id}_FupFileRequest"] as ICollection<FxlApiV1.FupPage>;

            if (fupFiles == default)
            {
                try
                {
                    fupFiles = await client.GetFupPagesAsync(Id);
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"Error: {nameof(FxlApiV1.FupPage)} > {e.InnerException?.Message}");
                }

                if (fupFiles == null)
                    return default;

                var policy = new CacheItemPolicy();
                policy.SlidingExpiration = TimeSpan.FromSeconds(300);

                cache.Set($"{Id}_FupFileRequest", fupFiles, policy);
            }

            return fupFiles;
        }

    }
}