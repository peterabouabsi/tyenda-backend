using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using tyenda_backend.App.Base_Controllers;
using tyenda_backend.App.Models._Comment_.Services._Add_Comment_;
using tyenda_backend.App.Models._Comment_.Services._Add_Comment_.Form;
using tyenda_backend.App.Models._Comment_.Services._Delete_Comment_;
using tyenda_backend.App.Models._Comment_.Services._Get_Item_Comments_;

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

        [HttpGet("{itemId}")]
        public async Task<IActionResult> GetComments([FromRoute] string itemId)
        {
            try
            {
                var req = new GetItemComments(itemId);
                var result = await _mediator.Send(req);
                return Ok(result);
            }
            catch (Exception error)
            {
                return Ok(new {error = true, message = error.Message});
            }
        }
        
        [HttpDelete("{commentId}")]
        public async Task<IActionResult> DeleteComments([FromRoute] string commentId)
        {
            try
            {
                var req = new DeleteComment(commentId);
                await _mediator.Send(req);
                return Ok(new {error = false, message = "Comment deleted"});
            }
            catch (Exception error)
            {
                return Ok(new {error = true, message = error.Message});
            }
        }

        [HttpPost("Add()")]
        public async Task<IActionResult> AddNewComment([FromBody] AddCommentForm form)
        {
            try
            {
                var req = new AddComment(form);
                await _mediator.Send(req);
                return Ok(new {error = false, message = "Comment added"});
            }
            catch (Exception error)
            {
                return Ok(new {error = true, message = error.Message});
            }
        }
    }
}