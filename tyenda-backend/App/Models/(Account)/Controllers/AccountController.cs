using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tyenda_backend.App.Base_Controllers;
using tyenda_backend.App.Models._Account_.Services._Activate_Account_;
using tyenda_backend.App.Models._Account_.Services._Activate_Account_.Form;
using tyenda_backend.App.Models._Account_.Services._Change_Password_;
using tyenda_backend.App.Models._Account_.Services._Change_Password_.Form;
using tyenda_backend.App.Models._Account_.Services._Customer_Signup_;
using tyenda_backend.App.Models._Account_.Services._Customer_Signup_.Forms;
using tyenda_backend.App.Models._Account_.Services._Forget_Password_;
using tyenda_backend.App.Models._Account_.Services._Get_Profile_Image_;
using tyenda_backend.App.Models._Account_.Services._Login_;
using tyenda_backend.App.Models._Account_.Services._Login_Google_;
using tyenda_backend.App.Models._Account_.Services._Login_Google_.Form;
using tyenda_backend.App.Models._Account_.Services._Logout_;
using tyenda_backend.App.Models._Account_.Services._Logout_.Form;
using tyenda_backend.App.Models._Account_.Services._My_Role_;
using tyenda_backend.App.Models._Account_.Services._Refresh_Token_;
using tyenda_backend.App.Models._Account_.Services._Refresh_Token_.Form;
using tyenda_backend.App.Models._Account_.Services._Reset_Password_;
using tyenda_backend.App.Models._Account_.Services._Reset_Password_.Form;
using tyenda_backend.App.Models._Account_.Services._Send_Activation_Email_;
using tyenda_backend.App.Models._Account_.Services._Send_Activation_Email_.Form;
using tyenda_backend.App.Models._Account_.Services._Store_Signup_;
using tyenda_backend.App.Models._Account_.Services._Store_Signup_.Form;
using TyendaBackend.App.Models._Account_.Services._Forget_Password_.Form;
using TyendaBackend.App.Models._Account_.Services._Login_.Form;

namespace tyenda_backend.App.Models._Account_.Controllers
{
    public class AccountController : AuthenticationController
    {
        private readonly IMediator _mediator;

        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("Login()")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginForm form)
        {
            try
            {
                var req = new Login(form);
                var res = await _mediator.Send(req);
                return Ok(res);
            }
            catch (Exception error)
            {
                return Ok(new {error = true, message = error.Message});
            }
        }
        
        [HttpPost("LoginGoogle()")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginWithGoogle([FromBody] LoginGoogleForm form)
        {
            try
            {
                var req = new LoginGoogle(form);
                var res = await _mediator.Send(req);
                return Ok(res);
            }
            catch (Exception error)
            {
                return Ok(new {error = true, message = error.Message});
            }
        }

        [HttpGet("Role()")]
        public async Task<IActionResult> GetAccountRole()
        {
            try
            {
                var req = new MyRole();
                var res = await _mediator.Send(req);
                return Ok(res);
            }
            catch (Exception error)
            {
                return Ok(new {error = true, message = error.Message});
            }
        }

        [HttpPost("ForgetPassword()")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordForm form)
        {
            try
            {
                var req = new ForgetPassword(form);
                await _mediator.Send(req);
                return Ok(new {error = false, message = "Email successfully sent"});
            }
            catch (Exception error)
            {
                return Ok(new {error = true, message = error.Message});
            }
        }

        [HttpPost("ResetPassword()")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordForm form)
        {
            try
            {
                var req = new ResetPassword(form);
                await _mediator.Send(req);
                return Ok(new {error = false, message = "Password has been successfully changed"});
            }
            catch (Exception error)
            {
                return Ok(new { error=true, message=error.Message });
            }
        }

        [HttpPost("Customer/Signup()")]
        [AllowAnonymous]
        public async Task<IActionResult> CustomerSignup([FromBody] CustomerSignupForm form)
        {
            try
            {
                var req = new CustomerSignup(form);
                await _mediator.Send(req);
                return Ok(new {error = false, message = "Account successfully created"});
            }
            catch (Exception error)
            {
                return Ok(new {error = true, message = error.Message});
            }
        }

        [HttpPost("Store/Signup()")]
        [AllowAnonymous]
        public async Task<IActionResult> StoreSignup([FromBody] StoreSignupForm form)
        {
            try
            {
                var req = new StoreSignup(form);
                await _mediator.Send(req);
                return Ok(new {error = false, message = "Account successfully created"});
            }
            catch (Exception error)
            {
                return Ok(new {error = true, message = error.Message});
            }
        }

        [HttpPost("SendActivationEmail()")]
        [AllowAnonymous]
        public async Task<IActionResult> SendActivationEmail([FromBody] SendActivationEmailForm form)
        {
            try
            {
                var req = new SendActivationEmail(form);
                await _mediator.Send(req);
                return Ok(new {error = false, message = "Email sent successfully"});
            }
            catch (Exception error)
            {
                return Ok(new {error = true, message = error.Message});
            }
        }

        [HttpPost("Activate()")]
        [AllowAnonymous]
        public async Task<IActionResult> Activate([FromBody] ActivateAccountForm form)
        {
            try
            {
                var req = new ActivateAccount(form);
                await _mediator.Send(req);
                return Ok(new {error = false, message = "Account activated"});
            }
            catch (Exception error)
            {
                return Ok(new {error = true, message = error.Message});
            }
        }

        [HttpPost("RefreshToken()")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenForm form)
        {
            try
            {
                var req = new RefreshToken(form);
                var res = await _mediator.Send(req);
                return Ok(res);
            }
            catch (Exception error)
            {
                return Ok(new {error = true, message = error});
            }
        }

        [HttpPost("Logout()")]
        public async Task<IActionResult> Logout([FromBody] LogoutForm form)
        {
            try
            {
                var req = new Logout(form);
                await _mediator.Send(req);
                return Ok(new {error = false, message = "Logged out successfully"});
            }
            catch (Exception error)
            {
                return Ok(new {error = true, message = error.Message});
            }
        }

        [HttpPost("ChangePassword()")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordForm form)
        {
            try
            {
                var req = new ChangePassword(form);
                await _mediator.Send(req);
                return Ok(new {error = false, message = "Password changed"});
            }
            catch (Exception error)
            {
                return Ok(new {error = true, message = error.Message});
            }
        }

        [HttpGet("Profile/Image")]
        public async Task<IActionResult> GetProfileImage()
        {
            try
            {
                var req = new GetProfileImage();
                var res = await _mediator.Send(req);
                return Ok(new {profileImage = res});
            }
            catch (Exception error)
            {
                return Ok(new {error = true, message = error.Message});
            }
        }
    }
}