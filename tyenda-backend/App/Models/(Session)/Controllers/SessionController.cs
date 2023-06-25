﻿using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using tyenda_backend.App.Base_Controllers;
using tyenda_backend.App.Models._Session_.Services._Token_Expiration_Checker_;
using tyenda_backend.App.Models._Session_.Services._Token_Expiration_Checker_.Form;

namespace tyenda_backend.App.Models._Session_.Controllers
{
    public class SessionController : AuthenticationController
    {
        private readonly IMediator _mediator;

        public SessionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("Check()")]
        public async Task<IActionResult> TokenExpirationCheck([FromBody] TokenExpirationCheckerForm form)
        {
            try
            {
                var req = new TokenExpirationChecker(form);
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