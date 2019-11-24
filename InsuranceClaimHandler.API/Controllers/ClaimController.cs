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
            try
            {
                await _mediator.Send(command);
                return Ok();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, "There is something wrong, try again in few moments, or contact our support team.");
            }
        }

        [Microsoft.AspNetCore.Cors.EnableCors("ClaimApiAccessPolicy")]
        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                return Ok(await _mediator.Send(new GetClaimsQuery()));
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, "There is something wrong, try again in few moments, or contact our support team.");
            }
        }

        [Microsoft.AspNetCore.Cors.EnableCors("ClaimApiAccessPolicy")]
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var getClaimByIdQuery = new GetClaimByIdQuery() { Id = id.ToString() };
                return this.Ok(await _mediator.Send(getClaimByIdQuery));
            }
            catch (NotFoundException notFoundException)
            {
                _logger.LogError(notFoundException.ToString());
                return StatusCode(StatusCodes.Status404NotFound, notFoundException.Message);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, "There is something wrong, try again in few moments, or contact our support team.");
            }
        }

        [Microsoft.AspNetCore.Cors.EnableCors("ClaimApiAccessPolicy")]
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Remove(int id)
        {
            try
            {
                var deleteCommand = new DeleteClaimCommand() { Id = id.ToString() };

                await _mediator.Send(deleteCommand);

                return Ok(true);
            }
            catch (NotFoundException notFoundException)
            {
                _logger.LogError(notFoundException.ToString());
                return StatusCode(StatusCodes.Status404NotFound, notFoundException.Message);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, "There is something wrong, try again in few moments, or contact our support team.");
            }
        }

        [Microsoft.AspNetCore.Cors.EnableCors("ClaimApiAccessPolicy")]
        [HttpGet]
        [Route("types")]
        public IActionResult GetClaimTypes()
        {
            try
            {
                return Ok(EnumHelper.GetClaimTypeEnumReadableValues());
            }
            catch (NotFoundException notFoundException)
            {
                _logger.LogError(notFoundException.ToString());
                return StatusCode(StatusCodes.Status404NotFound, notFoundException.Message);
            }
        }

    }
}