namespace tyenda_backend.App.Models._Account_.Services._Reset_Password_.Form
{
    public class ResetPasswordForm
    {
        public string NewPassword { get; set; } = "";
        public string ConfirmPassword { get; set; } = "";
        public string Token { get; set; } = "";
    }
}