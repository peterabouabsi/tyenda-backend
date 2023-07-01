using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tyenda_backend.App.Base_Controllers;
using tyenda_backend.App.Models._Cart_.Services._Get_Stores_;
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
        public async Task<IActionResult> GetCartStores([FromQuery] int top, [FromQuery] int skip)
        {
            var req = new GetStores(top ,skip);
            var res = await _mediator.Send(req);
            var result = _mapper.Map<ICollection<CartStoreBasicView>>(res);
            return Ok(result);
        }
    }
}