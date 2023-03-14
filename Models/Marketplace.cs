using System.Text;

namespace Models;
public class Marketplace
{
    private int _listingId;
    private int _itemId;
    private int _quantity;
    private int _unitPrice;
    private int _sellerId;
    
    public Marketplace(int listingId, int itemId, int quantity, int unitPrice, int sellerId)
    {
        _listingId=listingId;
        _itemId=itemId;
        _quantity=quantity;
        _unitPrice=unitPrice;
        _sellerId=sellerId;
    }
    public int ListingId {
        set {
            _listingId=value;
        }
        get {
            return _listingId;
        }
    }
    public int ItemId {
        set {
            _itemId=value;
        }
        get {
            return _itemId;
        }
    }
    public int Quantity {
        set {
            _quantity=value;
        }
        get {
            return _quantity;
        }
    }
    public int UnitPrice {
        set {
            _unitPrice=value;
        }
        get {
            return _unitPrice;
        }
    }
    public int SelleId {
        set {
            _sellerId=value;
        }
        get {
            return _sellerId;
        }
    }

    public override string ToString()
    {
        StringBuilder sb = new();
        sb.Append($"ID: {this.ListingId} | ItemID: {this.ItemId}");
        return sb.ToString();
    }
}




