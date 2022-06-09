using BiddingAPI.Models;

namespace BiddingAPI.Services;

public interface IBidsHandler
{
    void HandleBid(Bid bid);
}