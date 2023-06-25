namespace tyenda_backend.App.Models._Account_.Services._Change_Password_.Form
{
    public class ChangePasswordForm
    {
        public string OldPassword { get; set; } = "";
        public string NewPassword { get; set; } = "";
        public string ConfirmPassword { get; set; } = "";
    }
}