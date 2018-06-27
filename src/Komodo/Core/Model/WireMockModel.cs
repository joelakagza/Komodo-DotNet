namespace Komodo.Core.Model
{
    public class Request
    {
        public string method { get; set; }
        public string url { get; set; }
        public string urlPath { get; set; }
        public string urlPattern { get; set; }
        
    }

    public class Headers
    {
        public string ContentType { get; set; }
        public string CacheControl { get; set; }
    }

    public class Response
    {
        public int status { get; set; }
        public string statusMessage { get; set; }
        public Headers headers { get; set; }
        public string body { get; set; }
        public string proxyBaseUrl { get; set; }
        public string bodyFileName { get; set; }
    }

    public class WireMockModel
    {
        public WireMockModel()
        {
            request = new Request();
            response = new Response();
        }

        public string priority { get; set; }
        public Request request { get; set; }
        public Response response { get; set; }
        
    }
}