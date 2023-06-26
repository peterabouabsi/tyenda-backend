using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using tyenda_backend.App.Base_Controllers;
using tyenda_backend.App.Models._Notification_.Services._Get_Notifications_;
using tyenda_backend.App.Models._Notification_.Services._View_Notification_;
using tyenda_backend.App.Models._Notification_.Views;

namespace tyenda_backend.App.Models._Notification_.Controllers
{
    public class NotificationController : AuthenticationController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public NotificationController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet()]
        public async Task<IActionResult> GetNotifications()
        {
            try
            {
                var req = new GetNotifications();
                var res = await _mediator.Send(req);
                var result = _mapper.Map<ICollection<ViewModerateAlert>>(res);
                return Ok(result);
            }
            catch (Exception error)
            {
                return Ok(new {error = true, message = error.Message});
            }
        }

        [HttpPost("View/{notificationId}")]
        public async Task<IActionResult> ViewNotification([FromRoute] string notificationId)
        {
            try
            {
                var req = new ViewNotification(notificationId);
                await _mediator.Send(req);
                return Ok(new {error = false, message = "Notification viewed"});
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }
    }
}