using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using TheApp.Models;

namespace TheApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        SqlConnection connection = null;
        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
            connection = new SqlConnection(_configuration.GetConnectionString("DBCS").ToString());
        }
        
        [AllowAnonymous]
        [HttpPost]
        [Route("Patient")]
        public Response<Patient> LoginP(User user)
        {
            
            Response<Patient> response;
            
            DAL dal = new DAL();
            response = dal.LoginPatient(user, connection);
            if(response.Data != null)
            {
                response.token = GenerateToken(response.Data.Id, 0);
                response.Data.Id = -1;
            }
            
            


            return response;
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("Doctor")]
        public Response<Doctor> LoginD(User user)
        {

            Response<Doctor> response;

            DAL dal = new DAL();
            response = dal.LoginDoctor(user, connection);
            if (response.Data != null)
            {
                response.token = GenerateToken(response.Data.Id, 1);
                response.Data.Id = -1;
            }

            


            return response;
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("Admin")]
        public Response<Admin> LoginA(User user)
        {

            Response<Admin> response;

            DAL dal = new DAL();
            if(user.Username == "Alper" &&  user.Password == "951753")
            {
                response = new Response<Admin>();
                response.Data = new Admin();
                response.Data.FirstName = "Alper";
                response.Data.LastName = "Kömürcü";
                response.Data.Id = 1;
                response.StatusMessage = "OK";
                response.StatusCode = 200;
            }
            else
            {
                response = new Response<Admin>();
                response.StatusMessage = "Username or Password incorrect";
                response.StatusCode = 100;
            }
            if (response.Data != null)
            {
                response.token = GenerateToken(response.Data.Id, 2);
                response.Data.Id = -1;
            }




            return response;
        }

        private string GenerateToken(int Id , int role)
        {
            string sId=Id.ToString();
            string srole=role.ToString();
            var claims = new List<Claim>
            {
                new Claim("Role", srole),
                new Claim("Id", sId)
                // Add more claims as needed
            };
            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials =new SigningCredentials(securitykey,SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"], claims:claims,
            expires: DateTime.Now.AddMinutes(600), // Token expiration time
            signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
            
        }
    }
}
