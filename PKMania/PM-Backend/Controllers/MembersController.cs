using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PM_BLL.Data.DTO.Forms;
using PM_BLL.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace PM_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembersController : ControllerBase
    {
        private readonly IMembersService _membersService;

        public MembersController(IMembersService membersService)
        {
            _membersService = membersService;
        }

        [HttpPost]
        public IActionResult Post(MemberRegisterFormDTO member)
        {
            try
            {
                this._membersService.AddMember(member);
                return NoContent();
            }catch(ValidationException ve)
            {
                return BadRequest(ve.Message);
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
