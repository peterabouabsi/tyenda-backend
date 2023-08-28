using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using tyenda_backend.App.Base_Controllers;
using tyenda_backend.App.Models._Account_.Services._Get_Profile_;

namespace tyenda_backend.App.Models._Customer_.Controllers
{
    public class CustomerController : AuthenticationController
    {
        private readonly IMediator _mediator;

        public CustomerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("Profile")]
        public async Task<IActionResult> GetProfile()
        {
            try
            {
                var req = new GetProfile();
                var res = await _mediator.Send(req);
                return Ok(res);
            }
            catch (Exception error)
            {
                return Ok(new {error = true, message = error.Message});
            }
        }

    }
}