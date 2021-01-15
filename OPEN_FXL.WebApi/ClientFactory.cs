using System;

namespace FXL.WebApi
{
    public class ClientFactory
    {

        public static FxlClient GetClient(string host = "127.0.0.1", int port = 8080)
        {
            var client = new FxlApiV1.Client(new System.Net.Http.HttpClient());
            client.BaseUrl = $"http://{host}:{port}/api/v1"; ;
            return new FxlClient(client);
        }

    }
}
