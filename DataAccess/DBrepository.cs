using Models;
using System.Data.SqlClient;

namespace DataAccess;
public class DBRepository : IRepository
{
    public User AddUser(User user)
    {
        try{
            using SqlConnection connect = new SqlConnection(_connectString);
            connect.Open();
            using SqlCommand command = new SqlCommand("INSERT INTO EmployeeTable(EmployeeName, Username, userID, empPassword, EmployeeType) VALUES (@eName, @uName, @uID, @ePwd, @eType)", connect);
            command.Parameters.AddWithValue("@eName", user.empName);
            command.Parameters.AddWithValue("@uName", user.username);
            command.Parameters.AddWithValue("@uID", user.userID);
            command.Parameters.AddWithValue("@ePwd", user.password);
            command.Parameters.AddWithValue("@eType", user.employeeType);
            string userID = (string)command.ExecuteScalar();


            return user;
        }
        catch(SqlException ex) {
            Log.Warning("Error! Couldn't add user because {0}", ex);
            throw;
        }
    }

    public List<Item> GetAllItems()
    {
        throw new NotImplementedException();
    }

    public List<User> GetAllUsers()
    {
        throw new NotImplementedException();
    }
}
