namespace UpBlogApp.DTO;

public class RegisterModel
{
    public int ID { get; set; }
    // Profile
    public string Name { get; set; }
    public string Email { get; set; }
    public string AvaterURL { get; set; }
    public string Role { get; set; }

    // Login
    public string UserName { get; set; }
    public string LoginPassword { get; set; }
}