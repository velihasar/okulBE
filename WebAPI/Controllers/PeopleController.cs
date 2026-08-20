
using Business.Handlers.People.Commands;
using Business.Handlers.People.Queries;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Entities.Concrete;
using System.Collections.Generic;
using Core.Entities.Concrete.Project;
using Core.Entities.Dtos.PersonDto;

namespace WebAPI.Controllers
{
    /// <summary>
    /// People If controller methods will not be Authorize, [AllowAnonymous] is used.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController : BaseApiController
    {
        ///<summary>
        ///List People
        ///</summary>
        ///<remarks>People</remarks>
        ///<return>List People</return>
        ///<response code="200"></response>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<PersonGetAllDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpGet("getall")]
        public async Task<IActionResult> GetList()
        {
            var result = await Mediator.Send(new GetPeopleQuery());
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }

        ///<summary>
        ///It brings the details according to its id.
        ///</summary>
        ///<remarks>People</remarks>
        ///<return>People List</return>
        ///<response code="200"></response>  
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PersonGetByIdDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpGet("getbyid")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await Mediator.Send(new GetPersonQuery { Id = id });
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }

        /// <summary>
        /// Add Person.
        /// </summary>
        /// <param name="createPerson"></param>
        /// <returns></returns>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PersonCreateResponseDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreatePersonCommand createPerson)
        {
            var result = await Mediator.Send(createPerson);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        /// <summary>
        /// Update Person.
        /// </summary>
        /// <param name="updatePerson"></param>
        /// <returns></returns>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PersonUpdateResponseDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdatePersonCommand updatePerson)
        {
            var result = await Mediator.Send(updatePerson);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        /// <summary>
        /// Delete Person.
        /// </summary>
        /// <param name="deletePerson"></param>
        /// <returns></returns>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] DeletePersonCommand deletePerson)
        {
            var result = await Mediator.Send(deletePerson);
            if (result.Success)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }
    }
}
