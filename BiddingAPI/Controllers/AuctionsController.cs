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
    public async Task<ActionResult<IEnumerable<Lot>>> GetLots(string auctionId)
    {
        switch(auctionId){
            case "1":
                return new Lot[]{
                    new Lot(Name: "Apples", Id: "1", Price: 1.21),
                    new Lot(Name: "Oranges", Id: "2", Price: 4.75),
                    new Lot(Name: "Bananas", Id: "3", Price: 5.19),
                };
            case "2":
                return new Lot[]{
                    new Lot(Name: "Closet", Id: "1", Price: 2.24),
                    new Lot(Name: "Mirror", Id: "2", Price: 5.74),
                    new Lot(Name: "Chair", Id: "3", Price: 9.29),
                    new Lot(Name: "Table", Id: "4", Price: 6.69),
                };
            case "3":
                return new Lot[]{
                    new Lot(Name: "Radio", Id: "1", Price: 2.13),
                    new Lot(Name: "TV", Id: "2", Price: 5.74),
                    new Lot(Name: "Smartphone", Id: "3", Price: 9.29),
                    new Lot(Name: "Speakers", Id: "4", Price: 1.69),
                    new Lot(Name: "Smartwatch", Id: "5", Price: 2.29),
                    new Lot(Name: "Toaster", Id: "6", Price: 9.44),
                };
            case "4":
                return new Lot[]{
                    new Lot(Name: "Mona Lisa", Id: "1", Price: 50.78),
                    new Lot(Name: "The Scream", Id: "2", Price: 78.81),
                    new Lot(Name: "The Starry Night", Id: "3", Price: 42.21),
                    new Lot(Name: "The Kiss", Id: "4", Price: 93.99),
                };
            default:
                return NotFound();
        };
    }
}
