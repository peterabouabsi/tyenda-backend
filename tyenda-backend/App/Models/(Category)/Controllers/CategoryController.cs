using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using tyenda_backend.App.Base_Controllers;
using tyenda_backend.App.Models._Category_.Services._GetCategories_;
using tyenda_backend.App.Models._City_.Views;

namespace tyenda_backend.App.Models._Category_.Controllers
{
    public class CategoryController : AnonymousController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public CategoryController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet()]
        public async Task<IActionResult> GetAllCategories()
        {
            try
            {
                var req = new GetCategories();
                var res = await _mediator.Send(req);
                var result = _mapper.Map<ICollection<BasicCategoryView>>(res);
                return Ok(result);
            }
            catch (Exception error)
            {
                return Ok(new {error = true, message = error.Message});
            }
        }

    }
}