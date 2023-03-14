using System.Text;

namespace Models;
public class User
{
    private int _id;
    private string? _firstName;
    private string? _lastName;
    private string? _username;
    private string? _password;
    private int _wallet;
    
    public List<Item> listOfItems = new List<Item>();

    public User(){
        
    }
    public User(int id, string? firstname, string? lastname, string? username, string? password, int wallet)
    {
        _id = id;
        _firstName = firstname;
        _lastName = lastname;
        _username = username;
        _password = password;
        _wallet = wallet;
    }

    public int Id {
        set {
            _id = value;
        }
        get {
            return _id;
        }
    }
    public string? FirstName {
        set {
            _firstName = value;
        }
        get {
            return _firstName;
        }
    }
    public string? LastName {
        set {
            _lastName = value;
        }
        get {
            return _lastName;
        }
    }
    public string? Username {
        set {
            _username = value;
        }
        get {
            return _username;
        }
    }
    public string? Password {
        set {
            _password = value;
        }
        get {
            return _password;
        }
    }
    public int Wallet {
        set {
            _wallet = value;
        }
        get {
            return _wallet;
        }
    }

    public override string ToString()
    {
        StringBuilder sb = new();
        sb.Append($"ID: {this.Id} | Username: {this.Username}");
        return sb.ToString();
    }
}
