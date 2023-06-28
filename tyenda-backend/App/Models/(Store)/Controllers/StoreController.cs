using System;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tyenda_backend.App.Base_Controllers;
using tyenda_backend.App.Models._Store_.Services._AddToCart_;
using tyenda_backend.App.Models._Store_.Services._Follow_;
using tyenda_backend.App.Models._Store_.Services._Follow_.Form;
using tyenda_backend.App.Models._Store_.Services._Get_Random_Stores_;
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

        [Authorize(Policy = "CustomerPolicy")]
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

        [Authorize(Policy = "CustomerPolicy")]
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