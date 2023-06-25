using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace tyenda_backend.App.Base_Controllers
{
    [ApiController]
    [Route("api/Authentication/[controller]")]
    [Authorize]
    public class AuthenticationController : ControllerBase
    {
    }
}