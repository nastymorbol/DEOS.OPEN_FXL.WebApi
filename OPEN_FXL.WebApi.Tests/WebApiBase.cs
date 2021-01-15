using FxlApiV1;
using System;
using System.Linq;
using Xunit;

namespace FXL.WebApi.Tests
{
    public class DatabaseFixture : IDisposable
    {
        public DatabaseFixture()
        {
            Db = ClientFactory.GetClient();
            WebApiClient = Db.ApiV1Client;

            // ... initialize data in the test database ...
            var projects = Db.GetProjectsAsync().ConfigureAwait(false)
                .GetAwaiter().GetResult().Where(p => p.Name.StartsWith("apiTst"));

            foreach (var project in projects)
            {
                Db.DeleteProject(project).ConfigureAwait(false)
                .GetAwaiter().GetResult();
            }
        }

        public void Dispose()
        {
            // ... clean up test data from the database ...
            var projects = Db.GetProjectsAsync().ConfigureAwait(false)
                .GetAwaiter().GetResult().Where(p => p.Name.StartsWith("apiTst"));

            foreach (var project in projects)
            {
                Db.DeleteProject(project).ConfigureAwait(false)
                .GetAwaiter().GetResult();
            }
                
        }

        public FxlClient Db { get; private set; }
        public Client WebApiClient { get; }
    }

}
