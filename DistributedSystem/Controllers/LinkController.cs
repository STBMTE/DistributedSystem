using ConsumerLinks.Models;
using DistributedSystem.Model;
using DistributedSystem.RabbitMQ;
using DistributedSystem.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace DistributedSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LinkController : ControllerBase
    {
        private readonly LinkRepository _linkRepository;
        private readonly IRabbitMqService _rabbitMqService;

        public LinkController(LinkRepository linkRepository, IRabbitMqService rabbitMqService)
        {
            _linkRepository = linkRepository;
            _rabbitMqService = rabbitMqService;
        }

        [HttpGet]
        [Route("all")]
        public async Task<IReadOnlyCollection<Link>> getLinks()
        {
            return await _linkRepository.GetLinks();
        }

        [HttpGet]
        public async Task<Link?> getLink(int id)
        {
            return await _linkRepository.GetLink(id);
        }

        [HttpPost]
        public async Task<Link?> SaveLink(string link)
        {
            Link createdlink = await _linkRepository.AddLink(new Link(link));
            _rabbitMqService.SendMessage(createdlink);
            return createdlink;
        }

        [HttpPatch]
        public async Task<Link?> UpdateLinkStatus([FromBody] UpdateStatusCode updateStatus)
        {
            Link oldLink = await _linkRepository.GetLink(updateStatus.id);
            oldLink.status = updateStatus.status;
            oldLink.statusCode = updateStatus.statusCode;
            return await _linkRepository.UpdateLink(oldLink);
        }
    }
}
