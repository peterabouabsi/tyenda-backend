using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using tyenda_backend.App.Base_Controllers;
using tyenda_backend.App.Models._Country_.Services._Get_Countries_;
using tyenda_backend.App.Models._Country_.Views;

namespace tyenda_backend.App.Models._Country_.Controllers
{
    public class CountryController : AnonymousController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public CountryController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetCountries()
        {
            try
            {
                var req = new GetCountries();
                var res = await _mediator.Send(req);
                var result = _mapper.Map<ICollection<BasicCountryView>>(res);
                return Ok(result);
            }
            catch (Exception error)
            {
                return Ok(new {error = true, message = error.Message});
            }
        }
    }
}