using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tyenda_backend.App.Base_Controllers;
using tyenda_backend.App.Models._Cart_.Services._Get_Items_;
using tyenda_backend.App.Models._Cart_.Services._Get_Stores_;
using tyenda_backend.App.Models._Cart_.Services._Item_Cart_Update_;
using tyenda_backend.App.Models._Cart_.Services._Item_Cart_Update_.Form;
using tyenda_backend.App.Models._Cart_.Views;

namespace tyenda_backend.App.Models._Cart_.Controllers
{
    public class CartController : AuthenticationController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public CartController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }
        
        [Authorize(Policy = "CustomerPolicy")]
        [HttpGet("Stores")]
        public async Task<IActionResult> GetCartStores([FromQuery] int top)
        {
            var req = new GetStores(top);
            var res = await _mediator.Send(req);
            var result = _mapper.Map<ICollection<CartStoreBasicView>>(res);
            return Ok(result);
        }
        
        [Authorize(Policy = "CustomerPolicy")]
        [HttpGet("Items")]
        public async Task<IActionResult> GetCartItems([FromQuery] int top)
        {
            var req = new GetItems(top);
            var res = await _mediator.Send(req);
            var result = _mapper.Map<ICollection<CartItemModerateView>>(res);
            return Ok(result);
        }

        [Authorize(Policy = "CustomerPolicy")]
        [HttpPost("Update()")]
        public async Task<IActionResult> UpdateItemCart([FromBody] ItemCartUpdateForm form)
        {
            try
            {
                var req = new ItemCartUpdate(form);
                var quantity = await _mediator.Send(req);
                return Ok(new {Quantity = quantity});
            }
            catch (Exception error)
            {
                return Ok(new {error = true, message = error.Message});
            }
        }
    }
}