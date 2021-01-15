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
        /// <summary>
        /// Connect to OPEN FXL WebServer
        /// </summary>
        /// <param name="client"></param>
        public FxlClient(FxlApiV1.Client client) : base(client) { }

        /// <summary>
        /// Get Projects in the current active Workspace
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<Project>> GetProjectsAsync() 
            => GetProjectsRequestAsync();

        /// <summary>
        /// Get Projects in the current active Workspace (async)
        /// </summary>
        /// <returns></returns>
        public async Task<Project> GetProjectAsync(string projectname)
            => (await GetProjectsAsync()).SingleOrDefault(p => p.Name.Equals(projectname, StringComparison.InvariantCultureIgnoreCase));

        /// <summary>
        /// Create a new Project
        /// </summary>
        /// <param name="name">The Projectname</param>
        /// <param name="company">The Company</param>
        /// <param name="language">The Language</param>
        /// <param name="programmer">The Programmer</param>
        /// <param name="utf8">The used Encoding</param>
        /// <returns></returns>
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

        /// <summary>
        /// Delete the given Project
        /// </summary>
        /// <param name="project"></param>
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

        /// <summary>
        /// Returns an Controller within the given ProjectName
        /// </summary>
        /// <param name="projectname">The project name</param>
        /// <param name="controllername">The controller name</param>
        /// <returns></returns>
        public async Task<Controller> GetControllerAsync(string projectname, string controllername)
        {
            var mproject = await GetProjectAsync(projectname);
            var mcontroller = await mproject.GetControllerAsync(controllername);

            return mcontroller;
        }

        /// <summary>
        /// Returns an singe Fup Page Item
        /// </summary>
        /// <param name="projectname">The project name</param>
        /// <param name="controllername">The controller name</param>
        /// <param name="fupFilename">FUP-File Name (const.f)</param>
        /// <returns></returns>
        public async Task<FupFile> GetFupPageAsync(string projectname, string controllername, string fupFilename)
        {
            var mproject = await GetProjectAsync(projectname);
            var mcontroller = await mproject.GetControllerAsync(controllername);
            var mfupFile = await mcontroller.GetFupFileAsync(fupFilename);

            return mfupFile;
        }


        private Task<IEnumerable<Project>> GetProjectsRequestAsync()
            => GetFromCache("projects", (x) => x.Select(y => new Project(client, y)), client.GetProjectsAsync());

    }
}