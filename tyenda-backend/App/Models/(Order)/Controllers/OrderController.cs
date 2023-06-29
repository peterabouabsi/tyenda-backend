using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tyenda_backend.App.Base_Controllers;
using tyenda_backend.App.Models._Order_.Services._Get_Recent_Orders_;
using tyenda_backend.App.Models._Order_.Views;

namespace tyenda_backend.App.Models._Order_.Controllers
{
    public class OrderController : AuthenticationController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public OrderController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [Authorize(Policy = "CustomerPolicy")]
        [HttpGet("Recent()")]
        public async Task<IActionResult> GetCustomerRecentOrders()
        {
            try
            {
                var req = new GetRecentOrders();
                var res = await _mediator.Send(req);
                var result = _mapper.Map<ICollection<OrderBasicView>>(res);
                return Ok(result);
            }
            catch (Exception error)
            {
                return Ok(new {error = true, message = error.Message});
            }
        }
    }
}