using DataAccess;
using Models;

namespace Services;
public class UserServices{
    IRepository _iRepo;
    UserServices(IRepository iRepo){
        _iRepo = iRepo;
    }

    public List<User>? GetUsers(){
        return _iRepo.GetAllUsers();
    }
    public void Login(){

    }

    public void CreateAccount(){
        try{
            Console.Write("Please Enter Your First Name: ");
            string fname = Console.ReadLine()!;

            Console.Write("Please Enter Your Last Name: ");
            string lname = Console.ReadLine()!;

            Console.Write("Please Create Your Username: ");
            string uName = Console.ReadLine()!;

            Console.Write("Please Create A Password: ");
            string pwd = Console.ReadLine()!;

            User newUser = new ();
            newUser.FirstName = fname;
            newUser.LastName = lname;
            newUser.Username = uName;
            newUser.Password = pwd;
            newUser.Wallet = 1000;
            _iRepo.AddUser(newUser);

        }
        catch(Exception e){
            Console.Write("Error When Creating Account: {0}", e);
        }
    }
    
}
