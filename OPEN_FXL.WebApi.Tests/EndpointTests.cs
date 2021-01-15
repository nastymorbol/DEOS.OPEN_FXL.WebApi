using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Shouldly;
using Polly;
using System.Diagnostics;
using Xunit.Abstractions;

namespace FXL.WebApi.Tests
{
    public class EndpointTests : IClassFixture<DatabaseFixture>
    {

        DatabaseFixture fixture;
        private readonly ITestOutputHelper output;
        private readonly string projectname = "apiTst1";

        public EndpointTests(DatabaseFixture fixture, ITestOutputHelper output)
        {
            this.fixture = fixture;
            this.output = output;
        }

        [Fact(DisplayName = "OFXL-218 01_GetProjectsAsync")]
        public async Task OFXL_218_01_GetProjectsAsync()
        {
            var projects = await fixture.WebApiClient.GetProjectsAsync();
            projects.ShouldNotBeEmpty("Workspace ohne Projekte");
        }

        [Theory(DisplayName = "OFXL-218 02_CreateProjectAsync")]
        [InlineData("apiTst1", 409, "Wenn projekt schon vorhanden sollte status bekannt sein")]
        [InlineData("apiTst1", 409, "Wenn -projekt- schon vorhanden sollte status bekannt sein")]
        [InlineData("apiTst1123apiTst.1234", 400)]
        public async Task OFXL_218_02_CreateProjectAsync(string name, int expectedStatus, string info = null)
        {
            try
            {
                var project = await fixture.WebApiClient.CreateProjectAsync(
                        new FxlApiV1.ProjectInfo
                        {
                            Name = name,
                            Language = FxlApiV1.ProjectInfoLanguage.DE,
                            Company = "DEOS AG - Automated",
                            Programmer = Environment.UserName,
                            Utf8 = true
                        }
                    );

                project.ShouldNotBeNull();
                project.Name.ShouldBeEquivalentTo(name);
                project.Id.ShouldNotBeNullOrEmpty();
            }
            catch(FxlApiV1.ApiException apiException)
            {
                apiException.Message.ShouldNotContain("The HTTP status code of the response was not expected", Case.Insensitive , apiException.Message);
                apiException.StatusCode.ShouldBe(expectedStatus, apiException.Message);
            }
            catch (Exception)
            {

                throw;
            }
        }


        [Fact(DisplayName = "OFXL-218 05_GetControllersAsync")]
        public async Task OFXL_218_05_GetControllersAsync()
        {
            var project = await GetTestProject();

            var controllers = await fixture.WebApiClient.GetControllersAsync(project.Id);
            controllers.ShouldNotBeNull("Projekt");
        }

        [Theory(DisplayName = "OFXL-218 06_CreateControllerAsync")]
        [InlineData("apiTst1", 500, "Wenn projekt schon vorhanden sollte status bekannt sein")]
        [InlineData("apiTst1", 500, "Wenn -projekt- schon vorhanden sollte status bekannt sein")]
        [InlineData("apiTst1123apiTst.1234", 400)]
        public async Task OFXL_218_06_CreateControllerAsync(string name, int expectedStatus, string info = null)
        {
            var project = await GetTestProject();

            try
            {
                var controller = await fixture.WebApiClient.CreateControllerAsync(project.Id,
                    new FxlApiV1.ControllerInfo
                    {
                        Name = name,
                        Type = FxlApiV1.ControllerInfoType.OPEN_3100_EMS
                    });
                controller.ShouldNotBeNull();
                controller.Name.ShouldBeEquivalentTo(name);
                controller.Id.ShouldNotBeNullOrEmpty();
            }
            catch (FxlApiV1.ApiException apiException)
            {
                apiException.Message.ShouldNotContain("The HTTP status code of the response was not expected", Case.Insensitive, apiException.Message);
                apiException.StatusCode.ShouldBe(expectedStatus, apiException.Message);
            }
            catch (Exception)
            {

                throw;
            }

        }

