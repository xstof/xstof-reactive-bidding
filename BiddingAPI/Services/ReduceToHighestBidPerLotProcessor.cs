using System.Reactive.Linq;
using BiddingAPI.Services;
using BiddingAPI.Models;

namespace BiddingAPI.Processors;

public class ReduceToHighestBidPerLotProcessor : IProcessor
{
    private ILogger<TestProcessor> _logger;
    private IBidsProvider _bidsProvider;

    private IObservable<Bid>? bidsStream;

    public ReduceToHighestBidPerLotProcessor(ILogger<TestProcessor> logger,
                                             IBidsProvider bidsProvider){
        _logger = logger;
        _bidsProvider = bidsProvider;
    }

    public void Initialize()
    {
        setupRx();

        // Reduce the output to the highest bid per lot in last X milliseconds:
           highestBidPerLot(10_000);
    }

    private void setupRx(){
        _logger.LogInformation("setting up rx in most highest bid per lot processor");
        bidsStream = _bidsProvider!.GetStreamOfBids();
    }

    private void highestBidPerLot(int windowLengthInMilliSeconds){
        Comparer<Bid> bidByPriceComparer = 
                Comparer<Bid>.Create((a, b) => a.BiddingPrice.CompareTo(b.BiddingPrice));

        // Here's the actual RX streaming processing code that counts number of bids per auction in last 30 seconds:
        bidsStream!
        .GroupBy(bid => bid.AuctionId)  // observable of [grouped observable - by auction]
        .Select(bidForAuction => bidForAuction.GroupBy(bid => bid.LotId)  // observable of [observable of [grouped observable - by lot]
                                              .Select(bidForLot => bidForLot.Window(TimeSpan.FromMilliseconds(windowLengthInMilliSeconds))
                                                                            .Select(window => window.Max(bidByPriceComparer))
                                                                            .Merge()
                                                     )
               )
        .Merge()
        .Merge()
        .Where(bid => bid != null)
        .Do(auction => _logger.LogInformation($"The maximum bid for Lot {auction.LotId} in Auction {auction.AuctionId} was {auction.BiddingPrice} from user: {auction.UserId}"))
        .Subscribe();
    }

    private class AuctionBidCount{
        public string Auction { get; init; }
        public int BidCount { get; init; }
    }

}