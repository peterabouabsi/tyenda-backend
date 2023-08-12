using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tyenda_backend.App.Base_Controllers;
using tyenda_backend.App.Models._Item_.Services._Add_Remove_Cart_;
using tyenda_backend.App.Models._Item_.Services._Get_Item_;
using tyenda_backend.App.Models._Item_.Services._Get_Item_For_Request_;
using tyenda_backend.App.Models._Item_.Services._Get_Random_Items_;
using tyenda_backend.App.Models._Item_.Services._Items_Search_;
using tyenda_backend.App.Models._Item_.Services._Like_Item_;
using tyenda_backend.App.Models._Item_.Services._Like_Item_.Form;
using tyenda_backend.App.Models._Item_.Services._My_Item_Orders_;
using tyenda_backend.App.Models._Item_.Services._Rate_Item_;
using tyenda_backend.App.Models._Item_.Services._Rate_Item_.Form;
using tyenda_backend.App.Models._Item_.Services._Top_Selling_Item_;
using tyenda_backend.App.Models._Item_.Views;
using tyenda_backend.App.Models._Order_.Views;
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

        [Authorize(Policy = Constants.CustomerPolicy)]
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

        [Authorize(Policy = Constants.CustomerPolicy)]
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

        [Authorize(Policy = Constants.CustomerPolicy)]
        [HttpPost("AddRemoveCart()")]
        public async Task<IActionResult> AddRemoveCart([FromBody] AddRemoveCartForm form)
        {
            try
            {
                var req = new AddRemoveCart(form);
                var res = await _mediator.Send(req);
                return Ok(new {isAddedToCart = res});
            }
            catch (Exception error)
            {
                return Ok(new {error = true, message = error.Message});
            }
        }

        [Authorize(Policy = Constants.CustomerPolicy)]
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
        
        [HttpPost("Search()")]
        public async Task<IActionResult> ItemsSearch([FromBody] ItemStoreSearchForm form)
        {
            try
            {
                var req = new ItemsSearch(form);
                var res = await _mediator.Send(req);
                var result = _mapper.Map<ICollection<ItemBasicView>>(res);
                return Ok(result);
            }
            catch (Exception error)
            {
                return Ok(new {error = true, message = error.Message});
            }
        }

        [Authorize(Policy = Constants.CustomerPolicy)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetItem([FromRoute] string id)
        {
            try
            {
                var req = new GetItem(id);
                var res = await _mediator.Send(req);
                return Ok(res);
            }
            catch (Exception error)
            {
                return Ok(new {error = true, message = error.Message});
            }
        }

        [Authorize(Policy = Constants.CustomerPolicy)]
        [HttpGet("{id}/MyOrders()")]
        public async Task<IActionResult> GetMyItemOrders([FromRoute] string id)
        {
            try
            {
                var req = new MyItemOrders(id);
                var res = await _mediator.Send(req);
                var result = _mapper.Map<ICollection<OrderBasicView>>(res);
                return Ok(result);
            }
            catch (Exception error)
            {
                return Ok(new {error = true, message = error.Message});
            }
        }

        [Authorize(Policy = Constants.CustomerPolicy)]
        [HttpPost("Rate()")]
        public async Task<IActionResult> RateItem([FromBody] RateItemForm form)
        {
            try
            {
                var req = new RateItem(form);
                var res = await _mediator.Send(req);
                return Ok(res);
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }

        [HttpGet("OrderRequest/{itemId}")]
        public async Task<IActionResult> GetItemForRequest([FromRoute] string itemId)
        {
            try
            {
                var req = new GetItemForRequest(itemId);
                var res = await _mediator.Send(req);
                var result = _mapper.Map<ItemEntryView>(res);
                return Ok(result);
            }
            catch (Exception error)
            {
                return Ok(new {error = true, message = error.Message});
            }
        }
    }
}