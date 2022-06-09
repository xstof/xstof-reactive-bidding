using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using BiddingAPI.Models;

namespace BiddingAPI.Services;

public class BidsProvider : IBidsProvider, IBidsHandler
//: IBidsHandler, IBidsProvider, IBidsProvider
{
    private readonly ILogger<BidsProvider> logger;
    private readonly ConcurrentQueue<Bid> queue;
    private IObservable<Bid> stream;

    public BidsProvider(ILogger<BidsProvider> logger)
    {
        logger.LogInformation("constructing Bids provider");

        this.logger = logger!;
        this.queue = new ConcurrentQueue<Bid>();

        // Initialize inner Observable - at this point there are no Observers yet so, we don't have an instance of Observer to push data towards
        var innerObservable = initStream();
        logger.LogInformation("initialized inner-observable - no subscriptions yet");

        // Subscribe to the inner Observable ourselves and make sure that subscription is shared by anyone else
        var connectedObservable = innerObservable.Publish();          // Ensure subscription is shared amongst all Observers
        logger.LogInformation("published inner-observable");

        var connectedObservableDisposable = connectedObservable.Connect();      // Connect right away so all subscribers as of now get the same data pushed
        stream = connectedObservable;
        logger.LogInformation("connected inner-observable");
        logger.LogInformation("constructed DeviceHeartbeatProvider");

    }

    public void HandleBid(Bid bid)
    {
        queue.Enqueue(bid);
    }

    private IObservable<Bid> initStream()
    {
        logger.LogInformation("initializing stream of device heartbeats");
        var o = Observable.Create<Bid>((IObserver<Bid> observer, CancellationToken token) =>
        {
            logger.LogInformation("creating observable in initStream");
            return Task.Run(() =>
            {
                while (!token.IsCancellationRequested)
                {
                    if (queue.TryDequeue(out Bid? nextItem))
                    {
                        observer.OnNext(nextItem);
                    }
                    else
                    {
                        Thread.Sleep(100);
                    }
                }
            }, token);
        });

        return o.ObserveOn(NewThreadScheduler.Default);
    }

    public IObservable<Bid> GetStreamOfBids()
    {
        return stream;
    }
}