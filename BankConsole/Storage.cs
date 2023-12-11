using MimeKit.Encodings;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace BankConsole;

public static class Storage
{
    static string filePath = AppDomain.CurrentDomain.BaseDirectory + @"\users.json";

    public static bool AddUser(User user)
    {
        string json = "", usersInFile = "";

        if(File.Exists(filePath))
            usersInFile = File.ReadAllText(filePath);
            
        if(usersInFile.IndexOf($"\"ID\": {user.GetID()}") != -1)
            return false;

        var listUsers = JsonConvert.DeserializeObject<List<object>>(usersInFile);

        if (listUsers == null)
            listUsers = new List<object>();
        
        listUsers.Add(user);

        JsonSerializerSettings settings = new JsonSerializerSettings{Formatting = Formatting.Indented};

        json = JsonConvert.SerializeObject(listUsers, settings);

        File.WriteAllText(filePath, json);

        return true;

    }

    public static List<User>GetNewUsers()
    {
        string usersInFile = "";
        var listUsers = new List<User>();


        if(File.Exists(filePath))
            usersInFile = File.ReadAllText(filePath);

        var listObjects = JsonConvert.DeserializeObject<List<object>>(usersInFile);

        if(listObjects == null)
            return listUsers;

        foreach(object obj in listObjects)
        {
            User newUser;
            JObject user = (JObject)obj;

            if (user.ContainsKey("TaxRegime"))
                newUser = user.ToObject<Client>();
            else
                newUser = user.ToObject<Employee>();

            listUsers.Add(newUser);
        }

        var newUsersList = listUsers.Where(user => user.GetRegisterDate().Date.Equals(DateTime.Today)).ToList();

        return newUsersList;
    }

    public static bool DeleteUser(int id)
    {
        string usersInFile = "";
        var listUsers = new List<User>();

        if(File.Exists(filePath))
            usersInFile = File.ReadAllText(filePath);

        var listObjects = JsonConvert.DeserializeObject<List<object>>(usersInFile);

        if(listObjects == null)
            return false;
        
        if(usersInFile.IndexOf($"\"ID\": {id}") == -1)
            return false;

        foreach(object obj in listObjects)
        {
            User newUser;
            JObject user = (JObject)obj;

            if (user.ContainsKey("TaxRegime"))
                newUser = user.ToObject<Client>();
            else
                newUser = user.ToObject<Employee>();
            
            listUsers.Add(newUser);
        }
        

    var userToDelete = listUsers.Where(user => user.GetID() == id).Single();
    
    listUsers.Remove(userToDelete);

    JsonSerializerSettings settings = new JsonSerializerSettings{Formatting = Formatting.Indented};

    string json = JsonConvert.SerializeObject(listUsers, settings);

    File.WriteAllText(filePath, json);

    return true;
    }
}