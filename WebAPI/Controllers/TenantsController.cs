
using Business.Handlers.Tenants.Commands;
using Business.Handlers.Tenants.Queries;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Entities.Concrete;
using System.Collections.Generic;
using Core.Entities.Concrete.Project;
using Core.Entities.Dtos.TenantDto;

namespace WebAPI.Controllers
{
    /// <summary>
    /// Tenants If controller methods will not be Authorize, [AllowAnonymous] is used.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TenantsController : BaseApiController
    {
        ///<summary>
        ///List Tenants
        ///</summary>
        ///<remarks>Tenants</remarks>
        ///<return>List Tenants</return>
        ///<response code="200"></response>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TenantGetAllDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpGet("getall")]
        public async Task<IActionResult> GetList()
        {
            var result = await Mediator.Send(new GetTenantsQuery());
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }

        ///<summary>
        ///It brings the details according to its id.
        ///</summary>
        ///<remarks>Tenants</remarks>
        ///<return>Tenants List</return>
        ///<response code="200"></response>  
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TenantGetByIdDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpGet("getbyid")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await Mediator.Send(new GetTenantQuery { Id = id });
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }

        /// <summary>
        /// Add Tenant.
        /// </summary>
        /// <param name="createTenant"></param>
        /// <returns></returns>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TenantCreateResponseDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateTenantCommand createTenant)
        {
            var result = await Mediator.Send(createTenant);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        /// <summary>
        /// Update Tenant.
        /// </summary>
        /// <param name="updateTenant"></param>
        /// <returns></returns>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TenantUpdateResponseDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateTenantCommand updateTenant)
        {
            var result = await Mediator.Send(updateTenant);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        /// <summary>
        /// Delete Tenant.
        /// </summary>
        /// <param name="deleteTenant"></param>
        /// <returns></returns>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] DeleteTenantCommand deleteTenant)
        {
            var result = await Mediator.Send(deleteTenant);
            if (result.Success)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }
    }
}
