namespace Client1.Services
{
    public interface IHttpClientService
    {
        Task<HttpClient> GetHttpClientAsync();
    }
}
