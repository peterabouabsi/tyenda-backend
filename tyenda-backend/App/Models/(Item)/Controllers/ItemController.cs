using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tyenda_backend.App.Base_Controllers;
using tyenda_backend.App.Models._Item_.Services._AddToCart_;
using tyenda_backend.App.Models._Item_.Services._Get_Random_Items_;
using tyenda_backend.App.Models.Form;

namespace tyenda_backend.App.Models._Item_.Controllers
{
    public class ItemController : AuthenticationController
    {
        private readonly IMediator _mediator;

        public ItemController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [Authorize(Policy = "CustomerPolicy")]
        [HttpGet("Random()")]
        public async Task<IActionResult> GetRandomItems([FromQuery] int top, [FromQuery] int skip)
        {
            try
            {
                var req = new GetRandomItems(top, skip);
                var result = await _mediator.Send(req);
                return Ok(result);
            }
            catch (Exception error)
            {
                return Ok(new {error = true, message = error.Message});
            }
        }

        [Authorize(Policy = "CustomerPolicy")]
        [HttpPost("AddCart()")]
        public async Task<IActionResult> AddRemoveCart([FromBody] AddToCartForm form)
        {
            try
            {
                var req = new AddToCart(form);
                var res = await _mediator.Send(req);
                return Ok(new {isAddedToCart = res});
            }
            catch (Exception error)
            {
                return Ok(new {error = true, message = error.Message});
            }
        }
    }
}