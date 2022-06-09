using BiddingAPI.Models;

namespace BiddingAPI.Services;

public interface IBidsProvider
{
    IObservable<Bid> GetStreamOfBids();
}