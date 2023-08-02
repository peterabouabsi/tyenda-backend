namespace tyenda_backend.App.Models._Account_.Services._Customer_Signup_.Forms
{
    public class CustomerSignupForm
    {
        public string Firstname { get; set; } = "";
        public string Lastname { get; set; } = "";
        public string Email { get; set; } = "";
        public string? PhoneNumber { get; set; } = "";
        public string Username { get; set; } = "";
        public string? Password { get; set; } = "";
    }
}