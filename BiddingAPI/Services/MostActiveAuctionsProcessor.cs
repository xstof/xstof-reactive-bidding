using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.PlatformServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using BiddingAPI.Processors;
using BiddingAPI.Services;
using BiddingAPI.Models;

namespace BiddingAPI.Processors;

public class MostActiveAuctionsProcessor : IProcessor
{
    private ILogger<TestProcessor> _logger;
    private IBidsProvider _bidsProvider;

    private IObservable<Bid>? bidsStream;

    public MostActiveAuctionsProcessor(ILogger<TestProcessor> logger,
                         IBidsProvider bidsProvider){
        _logger = logger;
        _bidsProvider = bidsProvider;
    }

    public void Initialize()
    {
        setupRx();

        // Log the number of bids in the last time window of X seconds:
        // numberOfBidsAcrossAllActionsInLastNumberOfSeconds(10);

        // Log the number of bids in the last time window of X seconds PER AUCTION:
        // numberOfBidsAcrossAllActionsInLastNumberOfSecondsPerAuction(10);

        // Log the most active auction in last time window of X seconds:
        mostActiveActionInLastNumberOfSeconds(10);
    }

    private void setupRx(){
        _logger.LogInformation("setting up rx in most active auctions processor");
        bidsStream = _bidsProvider!.GetStreamOfBids();
    }

    private void numberOfBidsAcrossAllActionsInLastNumberOfSeconds(int windowLengthInSeconds){
        // Here's the actual RX streaming processing code that counts number of bids in last 30 seconds:
        bidsStream!.Window(TimeSpan.FromSeconds(windowLengthInSeconds)) // create non-overlapping tumbling window of bidding events
                   .Select(windowedBidStream => windowedBidStream.Count())
                   .Merge()
                   .Do(count => _logger.LogInformation($"In the last {windowLengthInSeconds} secs, {count} bids where placed across all auctions."))
                   .Subscribe();
    }

    private void numberOfBidsAcrossAllActionsInLastNumberOfSecondsPerAuction(int windowLengthInSeconds){
        // Here's the actual RX streaming processing code that counts number of bids per auction in last 30 seconds:
        bidsStream!.Window(TimeSpan.FromSeconds(windowLengthInSeconds)) // create non-overlapping tumbling window of bidding events
                   .Select(windowedBidStream => windowedBidStream       // every item in this observable contains the bids in a time window
                                                .GroupBy(bid => bid.AuctionId)       // group by auction within that time window
                                                .Select(auctionBidStreamInWindow =>  // for every auction group/observable, count the bids
                                                    auctionBidStreamInWindow.Count()
                                                    .Select(count => 
                                                    new {
                                                        Auction = auctionBidStreamInWindow.Key,
                                                        BidCount = count // count bids for Auction within time window
                                                    })
                                                )
                                                .Merge()
                   )
                   .Merge()
                   .Do(msg => _logger.LogInformation($"In the last {windowLengthInSeconds} secs, for auction {msg.Auction} {msg.BidCount} bids where placed."))
                   .Subscribe();
    }

        private void mostActiveActionInLastNumberOfSeconds(int windowLengthInSeconds){
            Comparer<AuctionBidCount> BidCountComparer = 
                Comparer<AuctionBidCount>.Create((a, b) => a.BidCount.CompareTo(b.BidCount));

            // Here's the actual RX streaming processing code that counts number of bids per auction in last 30 seconds:
            bidsStream!.Window(TimeSpan.FromSeconds(windowLengthInSeconds)) // create non-overlapping tumbling window of bidding events
                    .Select(windowedBidStream => windowedBidStream       // every item in this observable contains the bids in a time window
                                                    .GroupBy(bid => bid.AuctionId)       // group by auction within that time window
                                                    .Select(auctionBidStreamInWindow =>  // for every auction group/observable, count the bids
                                                        auctionBidStreamInWindow
                                                        .Count()         // count the bid (gives observable of int)
                                                        .Select(count => new AuctionBidCount {
                                                            Auction = auctionBidStreamInWindow.Key,
                                                            BidCount = count
                                                        })
                                                    )
                                                    .Merge()
                                                    .Max<AuctionBidCount>(BidCountComparer)
                    )
                    .Merge()
                    .Where(a => a != null)
                    .Do(auction => _logger.LogInformation($"In the last {windowLengthInSeconds} secs, auction {auction.Auction} is most popular with {auction.BidCount} bids"))
                    .Subscribe();
    }

    private class AuctionBidCount{
        public string Auction { get; init; }
        public int BidCount { get; init; }
    }

}