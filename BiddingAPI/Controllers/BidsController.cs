using BiddingAPI.Models;
using BiddingAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BiddingAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class BidsController : ControllerBase
{
    private readonly ILogger<BidsController> _logger;
    private readonly IBidsHandler _bidsHandler;

    public BidsController(ILogger<BidsController> logger, IBidsHandler bidsHandler)
    {
        _logger = logger;
        _bidsHandler = bidsHandler;
    }

    [HttpPost(Name = "PlaceBid")]
    public void Post(Bid bid)
    {
        // Add current bid to consumable stream of bids
        _bidsHandler.HandleBid(bid);
    }
}
