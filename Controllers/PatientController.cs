using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using TheApp.Models;

namespace TheApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        SqlConnection connection = null;
        public PatientController(IConfiguration configuration)
        {
            _configuration = configuration;
            connection = new SqlConnection(_configuration.GetConnectionString("DBCS").ToString());
        }
        [Authorize]
        [HttpPatch]
        [Route("UpdatePassword")]
        public Response<string> UpdatePassword(User user)
        {
            Response<string> response;

            string sRole = User.FindFirst("Role")?.Value;
            string sId = User.FindFirst("Id")?.Value;
            if(Validate(sRole,sId))
            {
                int Id = int.Parse(sId);
                DAL dal = new DAL();
                response = dal.UpdatePatientPasswordById(Id, user, connection);
                response.Data = "Nothing to return";
                
                
            }
            else {
                response = new Response<string>();
                response.StatusCode = 100;
                response.StatusMessage = "Unauthorized";

                
            }
            return response;
        }
        [Authorize]
        [HttpPatch]
        [Route("UpdateInfo")]
        public Response<string> UpdateInfo(Info info)
        {
            Response<string> response;

            string sRole = User.FindFirst("Role")?.Value;
            string sId = User.FindFirst("Id")?.Value;
            if (Validate(sRole, sId))
            {
                int Id = int.Parse(sId);
                DAL dal = new DAL();
         
                response = dal.UpdatePatientPhoneAddressById(Id,info, connection);
                response.Data = "Nothing to return";


            }
            else
            {
                response = new Response<string>();
                response.StatusCode = 100;
                response.StatusMessage = "Unauthorized";


            }
            return response;
        }

        [Authorize]
        [HttpPatch]
        [Route("CancelAppointment/{AppointmentId}")]
        public Response<string> CancelAppointment(int AppointmentId)
        {
            Response<string> response;

            string sRole = User.FindFirst("Role")?.Value;
            string sId = User.FindFirst("Id")?.Value;
            if (Validate(sRole, sId))
            {
                int Id = int.Parse(sId);
                DAL dal = new DAL();
                
                response = dal.DeleteAppointmentByPatientId(Id,AppointmentId, connection);
                response.Data = "Nothing to return";


            }
            else
            {
                response = new Response<string>();
                response.StatusCode = 100;
                response.StatusMessage = "Unauthorized";


            }
            return response;
        }

        [Authorize]
        [HttpPatch]
        [Route("BookAppointment/{AppointmentId}")]
        public Response<string> BookAppointment(int AppointmentId)
        {
            Response<string> response;

            string sRole = User.FindFirst("Role")?.Value;
            string sId = User.FindFirst("Id")?.Value;
            if (Validate(sRole, sId))
            {
                int Id = int.Parse(sId);
                DAL dal = new DAL();

                response = dal.PostAppointmentByPatientId(Id, AppointmentId, connection);
                response.Data = "Nothing to return";


            }
            else
            {
                response = new Response<string>();
                response.StatusCode = 100;
                response.StatusMessage = "Unauthorized";


            }
            return response;
        }

        [AllowAnonymous]
        [HttpPatch]
        [Route("BookAdminAppointment")]
        public Response<string> BookAdminAppointment(Temp temp)
        {
            Response<string> response;

            DAL dal = new DAL();

            response = dal.PostAppointmentByPatientId(temp.Id, temp.AppointmentId, connection);
            response.Data = "Nothing to return";

            return response;
        }


        [Authorize]
        [HttpGet]
        [Route("MyAppointments")]
        public Response<List<Appointment>> GetMyAppointments()
        {
            Response<List<Appointment>> response;

            string sRole = User.FindFirst("Role")?.Value;
            string sId = User.FindFirst("Id")?.Value;
            if (Validate(sRole, sId))
            {
                int Id = int.Parse(sId);
                DAL dal = new DAL();

                response = dal.GetAppointmentByPatientId(Id, connection);
                


            }
            else
            {
                response = new Response<List<Appointment>>();
                response.StatusCode = 100;
                response.StatusMessage = "Unauthorized";


            }
            return response;
        }

        [Authorize]
        [HttpPost]
        [Route("SearchAppointment")]
        public Response<List<Appointment>> SearchAppointment(Clinic clinic)
        {
            Response<List<Appointment>> response;

            string sRole = User.FindFirst("Role")?.Value;
            string sId = User.FindFirst("Id")?.Value;
            if (Validate(sRole, sId))
            {
                
                DAL dal = new DAL();

                response = dal.GetAppointmentBySearch(clinic, connection);



            }
            else
            {
                response = new Response<List<Appointment>>();
                response.StatusCode = 100;
                response.StatusMessage = "Unauthorized";


            }
            return response;
        }


        private bool Validate(string sRole,string sId)
        {
            if (sRole == null || sId == null) return false;
            if (int.Parse(sRole) == 0) return true;
            return false;

        }
    }
}
