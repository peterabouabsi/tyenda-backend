using System;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tyenda_backend.App.Base_Controllers;

namespace tyenda_backend.App.Models._Comment_.Controllers
{
    public class CommentController : AuthenticationController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public CommentController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [Authorize(Policy = "CustomerPolicy")]
        [HttpGet("{itemId}")]
        public async Task<IActionResult> GetItemComments([FromRoute] string itemId)
        {
            try
            {
                var req = GetItemComments(itemId);
                return Ok();
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }
    }
}