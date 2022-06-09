namespace BiddingAPI.Models;

public class Bid {

    public Bid(string auctionId, string lotId, string userId, double biddingPrice){
        this.AuctionId = auctionId;
        this.LotId = lotId;
        this.UserId = userId;
        this.BiddingPrice = biddingPrice;
    }
    
    public string AuctionId { get; init;}
    public string LotId { get; init;}
    public string UserId { get; init;}
    public double BiddingPrice { get; init;}

    public override string ToString()
    {
        return $"Auction {AuctionId}, Lot {LotId} got bid of {BiddingPrice.ToString()} by {UserId}";
    }
}