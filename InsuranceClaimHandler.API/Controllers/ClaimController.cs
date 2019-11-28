using ClaimManagement.Application.Claims.Commands;
using ClaimManagement.Application.Claims.Queries;
using ClaimManagement.Application.Common.Exceptions;
using ClaimManagement.Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace InsuranceClaimHandler.WriteAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ClaimController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ClaimController> _logger;

        public ClaimController(IMediator mediator, ILogger<ClaimController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [Microsoft.AspNetCore.Cors.EnableCors("ClaimApiAccessPolicy")]
        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> Add(AddClaimCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }

        [Microsoft.AspNetCore.Cors.EnableCors("ClaimApiAccessPolicy")]
        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _mediator.Send(new GetClaimsQuery()));
        }

        [Microsoft.AspNetCore.Cors.EnableCors("ClaimApiAccessPolicy")]
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {

            var getClaimByIdQuery = new GetClaimByIdQuery() { Id = id.ToString() };
            return this.Ok(await _mediator.Send(getClaimByIdQuery));

        }

        [Microsoft.AspNetCore.Cors.EnableCors("ClaimApiAccessPolicy")]
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Remove(int id)
        {

            var deleteCommand = new DeleteClaimCommand() { Id = id.ToString() };

            await _mediator.Send(deleteCommand);

            return Ok(true);

        }

        [Microsoft.AspNetCore.Cors.EnableCors("ClaimApiAccessPolicy")]
        [HttpGet]
        [Route("types")]
        public IActionResult GetClaimTypes()
        {

            return Ok(EnumHelper.GetClaimTypeEnumReadableValues());

        }

    }
}