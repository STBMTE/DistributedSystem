using ConsumerLinks.Models;

namespace ConsumerLinks.Http
{
    public interface IHttpService
    {
        Task<int> Send(string url);
        Task UpdateLink(StatusUpdate statusUpdate);
    }
}
