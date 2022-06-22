using BiddingAPI.Models;
using BiddingAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace BiddingAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AuctionsController : ControllerBase
{
    private readonly ILogger<AuctionsController> _logger;
    private dynamic testData = new {
        Auctions = new[]{
            new {Name = "Fruits", Id = "1", Category = "Fruits and Vegetables", Lots = new []{
                new { Name = "Apples", Id = "1", Price = 1.1 },
                new { Name = "Bananas", Id = "2", Price = 1.5 },
                new { Name = "Oranges", Id = "3", Price = 2.3 },
                new { Name = "Grapefruits", Id = "4", Price = 5.6 }
            }},
            new {Name = "Antique", Id = "2", Category = "Antique", Lots = new []{
                new { Name = "Table", Id = "1", Price = 2.1 },
                new { Name = "Chair", Id = "2", Price = 2.4 },
                new { Name = "Mirror", Id = "3", Price = 3.1 },
                new { Name = "Closet", Id = "4", Price = 6.6 }
            }}
        }
    };

    public AuctionsController(ILogger<AuctionsController> logger, IBidsHandler bidsHandler)
    {
        _logger = logger;
    }

    [HttpGet(Name = "Get Auctions")]
    public async Task<IEnumerable<Auction>> Get()
    {
        
        return new Auction[]{
            new Auction(Name: "Fruits", Id: "1", Category: "Fruits and Vegetables"),
            new Auction(Name: "Antique", Id: "2", Category: "Antique"),
            new Auction(Name: "Electronics", Id: "3", Category: "Electronics"),
            new Auction(Name: "Paintings", Id: "4", Category: "Arts and Crafts")
        };
    }

    [HttpGet("{auctionId}/lots")]
    public async Task<IEnumerable<Lot>> GetLots(string auctionId)
    {
        return new Lot[]{
            new Lot(Name: "Fruits", Id: "1", Price: 1.3),
            new Lot(Name: "Antique", Id: "2", Price: 1.8),
            new Lot(Name: "Electronics", Id: "3", Price: 1.1),
            new Lot(Name: "Paintings", Id: "4", Price: 1.5)
        };
    }
}
