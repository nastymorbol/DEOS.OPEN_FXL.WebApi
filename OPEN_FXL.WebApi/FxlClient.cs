using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Caching;
using System.Threading.Tasks;

namespace FXL.WebApi
{
    public class FxlClient : BaseClient
    {
        public FxlClient(FxlApiV1.Client client) : base(client) { }

        public Task<IEnumerable<Project>> GetProjectsAsync() 
            => GetProjectsRequestAsync();

        private Task<IEnumerable<Project>> GetProjectsRequestAsync()
            =>  GetFromCache("projects", (x) => x.Select(y => new Project(client,y) ), client.GetProjectsAsync());

        public async Task<Project> GetProjectAsync(string projectname)
            => (await GetProjectsAsync()).SingleOrDefault(p => p.Name.Equals(projectname, StringComparison.InvariantCultureIgnoreCase));

        public async Task<Project> CreateProjectAsync(string name, string company = "DEOS AG", FxlApiV1.ProjectInfoLanguage language = FxlApiV1.ProjectInfoLanguage.DE, string programmer = null, bool utf8 = true)
        {
            var project = await GetProjectAsync(name);
            if (project != default)
                return project;

            if (string.IsNullOrWhiteSpace(programmer))
                programmer = Environment.UserName;

            var projectinfo = await client.CreateProjectAsync(new FxlApiV1.ProjectInfo
            {
                Name = name,
                Company = company,
                Language = language,
                Programmer = programmer,
                Utf8 = utf8
            });

            cache.Remove("projects");

            return (await GetProjectsAsync()).SingleOrDefault(p => p.Id.Equals(projectinfo.Id));
        }

        public async Task DeleteProject(Project project)
        {
            var retrys = 10;

            while (retrys-- > 0)
            {
                try
                {
                    await client.DeleteProjectAsync(project.Id);                    
                }
                catch (Exception)
                {
                    await Task.Delay(250);
                    continue;
                }
            }            
        }

        public async Task<FupFile> GetFupPageAsync(string projectname, string controllername, string fupFilename)
        {
            var mproject = await GetProjectAsync(projectname);
            var mcontroller = await mproject.GetControllerAsync(controllername);
            var mfupFile = await mcontroller.GetFupFileAsync(fupFilename);

            return mfupFile;
        }

        public async Task<Controller> GetControllerAsync(string projectname, string controllername)
        {
            var mproject = await GetProjectAsync(projectname);
            var mcontroller = await mproject.GetControllerAsync(controllername);

            return mcontroller;
        }

    }
}