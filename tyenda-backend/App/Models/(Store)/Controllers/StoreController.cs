using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tyenda_backend.App.Base_Controllers;
using tyenda_backend.App.Models._Store_.Services._Add_Remove_Cart_;
using tyenda_backend.App.Models._Store_.Services._Follow_;
using tyenda_backend.App.Models._Store_.Services._Follow_.Form;
using tyenda_backend.App.Models._Store_.Services._Get_Random_Stores_;
using tyenda_backend.App.Models._Store_.Services._Stores_Search_;
using tyenda_backend.App.Models._Store_.Services._View_Profile_;
using tyenda_backend.App.Models._Store_.Views;
using tyenda_backend.App.Models.Form;

namespace tyenda_backend.App.Models._Store_.Controllers
{
    public class StoreController : AuthenticationController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public StoreController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [Authorize(Policy = Constants.CustomerPolicy)]
        [HttpGet("Random()")]
        public async Task<IActionResult> GetRandomStores([FromQuery] int top, [FromQuery] int skip)
        {
            try
            {
                var req = new GetRandomStores(top, skip);
                var result = await _mediator.Send(req);
                return Ok(result);
            }
            catch (Exception error)
            {
                return Ok(new {error = true, message = error.Message});
            }
        }

        [Authorize(Policy = Constants.CustomerPolicy)]
        [HttpPost("Follow()")]
        public async Task<IActionResult> FollowUnfollow([FromBody] FollowForm form)
        {
            try
            {
                var req = new Follow(form);
                var res = await _mediator.Send(req);
                return Ok(new {isFollowed = res});
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
        [HttpPost("Search()")]
        public async Task<IActionResult> StoresSearch([FromBody] ItemStoreSearchForm form)
        {
            try
            {
                var req = new StoresSearch(form);
                var res = await _mediator.Send(req);
                var result = _mapper.Map<ICollection<StoreModerateView>>(res);
                return Ok(result);
            }
            catch (Exception error)
            {
                return Ok(new {error = true, message = error.Message});
            }
        }
        
        [Authorize(Policy = Constants.CustomerPolicy)]
        [HttpGet("Profile/{storeId}")]
        public async Task<IActionResult> ViewStoreProfile([FromRoute] string storeId)
        {
            try
            {
                var req = new ViewProfile(storeId);
                var result = await _mediator.Send(req);
                return Ok(result);
            }
            catch (Exception error)
            {
                return Ok(new {error = true, message = error.Message});
            }
        }
        
    }
}