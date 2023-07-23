using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tyenda_backend.App.Base_Controllers;
using tyenda_backend.App.Models._Order_.Services._Get_Orders_Overview_;
using tyenda_backend.App.Models._Order_.Services._Get_Recent_Orders_;
using tyenda_backend.App.Models._Order_.Services._Orders_Search_;
using tyenda_backend.App.Models._Order_.Services._Orders_Search_.Form;
using tyenda_backend.App.Models._Order_.Services._Request_Order_;
using tyenda_backend.App.Models._Order_.Services._Request_Order_.Forms;
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

        [Authorize(Policy = Constants.CustomerPolicy)]
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
        
        [Authorize(Policy = Constants.CustomerPolicy)]
        [HttpGet("Overview()")]
        public async Task<IActionResult> GetCustomerOrderOverview()
        {
            try
            {
                var req = new GetOrdersOverview();
                var res = await _mediator.Send(req);
                return Ok(res);
            }
            catch (Exception error)
            {
                return Ok(new {error = true, message = error.Message});
            }
        }
        
        [Authorize(Policy = Constants.CustomerPolicy)]
        [HttpPost("Search()")]
        public async Task<IActionResult> SearchOrders([FromBody] SearchForm form)
        {
            try
            {
                var req = new OrdersSearch(form);
                var res = await _mediator.Send(req);
                var result = _mapper.Map<ICollection<OrderBasicView>>(res);
                return Ok(result);
            }
            catch (Exception error)
            {
                return Ok(new {error = true, message = error.Message});
            }
        }

        [HttpPost("Request()")]
        public async Task<IActionResult> RequestOrder([FromBody] RequestOrderForm form)
        {
            try
            {
                var req = new RequestOrder(form);
                var orderId = await _mediator.Send(req);
                return Ok(new {error = false, message = "Your request has been successfully sent.", orderId = orderId});
            }
            catch (Exception error)
            {
                return Ok(new {error = true, message = error.Message});
            }
        }
    }
    
}