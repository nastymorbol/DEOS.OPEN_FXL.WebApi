using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Demo
{
    class Program
    {

        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello FXL!");

            var client = FXL.WebApi.ClientFactory.GetClient();

            var projects = await client.GetProjectsAsync();

            foreach (var project in projects)
            {
                var controllers = await project.GetControllerAsync();

                foreach (var controller in controllers)
                {
                    var fupFiles = controller.GetFupFilesAsync();

                    await foreach(var fupFile in fupFiles)
                    {
                        var definitions = await fupFile.GetDefinitionsAsync();
                        Print(project, controller, fupFile, definitions);
                    }
                }
            }

        }

        private static void Print(FXL.WebApi.Project project, FXL.WebApi.Controller controller, FXL.WebApi.FupFile fupFile, IEnumerable<Fup.Definitionfile.Definition> definitions)
        {
            Console.WriteLine($"{project?.Name} ({controller?.Name} {controller?.Ip}) {fupFile?.Name} -> {fupFile?.Source} | {definitions?.Count()}");
        }

        //static async Task Main2(string[] args)
        //{
        //    Console.WriteLine("Hello FXL!");

        //    var client = FXL.WebApi.ClientFactory.GetClient();

        //    for (int i = 0; i < 100; i++)
        //    {
        //        var projects = await client.GetProjectsAsync();

        //        foreach (var project in projects)
        //        {
        //            var controllers = await client.GetControllersAsync(project);
        //            foreach (var controller in controllers)
        //            {
        //                var controllerInfo = await client.GetControllerInfoAsync(controller);

        //                var fupPages = await client.GetFupPagesAsync(controller);

        //                foreach (var fupPage in fupPages)
        //                {
        //                    var fupPageInfo = await client.GetFupPageInfoAsync(fupPage);

        //                    var definitions = await client.GetDefinitionsAsync(fupPage);
        //                    foreach (var definition in definitions)
        //                    {
        //                        Print(controllerInfo, fupPageInfo, definition);
        //                    }
        //                    //Print(controllerInfo, fupPageInfo);
        //                }

        //            }
        //        }
        //    }
        //}

        private static void Print(FxlApiV1.ControllerInfo controllerInfo, FxlApiV1.FupPageInfo fupPageInfo, FxlApiV1.Definition definition)
        {
            Console.WriteLine($"{controllerInfo?.Name} ({controllerInfo?.Ip}) {fupPageInfo?.Name} -> {fupPageInfo?.Source} | {definition?.Key} {definition?.Value.Trim()}");
        }
    }
}
