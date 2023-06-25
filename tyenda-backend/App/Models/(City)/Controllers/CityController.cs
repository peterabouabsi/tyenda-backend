using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using tyenda_backend.App.Base_Controllers;
using tyenda_backend.App.Models._City_.Services._GetCountryCities_;
using tyenda_backend.App.Models._City_.Views;

namespace tyenda_backend.App.Models._City_.Controllers
{
    public class CityController : AnonymousController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public CityController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet("{countryId}")]
        public async Task<IActionResult> GetCountryCities([FromRoute] string countryId)
        {
            try
            {
                var req = new GetCountryCities(countryId);
                var res = await _mediator.Send(req);
                var result = _mapper.Map<ICollection<BasicCityView>>(res);
                return Ok(result);
            }
            catch (Exception error)
            {
                return Ok(new {error = true, message = error.Message});
            }
        }
    }
}