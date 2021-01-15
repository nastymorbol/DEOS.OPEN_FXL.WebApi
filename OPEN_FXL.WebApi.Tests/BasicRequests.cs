using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Shouldly;

namespace FXL.WebApi.Tests
{
    public class BasicRequests : IClassFixture<DatabaseFixture>
    {

        DatabaseFixture fixture;

        private readonly string projectname = "wS001";

        public BasicRequests(DatabaseFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public async Task GetProjectsTest()
        {
            var project = (await fixture.Db.GetProjectsAsync()).Single(p => p.Name.Equals(projectname, StringComparison.InvariantCultureIgnoreCase));
            project.ShouldNotBe(null);

            var project2 = await fixture.Db.GetProjectAsync(projectname);

            project2.ShouldBeEquivalentTo(project);
        }

        [Fact]
        public async Task GetControllerTest()
        {
            var project = await fixture.Db.GetProjectAsync(projectname);

            var controllers = await project.GetControllerAsync();

            foreach (var controller in controllers)
            {
                controller.Name.ShouldNotBeEmpty();
                controller.Ip.ShouldNotBeEmpty();
            }
        }

        [Fact]
        public async Task CreateProject()
        {
            var project = await fixture.Db.CreateProjectAsync("apiDemo1");

            await fixture.Db.DeleteProject(project);
        }

        [Fact]
        public async Task CreateProjectController()
        {
            var project = await fixture.Db.CreateProjectAsync("apiDemo1");

            var controller = await project.CreateControllerAsync("apiCtr1", FxlApiV1.ControllerInfoType.OPEN_810_EMS);

            //await controller.BuildAsync();


            await fixture.Db.DeleteProject(project);
        }

        [Fact]
        public async Task CreateProjectControllerFupFileMacro()
        {
            var project = await fixture.Db.CreateProjectAsync("apiDemo1");

            var controller = await project.CreateControllerAsync("apiCtr1", FxlApiV1.ControllerInfoType.OPEN_810_EMS);

            var sourceFupFile = await fixture.Db.GetFupPageAsync("!MAKBIB.WIN", "V1005", "INAI8AO4.F$X");

            for (int i = 0; i < 5; i++)
            {
                var fupFile = await controller.CopyMacroAsync(sourceFupFile.Id, $"kiki{i}.f");
            }

            for (int i = 0; i < 5; i++)
            {
                var fupFile = await controller.GetFupFileAsync($"kiki{i}.f");

                await controller.DeleteFupFile(fupFile);
            }

            //await controller.BuildAsync();


            await fixture.Db.DeleteProject(project);
        }

        [Fact]
        public async Task SetDefinitions()
        {
            var project = await fixture.Db.CreateProjectAsync("apiDemo1");

            var controller = await project.CreateControllerAsync("apiCtr1", FxlApiV1.ControllerInfoType.OPEN_810_EMS);

            var sourceFupFile = await fixture.Db.GetFupPageAsync("!MAKBIB.WIN", "V1005", "INAI8AO4.F$X");

            for (int i = 0; i < 2; i++)
            {
                var fupFile = await controller.CopyMacroAsync(sourceFupFile.Id, $"kiki{i}.f");

                var definitions = (await fupFile.GetDefinitionsAsync()).ToArray();

                for (int j = 0; j < definitions.Count(); j++)
                {
                    definitions[j].Kommentar = "Ich xxx mache alles kaputt !!!";
                    definitions[j].Hinweis = " ----  xxx --- Ich mache alles kaputt !!!";
                }
                fupFile.SetDefinitions(definitions);
                fupFile.SetDefinition("def_o", "My Pages");
            }


            await fixture.Db.DeleteProject(project);
        }

        [Fact]
        public async Task GetAllIos_OPEN_FXL_WebApi()
        {
            var ios = new List<Fup.FupIo>();
            var project = await fixture.Db.GetProjectAsync("402080");

            foreach (var controller in await project.GetControllerAsync())
            {
                var cios = await controller.GetIosAsync();
                ios.AddRange(cios);
            }

            ios.Count.ShouldBe(411);
        }

        [Fact]
        public void GetAllIos_Dotnet_WebApi()
        {
            var ios = new List<Fup.FupIo>();
            var project = new Fup.FupFile(@"E:\Projekte\fxl3\Entwicklung\Tests\prj\402080");
            foreach (var path in project.FupFileInformation.UstPaths)
            {
                using (var controller = new Fup.FupFile(path))
                {
                    ios.AddRange(controller.IOs.Where(io => io.IoExists));
                }
            }

            ios.Count.ShouldBe(411);
        }

        [Fact]
        public async Task GetAllDefinitionComments()
        {
            var sourceController = await fixture.Db.GetControllerAsync("!M_TEMP", "HEIZUNG");

            var texts = new Dictionary<int, string>();

            await foreach(var fupFile in sourceController.GetFupFilesAsync())
            {
                var defs = await fupFile.GetDefinitionsAsync();
                foreach (var def in defs)
                {
                    var hash = def.Kommentar.GetHashCode();
                    if (texts.ContainsKey(hash))
                        continue;
                    texts[hash] = def.Kommentar;
                }
            }

            texts.Count.ShouldBe(636);
        }

        [Fact]
        public void GetAllDefinitionCommentsFileBased()
        {
            var sourceController = new Fup.FupFile(@"E:\Projekte\fxl3\Entwicklung\Tests\prj\!M_TEMP\HEIZUNG");

            var texts = new Dictionary<int, string>();

            foreach (var fupFilePath in sourceController.FupFileInformation.FupFilesInProjectUstPath)
            {
                using (var fupFile = new Fup.FupFile(fupFilePath))
                {                    
                    var defs = fupFile.Definitions;
                    foreach (var def in defs)
                    {
                        var hash = def.Kommentar?.GetHashCode();
                        if (!hash.HasValue || texts.ContainsKey(hash.Value))
                            continue;
                        texts[hash.Value] = def.Kommentar;
                    }
                }
            }

            texts.Count.ShouldBe(636);
        }
    }
}