        [Fact(DisplayName = "OFXL-218 08_DeleteControllerAsync")]
        public async Task OFXL_218_08_DeleteControllerAsync()
        {
            var controller = await GetTestController();

            try
            {
                await fixture.WebApiClient.DeleteControllerAsync(controller.Id);
                controller.ShouldNotBeNull();                
                controller.Id.ShouldNotBeNullOrEmpty();
            }
            catch (FxlApiV1.ApiException apiException)
            {
                apiException.Message.ShouldNotContain("The HTTP status code of the response was not expected", Case.Insensitive, apiException.Message);

                throw;
            }
            catch (Exception)
            {

                throw;
            }

            try
            {
                await fixture.WebApiClient.DeleteControllerAsync(controller.Id);
                controller.ShouldNotBeNull();
                controller.Id.ShouldNotBeNullOrEmpty();
            }
            catch (FxlApiV1.ApiException apiException)
            {
                apiException.Message.ShouldNotContain("The HTTP status code of the response was not expected", Case.Insensitive, apiException.Message);
                apiException.StatusCode.ShouldBe(404);
            }
            catch (Exception)
            {

                throw;
            }

        }


        [Theory(DisplayName = "OFXL-218 09_BuildControllerAsync")]
        [InlineData("apiTst1")]
        public async Task OFXL_218_10_BuildControllerAsync(string name)
        {
            try
            {
                var controller = await GetTestController();

                var process = await fixture.WebApiClient.BuildControllerProgramAsync(controller.Id, new FxlApiV1.BuildParameters
                {
                    Debug = false,
                    OnlyObj = false
                });

                /*
                    Code    Description
                    202	    Process is still running
                    302	    Process has finished
                    404	    Process with that Id was not found.
                 */
                var sw = Stopwatch.StartNew();

                var response = await Policy
                    .Handle<FxlApiV1.ApiException>(e =>
                    {
                        output.WriteLine($"PID request exception == {e.StatusCode} [{sw.Elapsed.Seconds} s]");
                        return e.StatusCode != 302;
                    })
                    .OrResult<FxlApiV1.SwaggerResponse>((r) =>
                    {
                        output.WriteLine($"PID request result   == {r.StatusCode} [{sw.Elapsed.Seconds} s]");
                        return r.StatusCode != 302;
                    })
                    .WaitAndRetryAsync(12, r => TimeSpan.FromSeconds(10))
                    .ExecuteAsync(() => 
                    {
                        output.WriteLine($"Retry PID request [{sw.Elapsed.Seconds} s]");
                        return fixture.WebApiClient.GetProcessStatusAsync(process.Id); 
                    });                                                       
            }
            catch (FxlApiV1.ApiException apiException)
            {
                apiException.Message.ShouldNotContain("The HTTP status code of the response was not expected", Case.Insensitive, apiException.Message);
                apiException.StatusCode.ShouldBe(302);
                //apiException.StatusCode.ShouldBe(expectedStatus, apiException.Message);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [Theory(DisplayName = "OFXL-218 10_CleanControllerProgramAsync")]
        [InlineData("apiTst1")]
        public async Task OFXL_218_10_CleanControllerProgramAsync(string name)
        {
            var controller = await GetTestController();

            try
            {

                var process = await fixture.WebApiClient.BuildControllerProgramAsync(controller.Id, new FxlApiV1.BuildParameters
                {
                    Debug = false,
                    OnlyObj = true
                });

                /*
                    Code    Description
                    202	    Process is still running
                    302	    Process has finished
                    404	    Process with that Id was not found.
                 */
                var sw = Stopwatch.StartNew();

                var response = await Policy
                    .Handle<FxlApiV1.ApiException>(e =>
                    {
                        output.WriteLine($"PID request exception == {e.StatusCode} [{sw.Elapsed.Seconds} s]");
                        return e.StatusCode != 302;
                    })
                    .OrResult<FxlApiV1.SwaggerResponse>((r) =>
                    {
                        output.WriteLine($"PID request result   == {r.StatusCode} [{sw.Elapsed.Seconds} s]");
                        return r.StatusCode != 302;
                    })
                    .WaitAndRetryAsync(12, r => TimeSpan.FromSeconds(10))
                    .ExecuteAsync(() =>
                    {
                        output.WriteLine($"Retry PID request [{sw.Elapsed.Seconds} s]");
                        return fixture.WebApiClient.GetProcessStatusAsync(process.Id);
                    });
                            
            }
            catch (FxlApiV1.ApiException apiException)
            {
                apiException.Message.ShouldNotContain("The HTTP status code of the response was not expected", Case.Insensitive, apiException.Message);
                apiException.StatusCode.ShouldBe(302);
                //apiException.StatusCode.ShouldBe(expectedStatus, apiException.Message);

                await fixture.WebApiClient.CleanControllerProgramAsync(controller.Id);

            }
            catch (Exception)
            {

                throw;
            }
        }

        [Theory(DisplayName = "OFXL-218 11_AddLogEntryAsync")]
        [InlineData("apiTst1")]
        public async Task OFXL_218_11_AddLogEntryAsync(string name)
        {
            var controller = await GetTestController();

            try
            {

                /*
                    Code    Description
                    200	    Log entry successfully added
                    404	    Controller with that Id was not found.
                 */
                var sw = Stopwatch.StartNew();

                await Policy
                    .Handle<FxlApiV1.ApiException>(e =>
                    {
                        output.WriteLine($"PID request exception == {e.StatusCode} [{sw.Elapsed.Seconds} s]");
                        return e.StatusCode != 404;
                    })
                    .OrResult<FxlApiV1.SwaggerResponse>(r =>
                    {
                        output.WriteLine($"PID request result   == {r.StatusCode} [{sw.Elapsed.Seconds} s]");
                        return r.StatusCode != 200;
                    })
                    .WaitAndRetryAsync(3, r => TimeSpan.FromSeconds(1))
                    .ExecuteAsync(() => 
                    {
                        return fixture.WebApiClient.AddLogEntryAsync(controller.Id, new FxlApiV1.LogEntry
                        {
                            Function = "My Function",
                            Info = "My Info",
                            Section = "My Section"
                        });
                    });

            }
            catch (FxlApiV1.ApiException apiException)
            {
                apiException.Message.ShouldNotContain("The HTTP status code of the response was not expected", Case.Insensitive, apiException.Message);
                apiException.StatusCode.ShouldBe(302);
                //apiException.StatusCode.ShouldBe(expectedStatus, apiException.Message);

                await fixture.WebApiClient.CleanControllerProgramAsync(controller.Id);

            }
            catch (Exception)
            {

                throw;
            }
        }

        [Theory(DisplayName = "OFXL-218 12_GetFupPagesAsync")]
        [InlineData("apiTst1")]
        public async Task OFXL_218_12_GetFupPagesAsync(string name)
        {
            var controller = await GetTestController();

            try
            {

                /*
                    Code    Description
                    200	    Log entry successfully added
                    404	    Controller with that Id was not found.
                 */
                var sw = Stopwatch.StartNew();

                var result = await Policy
                    .Handle<FxlApiV1.ApiException>(e =>
                    {
                        output.WriteLine($"Request exception == {e.StatusCode} [{sw.Elapsed.Seconds} s]");
                        return e.StatusCode != 404;
                    })
                    .OrResult<ICollection<FxlApiV1.FupPage>>(r =>
                    {
                        output.WriteLine($"Request result   == {r.Count} FUP-Pages [{sw.Elapsed.Seconds} s]");
                        return r == default || r.Count == 0;
                    })
                    .WaitAndRetryAsync(3, r => TimeSpan.FromSeconds(1))
                    .ExecuteAsync(() =>
                    {
                        return fixture.WebApiClient.GetFupPagesAsync(controller.Id);
                    });

                result.Count.ShouldBeGreaterThanOrEqualTo(2);
                result.ShouldContain(f => f.Name.Equals("zeit.f", StringComparison.InvariantCultureIgnoreCase));
                result.ShouldContain(f => f.Name.Equals("konst.f", StringComparison.InvariantCultureIgnoreCase));
            }
            catch (FxlApiV1.ApiException apiException)
            {
                apiException.Message.ShouldNotContain("The HTTP status code of the response was not expected", Case.Insensitive, apiException.Message);
                apiException.StatusCode.ShouldBe(302);
                //apiException.StatusCode.ShouldBe(expectedStatus, apiException.Message);

                await fixture.WebApiClient.CleanControllerProgramAsync(controller.Id);

            }
            catch (Exception)
            {

                throw;
            }
        }

        [Theory(DisplayName = "OFXL-218 13_InsertFupPageAsync")]
        [InlineData("apiTst1")]
        public async Task OFXL_218_13_InsertFupPageAsync(string name)
        {
            var controller = await GetTestController();

            var source = await fixture.Db.GetFupPageAsync("!MAKBIB.WIN", "V0005", "leer.f");

            source.ShouldNotBeNull();

            try
            {

                /*
                    Code    Description
                    200	    Log entry successfully added
                    404	    Controller with that Id was not found.
                 */
                var sw = Stopwatch.StartNew();

                var result = await Policy
                    .Handle<FxlApiV1.ApiException>(e =>
                    {
                        output.WriteLine($"Request exception == {e.StatusCode} [{sw.Elapsed.Seconds} s]");
                        return e.StatusCode != 404;
                    })
                    .OrResult<FxlApiV1.FupPageInfo>(r =>
                    {
                        output.WriteLine($"Request result   == {r.Name} FUP-Pages [{sw.Elapsed.Seconds} s]");
                        return r == default;
                    })
                    .WaitAndRetryAsync(3, r => TimeSpan.FromSeconds(1))
                    .ExecuteAsync(() =>
                    {
                        return fixture.WebApiClient.InsertFupPageAsync(controller.Id, new FxlApiV1.FupPageInfo { 
                            SourceId = source.Id,
                            Name = "_leer.f"
                        });
                    });

                result.Name.ShouldBeEquivalentTo("_leer.f");
                result.Macro.ShouldBeTrue();
                result.MacroStatus.ShouldBeTrue();

                var result2 = await Policy
                    .Handle<FxlApiV1.ApiException>(e =>
                    {
                        output.WriteLine($"Request exception == {e.StatusCode} [{sw.Elapsed.Seconds} s]");
                        return e.StatusCode != 404;
                    })
                    .OrResult<ICollection<FxlApiV1.FupPage>>(r =>
                    {
                        output.WriteLine($"Request result   == {r.Count} FUP-Pages [{sw.Elapsed.Seconds} s]");
                        return r == default || r.Count == 0;
                    })
                    .WaitAndRetryAsync(3, r => TimeSpan.FromSeconds(1))
                    .ExecuteAsync(() =>
                    {
                        return fixture.WebApiClient.GetFupPagesAsync(controller.Id);
                    });

                result2.Count.ShouldBe(3);
                result2.ShouldContain(f => f.Name.Equals("zeit.f", StringComparison.InvariantCultureIgnoreCase));
                result2.ShouldContain(f => f.Name.Equals("konst.f", StringComparison.InvariantCultureIgnoreCase));
                result2.ShouldContain(f => f.Name.Equals("_leer.f", StringComparison.InvariantCultureIgnoreCase));

            }
            catch (FxlApiV1.ApiException apiException)
            {
                apiException.Message.ShouldNotContain("The HTTP status code of the response was not expected", Case.Insensitive, apiException.Message);
                apiException.StatusCode.ShouldBe(302);
                //apiException.StatusCode.ShouldBe(expectedStatus, apiException.Message);

                await fixture.WebApiClient.CleanControllerProgramAsync(controller.Id);

            }
            catch (Exception)
            {

                throw;
            }
        }

        [Theory(DisplayName = "OFXL-218 14_GetFupPageInfoAsync")]
        [InlineData("apiTst1")]
        public async Task OFXL_218_14_GetFupPageInfoAsync(string name)
        {
            var controller = await GetTestController();

            var fupPages = await fixture.WebApiClient.GetFupPagesAsync(controller.Id);

            foreach (var fupPage in fupPages)
            {
                var fupPageInfo = await fixture.WebApiClient.GetFupPageInfoAsync(fupPage.Id);
                fupPageInfo.Name.ShouldNotBeNullOrWhiteSpace();
                fupPageInfo.Id.ShouldNotBeNullOrWhiteSpace();
            }            
        }

        [Theory(DisplayName = "OFXL-218 15_SetFupPageInfoAsync")]
        [InlineData("apiTst1")]
        public async Task OFXL_218_15_SetFupPageInfoAsync(string name)
        {
            var controller = await GetTestController();

            var fupPages = await fixture.WebApiClient.GetFupPagesAsync(controller.Id);

            foreach (var fupPage in fupPages)
            {
                var fupPageInfo = await fixture.WebApiClient.SetFupPageInfoAsync(fupPage.Id, new FxlApiV1.FupPageInfo
                {
                    MacroStatus = true
                });
                fupPageInfo.Name.ShouldNotBeNullOrWhiteSpace();
                fupPageInfo.Id.ShouldNotBeNullOrWhiteSpace();
                fupPageInfo.MacroStatus.ShouldBeTrue();
            }
        }

        [Theory(DisplayName = "OFXL-218 16_DeleteFupPageAsync")]
        [InlineData("apiTst1")]
        public async Task OFXL_218_16_DeleteFupPageAsync(string name)
        {
            var controller = await GetTestController();

            var fupPages = await fixture.WebApiClient.GetFupPagesAsync(controller.Id);

            foreach (var fupPage in fupPages)
            {
                await fixture.WebApiClient.DeleteFupPageAsync(fupPage.Id);
            }
        }


        [Theory(DisplayName = "OFXL-218 17_UpdateMacroAsync")]
        [InlineData("apiTst1")]
        public async Task OFXL_218_17_UpdateMacroAsync(string name)
        {
            var controller = await GetTestController();

            var source = await fixture.Db.GetFupPageAsync("!MAKBIB.WIN", "V0005", "leer.f");

            source.ShouldNotBeNull();

            try
            {

                /*
                    Code    Description
                    200	    Log entry successfully added
                    404	    Controller with that Id was not found.
                 */
                var sw = Stopwatch.StartNew();

                var result = await Policy
                    .Handle<FxlApiV1.ApiException>(e =>
                    {
                        output.WriteLine($"Request exception == {e.StatusCode} [{sw.Elapsed.Seconds} s]");
                        return e.StatusCode != 404;
                    })
                    .OrResult<FxlApiV1.FupPageInfo>(r =>
                    {
                        output.WriteLine($"Request result   == {r.Name} FUP-Pages [{sw.Elapsed.Seconds} s]");
                        return r == default;
                    })
                    .WaitAndRetryAsync(3, r => TimeSpan.FromSeconds(1))
                    .ExecuteAsync(() =>
                    {
                        return fixture.WebApiClient.InsertFupPageAsync(controller.Id, new FxlApiV1.FupPageInfo
                        {
                            SourceId = source.Id,
                            Name = "_leer.f"
                        });
                    });

                result.Name.ShouldBeEquivalentTo("_leer.f");
                result.Macro.ShouldBeTrue();
                result.MacroStatus.ShouldBeTrue();

                await fixture.WebApiClient.UpdateMacroAsync(result.Id);
            }
            catch (FxlApiV1.ApiException apiException)
            {
                apiException.Message.ShouldNotContain("The HTTP status code of the response was not expected", Case.Insensitive, apiException.Message);
                apiException.StatusCode.ShouldBe(200);
                //apiException.StatusCode.ShouldBe(expectedStatus, apiException.Message);

                await fixture.WebApiClient.CleanControllerProgramAsync(controller.Id);

            }
            catch (Exception)
            {

                throw;
            }
        }

        [Theory(DisplayName = "OFXL-218 18_GetDefinitionsAsync")]
        [InlineData("apiTst1")]
        public async Task OFXL_218_18_GetDefinitionsAsync(string name)
        {
            var controller = await GetTestController();

            var source = await fixture.Db.GetFupPageAsync("!MAKBIB.WIN", "V0005", "leer.f");

            source.ShouldNotBeNull();

            try
            {

                /*
                    Code    Description
                    200	    Log entry successfully added
                    404	    Controller with that Id was not found.
                 */
                var sw = Stopwatch.StartNew();

                var result = await Policy
                    .Handle<FxlApiV1.ApiException>(e =>
                    {
                        output.WriteLine($"Request exception == {e.StatusCode} [{sw.Elapsed.Seconds} s]");
                        return e.StatusCode != 404;
                    })
                    .OrResult<FxlApiV1.FupPageInfo>(r =>
                    {
                        output.WriteLine($"Request result   == {r.Name} FUP-Pages [{sw.Elapsed.Seconds} s]");
                        return r == default;
                    })
                    .WaitAndRetryAsync(3, r => TimeSpan.FromSeconds(1))
                    .ExecuteAsync(() =>
                    {
                        return fixture.WebApiClient.InsertFupPageAsync(controller.Id, new FxlApiV1.FupPageInfo
                        {
                            SourceId = source.Id,
                            Name = "_leer.f"
                        });
                    });

                result.Name.ShouldBeEquivalentTo("_leer.f");
                result.Macro.ShouldBeTrue();
                result.MacroStatus.ShouldBeTrue();

                var definitions = await fixture.WebApiClient.GetDefinitionsAsync(result.Id);

                definitions.Count.ShouldBe(5);
                definitions.ShouldContain(d => d.Key == "def_k" && d.Value == "" && d.Comment == "Kunde" && d.Hint == "");
                definitions.ShouldContain(d => d.Key == "def_o" && d.Value == "" && d.Comment == "Anlagenbezeichnung" && d.Hint == "Menüstruktur im FUP und Bezeichnung der Dokumentation/Einstellparameter");
            }
            catch (FxlApiV1.ApiException apiException)
            {
                apiException.Message.ShouldNotContain("The HTTP status code of the response was not expected", Case.Insensitive, apiException.Message);
                apiException.StatusCode.ShouldBe(200);
                //apiException.StatusCode.ShouldBe(expectedStatus, apiException.Message);

                await fixture.WebApiClient.CleanControllerProgramAsync(controller.Id);

            }
            catch (Exception)
            {

                throw;
            }
        }

        [Theory(DisplayName = "OFXL-218 19_SetDefinitionsAsync")]
        [InlineData("apiTst1")]
        public async Task OFXL_218_19_SetDefinitionsAsync(string name)
        {
            var controller = await GetTestController();

            var source = await fixture.Db.GetFupPageAsync("!MAKBIB.WIN", "V0005", "leer.f");

            source.ShouldNotBeNull();

            try
            {

                /*
                    Code    Description
                    200	    Log entry successfully added
                    404	    Controller with that Id was not found.
                 */
                var sw = Stopwatch.StartNew();

                var result = await Policy
                    .Handle<FxlApiV1.ApiException>(e =>
                    {
                        output.WriteLine($"Request exception == {e.StatusCode} [{sw.Elapsed.Seconds} s]");
                        return e.StatusCode != 404;
                    })
                    .OrResult<FxlApiV1.FupPageInfo>(r =>
                    {
                        output.WriteLine($"Request result   == {r.Name} FUP-Pages [{sw.Elapsed.Seconds} s]");
                        return r == default;
                    })
                    .WaitAndRetryAsync(3, r => TimeSpan.FromSeconds(1))
                    .ExecuteAsync(() =>
                    {
                        return fixture.WebApiClient.InsertFupPageAsync(controller.Id, new FxlApiV1.FupPageInfo
                        {
                            SourceId = source.Id,
                            Name = "_leer.f"
                        });
                    });

                result.Name.ShouldBeEquivalentTo("_leer.f");
                result.Macro.ShouldBeTrue();
                result.MacroStatus.ShouldBeTrue();

                var definitions = await fixture.WebApiClient.GetDefinitionsAsync(result.Id);

                var def_f = definitions.Single(d => d.Key == "def_f");
                var def_f_value = $"Ein neuer def_f {DateTimeOffset.Now}";

                await fixture.WebApiClient.SetDefinitionsAsync(result.Id, new[]
                {
                    new FxlApiV1.Definition
                    {
                        Key = "def_f",
                        Value = def_f_value
                    }
                });


                definitions = await fixture.WebApiClient.GetDefinitionsAsync(result.Id);

                def_f = definitions.Single(d => d.Key == "def_f");

                def_f.Value.ShouldBeEquivalentTo(def_f_value);
            }
            catch (FxlApiV1.ApiException apiException)
            {
                apiException.Message.ShouldNotContain("The HTTP status code of the response was not expected", Case.Insensitive, apiException.Message);
                apiException.StatusCode.ShouldBe(200);
                //apiException.StatusCode.ShouldBe(expectedStatus, apiException.Message);

                await fixture.WebApiClient.CleanControllerProgramAsync(controller.Id);

            }
            catch (Exception)
            {

                throw;
            }
        }

        #region Helper Methods

        private async Task<FxlApiV1.Project> GetTestProject()
        {
            try
            {
                _ = await fixture.WebApiClient.CreateProjectAsync(
                        new FxlApiV1.ProjectInfo
                        {
                            Name = projectname,
                            Language = FxlApiV1.ProjectInfoLanguage.DE,
                            Company = "DEOS AG - Automated",
                            Programmer = Environment.UserName,
                            Utf8 = true
                        }
                    );

            }
            catch (Exception)
            {
            }
            var project = (await fixture.WebApiClient.GetProjectsAsync()).Single(p => p.Name == projectname);
            project.ShouldNotBeNull();
            return project;
        }

        private async Task<FxlApiV1.Controller> GetTestController()
        {
            try
            {
                _ = await fixture.WebApiClient.CreateProjectAsync(
                        new FxlApiV1.ProjectInfo
                        {
                            Name = projectname,
                            Language = FxlApiV1.ProjectInfoLanguage.DE,
                            Company = "DEOS AG - Automated",
                            Programmer = Environment.UserName,
                            Utf8 = true
                        }
                    );

            }
            catch (Exception)
            {
            }
            var project = (await fixture.WebApiClient.GetProjectsAsync()).Single(p => p.Name == projectname);
            project.ShouldNotBeNull();

            try
            {
                _ = await fixture.WebApiClient.CreateControllerAsync(
                    project.Id,
                        new FxlApiV1.ControllerInfo
                        {
                            Name = projectname,
                            Type = FxlApiV1.ControllerInfoType.OPEN_600_EMS
                        }
                    );

            }
            catch (Exception)
            {
            }

            var controller = (await fixture.WebApiClient.GetControllersAsync(project.Id)).Single(p => p.Name == projectname);
            controller.ShouldNotBeNull();

            return controller;
        }

        #endregion
    }
}
