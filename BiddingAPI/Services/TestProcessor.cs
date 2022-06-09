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

public class TestProcessor : IProcessor
{
    private ILogger<TestProcessor> _logger;
    private IBidsProvider _bidsProvider;

    private IObservable<Bid>? bidsStream;

    public TestProcessor(ILogger<TestProcessor> logger,
                         IBidsProvider bidsProvider){
        _logger = logger;
        _bidsProvider = bidsProvider;
    }

    public void Initialize()
    {
        setupRx();

        logAll();
    }

    private void setupRx(){
        _logger.LogInformation("setting up rx in test processor");
        bidsStream = _bidsProvider!.GetStreamOfBids();
    }

    private void logAll(){
        // Here's the actual RX streaming processing code that simply logs every incoming out:
        bidsStream!.Do(bid => _logger.LogInformation(
                        $"TestProcessor received bid: {bid.ToString()}"))
                  .Subscribe();
    }

}