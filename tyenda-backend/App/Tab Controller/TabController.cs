using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using tyenda_backend.App.Base_Controllers;
using tyenda_backend.App.Models._Item_.Services._Get_Item_Name_;
using tyenda_backend.App.Models._Order_.Services._Get_Order_Reference_;
using tyenda_backend.App.Models._Store_.Services._Get_Store_Name_;

namespace tyenda_backend.App.Tab_Controller
{
    public class TabController : AuthenticationController
    {
        private readonly IMediator _mediator;

        public TabController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpGet("Store/{id}")]
        public async Task<IActionResult> GetStoreName([FromRoute] string id)
        {
            try
            {
                var req = new GetStoreName(id);
                var res = await _mediator.Send(req);
                return Ok(new {value = res});
            }
            catch (Exception error)
            {
                return Ok(new {error = true, message = error.Message});
            }
        }
        
        [HttpGet("Item/{id}")]
        public async Task<IActionResult> GetItemName([FromRoute] string id)
        {
            try
            {
                var req = new GetItemName(id);
                var res = await _mediator.Send(req);
                return Ok(new {value = res});
            }
            catch (Exception error)
            {
                return Ok(new {error = true, message = error.Message});
            }
        }

        [HttpGet("Order/{id}")]
        public async Task<IActionResult> GetOrderReference([FromRoute] string id)
        {
            try
            {
                var req = new GetOrderReference(id);
                var reference = await _mediator.Send(req);
                return Ok(new {error = false, value = reference});
            }
            catch (Exception error)
            {
                return Ok(new {error = true, message = error.Message});
            }
        }
    }
}