using System.Text;

namespace Models;
public class Item
{
    private int _id;
    private string? _name;
    public Item(int id, string? name)
    {
        _id=id;
        _name=name;        
    }
    public int Id {
        set {
            _id=value;
        }
        get {
            return _id;
        }
    }
    public string? Name {
        set {
            _name=value;
        }
        get {
            return _name;
        }
    }

    public override string ToString()
    {
        StringBuilder sb = new();
        sb.Append($"ID: {this.Id} | Name: {this.Name}");
        return sb.ToString();
    }
}




