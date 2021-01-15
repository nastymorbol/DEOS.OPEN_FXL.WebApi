using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FXL.WebApi
{
    public class Project : BaseClient
    {
        private readonly FxlApiV1.Project apiProject;

        public Project(FxlApiV1.Client client, FxlApiV1.Project apiProject) : base(client)
        {
            this.apiProject = apiProject;
        }

        public string Name => apiProject.Name;

        public string Id => apiProject.Id;

        public async Task<Controller> GetControllerAsync(string name)
            => (await GetControllerAsync()).SingleOrDefault(c => c.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));

        public Task<IEnumerable<Controller>> GetControllerAsync()
            => GetFromCache($"{apiProject.Id}_Controller", (x) => x.Select(y => new Controller(client, y)), client.GetControllersAsync(apiProject.Id));

        public async Task<Controller> CreateControllerAsync(string name, FxlApiV1.ControllerInfoType type, string ip = "192.168.170.101", string subnet = "255.255.255.0", string gateway = "", string description = "DEOS Controller", bool automaticTree = true, bool bacnetWorkflow = true, FxlApiV1.ControllerInfoLibrary library = FxlApiV1.ControllerInfoLibrary._MAKBIB_WIN__V0004)
        {
            var controller = await GetControllerAsync(name);
            if (controller != default)
                return controller;

            var info = await client.CreateControllerAsync(Id, new FxlApiV1.ControllerInfo
            {
                Name = name,
                Type = type,
                Ip = ip,
                Subnet = subnet,
                Gateway = gateway,
                Description = description,
                AutomaticTree = automaticTree,
                BacnetWorkflow = bacnetWorkflow,
                Library = library
            });

            cache.Remove($"{apiProject.Id}_Controller");

            controller = (await GetControllerAsync()).SingleOrDefault(c => c.Id.Equals(info.Id));
            return controller;
        }
    }
}