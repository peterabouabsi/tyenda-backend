using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace tyenda_backend.App.Base_Controllers
{
    [ApiController]
    [Route("api/Anonymous/[controller]")]
    [AllowAnonymous]
    public class AnonymousController : ControllerBase
    {
    }
}