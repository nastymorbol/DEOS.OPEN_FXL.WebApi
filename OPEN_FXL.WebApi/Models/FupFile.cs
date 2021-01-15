using Fup;
using FupClass.BDF;
using FxlApiV1;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
using System.Threading.Tasks;

namespace FXL.WebApi
{
    public class FupFile : Fup.FupFileBase
    {
        protected readonly FxlApiV1.Client client;
        protected readonly MemoryCache cache = MemoryCache.Default;
        protected readonly FxlApiV1.FupPage apiFupPage;
        protected readonly Lazy<FxlApiV1.FupPageInfo> apiFupPageInfo;
        private string _MakroQuelleWithoutPath;

        public FupFile(FxlApiV1.Client client, FxlApiV1.FupPage apiFupPage)
        {
            this.client = client;
            this.apiFupPage = apiFupPage;

            this.apiFupPageInfo = new Lazy<FxlApiV1.FupPageInfo>(() => {
                var info = GetFromCache($"{apiFupPage.Id}_Info", (x) => x, client.GetFupPageInfoAsync(apiFupPage.Id)).ConfigureAwait(false).GetAwaiter().GetResult();
                return info;
            });
        }

        public async Task<IEnumerable<Fup.Definitionfile.Definition>> GetDefinitionsAsync()
        {
            var request = await GetDefinitionsFromCache();

            return request;
        }

        public string Name => apiFupPage.Name?.ToLower();

        public string? Source => apiFupPageInfo.Value?.Source;

        public string Id => apiFupPage.Id;

        public void SetDefinition(string key, string value, string comment = null, string hint = null)
        {
            //var definition = GetDefinitionsAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            client.SetDefinitionsAsync(Id, new[]
            {
                new Definition
                {
                    Value=value,
                    Key=key,
                    Comment=comment,
                    Hint=hint
                }
            }).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public void SetDefinitions(IEnumerable<Fup.Definitionfile.Definition> definitions)
        {
            var defs = definitions.Select(d => new FxlApiV1.Definition
            {
                Key=d.Definitionsname,
                Value=d.DefinitionsEintrag,
                Comment=d.Kommentar,
                Hint=d.Hinweis
            });

            client.SetDefinitionsAsync(Id, defs).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        internal IEnumerable<FupIo> GetIos()
        {
            var ios = new List<FupIo>();
            for (int i = 0; i < 21; i++)
            {
                var io = new FupIo(this, i, i);
                if (io.IoExists)
                {
                    ios.Add(io);
                }
            }
            return ios;
        }

        private async Task<IEnumerable<Fup.Definitionfile.Definition>> GetDefinitionsFromCache()
        {
            var definitions = cache[$"{Id}_Definitions"] as IEnumerable<Fup.Definitionfile.Definition>;
            if (definitions == default)
            {
                var defs = await GetDefinitionsRequestFromCache();                
                if (defs == default)
                {
                    Debug.WriteLine($"Defintions {FupBlattName} not found");
                    return default;
                }

                var deflist = new List<Fup.Definitionfile.Definition>();

                foreach (var def in defs)
                {
                    var definition = new Fup.Definitionfile.Definition
                    {
                        Definitionsname = def.Key?.Trim(),
                        DefinitionsEintrag = def.Value?.Trim(),
                        Hinweis = def.Hint?.Trim(),
                        Kommentar = def.Comment?.Trim(),
                        Exists = true,
                        Zeilennummer = -1
                    };

                    deflist.Add(definition);
                }

                definitions = deflist;

                var policy = new CacheItemPolicy();
                policy.SlidingExpiration = TimeSpan.FromSeconds(300);

                cache.Set($"{Id}_Definitions", definitions, policy);
            }

            return definitions;
        }

        private async Task<ICollection<FxlApiV1.Definition>> GetDefinitionsRequestFromCache()
        {
            var definitions = cache[$"{Id}_DefinitionsRequest"] as ICollection<FxlApiV1.Definition>;

            if (definitions == default)
            {
                try
                {
                    definitions = (await client.GetDefinitionsAsync(Id));
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"Error: {nameof(FxlApiV1.FupPage)} > {e.InnerException?.Message}");
                }

                if (definitions == null)
                    return default;

                var policy = new CacheItemPolicy();
                policy.SlidingExpiration = TimeSpan.FromSeconds(300);

                cache.Set($"{Id}_DefinitionsRequest", definitions, policy);
            }

            return definitions;
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

        public override string MakroQuelleWithoutPath
        {
            get
            {
                if (_MakroQuelleWithoutPath == null)
                {
                    var source = apiFupPageInfo.Value?.Source;
                    if (string.IsNullOrWhiteSpace(source))
                        return _MakroQuelleWithoutPath;

                    _MakroQuelleWithoutPath = Path.GetFileName(source).ToLower().Replace(Fup.FupFile.FUP_EXTENSTION_FUP, "");
                }
                return _MakroQuelleWithoutPath;
            }
        }

        public override string FupBlattName => Name;

        //{ get => throw new NotImplementedException(); protected set => throw new NotImplementedException(); }

        public override Fup.Definitionfile.Definition GetDefinition(string key)
        {
            var def = this.GetDefinitionAsync(key).ConfigureAwait(false).GetAwaiter().GetResult();
            return def;
        }

        private async Task<Fup.Definitionfile.Definition> GetDefinitionAsync(string key)
        {
            var defs = await GetDefinitionsAsync();
            var def = defs.FirstOrDefault(d => d.Definitionsname == key);
            if (def == default)
                return Fup.Definitionfile.Definition.Error;

            return def;
        }

        public override IEnumerable<FXL_FUPELEMENT_LINE> GetFxlFupElements(Func<string, bool> predicate = null, Func<string, bool> breakcondition = null)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<GraphicTitle> GrafikTitles()
        {
            throw new NotImplementedException();
        }
    }
}