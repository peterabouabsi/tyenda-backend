using System;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tyenda_backend.App.Base_Controllers;
using tyenda_backend.App.Models._Item_.Services._Add_Remove_Cart_;
using tyenda_backend.App.Models._Item_.Services._Get_Random_Items_;
using tyenda_backend.App.Models._Item_.Services._Like_Item_;
using tyenda_backend.App.Models._Item_.Services._Like_Item_.Form;
using tyenda_backend.App.Models._Item_.Services._Top_Selling_Item_;
using tyenda_backend.App.Models._Item_.Views;
using tyenda_backend.App.Models.Form;

namespace tyenda_backend.App.Models._Item_.Controllers
{
    public class ItemController : AuthenticationController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public ItemController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [Authorize(Policy = "CustomerPolicy")]
        [HttpGet("Top()")]
        public async Task<IActionResult> GetTopSellingItem()
        {
            try
            {
                var req = new TopSellingItem();
                var res = await _mediator.Send(req);
                var result = _mapper.Map<ItemBasicView>(res);
                return Ok(result);
            }
            catch (Exception error)
            {
                return Ok(new {error = true, message = error.Message});
            }
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
        [HttpPost("AddRemoveCart()")]
        public async Task<IActionResult> AddRemoveCart([FromBody] AddRemoveCartForm form)
        {
            try
            {
                Console.Write(form.ItemId);
                var req = new AddRemoveCart(form);
                var res = await _mediator.Send(req);
                return Ok(new {isAddedToCart = res});
            }
            catch (Exception error)
            {
                return Ok(new {error = true, message = error.Message});
            }
        }

        [Authorize(Policy = "CustomerPolicy")]
        [HttpPost("Like()")]
        public async Task<IActionResult> LikeItem([FromBody] LikeItemForm form)
        {
            try
            {
                var req = new LikeItem(form);
                var res = await _mediator.Send(req);
                return Ok(new {isItemLiked = res});
            }
            catch (Exception error)
            {
                return Ok(new {error = true, message = error.Message});
            }
        }
    }
}