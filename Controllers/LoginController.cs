using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Product_api.Data;
using Product_api.model;

namespace Loginapi.Controller{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController:ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;
        public LoginController(AppDbContext context,IConfiguration config)
        {
                _context=context;
                _config=config;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        private User AuthenticateUser(User user)
        {
            User? _user=null;
            var Userfound= _context.Users.FirstOrDefault(x=>x.UserName==user.UserName && x.Password==user.Password);
            if(Userfound!=null)
            {
                    _user=Userfound;
            }
            return _user;
            
        }
        private string GenerateToken(User user)
        {
            var securitykey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials=new SigningCredentials(securitykey,SecurityAlgorithms.HmacSha256);
            var Token=new JwtSecurityToken(_config["Jwt:Issuer"],_config["Jwt:Audiance"],expires:DateTime.Now.AddMinutes(1),signingCredentials:credentials);
            return new JwtSecurityTokenHandler().WriteToken(Token);
        }
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login(User user)
        {
            IActionResult response=Unauthorized();
            var _user=AuthenticateUser(user);
            if(_user!=null)
            {
                var token=GenerateToken(_user);
                response=Ok(new {token=token});

            }
            return response;
        }
    }
}