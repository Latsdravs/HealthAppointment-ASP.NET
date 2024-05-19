using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using TheApp.Models;

namespace TheApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        SqlConnection connection = null;
        public AdminController(IConfiguration configuration)
        {
            _configuration = configuration;
            connection = new SqlConnection(_configuration.GetConnectionString("DBCS").ToString());
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("PostAppointment")]
        public Response<string> PostAppointment(AppointmentRaw appointment)
        {
            Response<string> response;

            DAL dal = new DAL();

            response = dal.PostAppointment(appointment.DateTime, appointment.DoctorId, connection);

            return response;
        }



        private bool Validate(string sRole, string sId)
        {
            if (sRole == null || sId == null) return false;
            if (int.Parse(sRole) == 2) return true;
            return false;

        }
    }
}
