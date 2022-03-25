using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using UserManagement.Library.Models;
using UserManagement.Library.Repository;
using UserManagement.Library.Response;

namespace UserManagementApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRep _userrep;

        public UserController(IUserRep userRep)
        {
            this._userrep = userRep;
        }

        [Route("Get"), HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                GenericResponse result = await _userrep.Get(id);

                if (!result.Success)
                    return BadRequest(result.Message);

                if (result.Obj == null)
                    return NotFound();

                return Ok(result.Obj);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("GetAll"), HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                GenericResponse result = await _userrep.GetAll();

                if (!result.Success)
                    return BadRequest(result.Message);

                if (!result.ObjList.Any())
                    return NoContent();

                return Ok(result.ObjList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("Post"), HttpPost]
        public async Task<IActionResult> Post([FromBody] User user)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState.Select(x => x.Value.Errors)
                           .Where(y => y.Count > 0)
                           .ToList());

                GenericResponse resultVal = await _userrep.EmailIsValid(user.Email);

                if (!string.IsNullOrEmpty(resultVal.Message))
                    return BadRequest(resultVal.Message);

                if (!resultVal.Success)
                    return BadRequest("Invalid email, already exists.");


                GenericResponse result = await _userrep.Create(user);

                if (!result.Success)
                    return BadRequest(result.Message);

                if (result.Obj == null)
                    return NotFound();

                return Ok(result.Obj);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("Put"), HttpPut()]
        public async Task<IActionResult> Put([FromBody] User user)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState.Select(x => x.Value.Errors)
                           .Where(y => y.Count > 0)
                           .ToList());

                GenericResponse result = await _userrep.Update(user);

                if (result.Success)
                    return Ok(result.Success);
                else if (!string.IsNullOrEmpty(result.Message))
                    return BadRequest(result.Message);
                else
                    return NotFound(false);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("Delete"), HttpDelete()]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                GenericResponse result = await _userrep.Delete(id);

                if (result.Success)
                    return Ok(result.Success);
                else if (!string.IsNullOrEmpty(result.Message))
                    return BadRequest(result.Message);
                else
                    return NotFound(result.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("EmailIsValid"), HttpGet]
        public async Task<IActionResult> EmailIsValid(string email)
        {
            try
            {
                GenericResponse result = await _userrep.EmailIsValid(email);

                if (!string.IsNullOrEmpty(result.Message))
                    return Ok(result.Message);

                return Ok(result.Success);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
