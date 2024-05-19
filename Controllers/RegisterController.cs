using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using TheApp.Models;

namespace TheApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        SqlConnection connection = null;
        public RegisterController(IConfiguration configuration)
        {
            _configuration = configuration;
            connection = new SqlConnection( _configuration.GetConnectionString("DBCS").ToString());
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("Patient")]
        public Response<string> RegisterPatient(Patient patient)
        {
            
            DAL dal = new DAL();
            Response<string> response = dal.RegisterPatient(patient,connection);
            response.Data = "No Data To Return";
            return response;
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("Doctor")]
        public Response<string> RegisterDoctor(Doctor doctor)
        {

            DAL dal = new DAL();
            Response<string> response = dal.RegisterDoctor(doctor, connection);
            response.Data = "No Data To Return";
            return response;
        }
    }
}
