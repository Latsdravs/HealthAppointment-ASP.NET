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
        [Authorize]
        [HttpGet]
        [Route("DoctorAppointments/{DoctorId}")]
        public Response<List<Appointment>> GetDoctorAppointment(int DoctorId)
        {
            Response<List<Appointment>> response;

            string sRole = User.FindFirst("Role")?.Value;
            string sId = User.FindFirst("Id")?.Value;
            if (Validate(sRole, sId))
            {
                int Id = int.Parse(sId);
                DAL dal = new DAL();

                response = dal.GetAppointmentByDoctorId(DoctorId, connection);



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
        [HttpGet]
        [Route("PatientAppointments/{PatientId}")]
        public Response<List<Appointment>> GetPatientAppointments(int PatientId)
        {
            Response<List<Appointment>> response;

            string sRole = User.FindFirst("Role")?.Value;
            string sId = User.FindFirst("Id")?.Value;
            if (Validate(sRole, sId))
            {
                int Id = int.Parse(sId);
                DAL dal = new DAL();

                response = dal.GetAppointmentByPatientId(PatientId, connection);



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
        [HttpDelete]
        [Route("DeleteDoctor/{DoctorId}")]
        public Response<string> DeleteDoctor(int DoctorId)
        {
            Response<string> response;

            string sRole = User.FindFirst("Role")?.Value;
            string sId = User.FindFirst("Id")?.Value;
            if (Validate(sRole, sId))
            {
                int Id = int.Parse(sId);
                DAL dal = new DAL();

                response = dal.DeleteDoctorById(DoctorId, connection);
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
        [HttpDelete]
        [Route("DeletePatient/{PatientId}")]
        public Response<string> DeletePatient(int PatientId)
        {
            Response<string> response;

            string sRole = User.FindFirst("Role")?.Value;
            string sId = User.FindFirst("Id")?.Value;
            if (Validate(sRole, sId))
            {
                int Id = int.Parse(sId);
                DAL dal = new DAL();

                response = dal.DeletePatientById(PatientId, connection);
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
        [HttpGet]
        [Route("GetDoctor/{DoctorId}")]
        public Response<Doctor> GetDoctorById(int DoctorId)
        {
            Response<Doctor> response;

            string sRole = User.FindFirst("Role")?.Value;
            string sId = User.FindFirst("Id")?.Value;
            if (Validate(sRole, sId))
            {
                int Id = int.Parse(sId);
                DAL dal = new DAL();

                response = dal.GetDoctorById(DoctorId, connection);



            }
            else
            {
                response = new Response<Doctor>();
                response.StatusCode = 100;
                response.StatusMessage = "Unauthorized";


            }
            return response;
        }
        [Authorize]
        [HttpGet]
        [Route("GetPatient/{PatientId}")]
        public Response<Patient> GetPatientById(int PatientId)
        {
            Response<Patient> response;

            string sRole = User.FindFirst("Role")?.Value;
            string sId = User.FindFirst("Id")?.Value;
            if (Validate(sRole, sId))
            {
                int Id = int.Parse(sId);
                DAL dal = new DAL();

                response = dal.GetPatientById(PatientId, connection);



            }
            else
            {
                response = new Response<Patient>();
                response.StatusCode = 100;
                response.StatusMessage = "Unauthorized";


            }
            return response;
        }
        [Authorize]
        [HttpGet]
        [Route("GetDoctor")]
        public Response<List<Doctor>> GetDoctor()
        {
            Response<List<Doctor>> response;

            string sRole = User.FindFirst("Role")?.Value;
            string sId = User.FindFirst("Id")?.Value;
            if (Validate(sRole, sId))
            {
                int Id = int.Parse(sId);
                DAL dal = new DAL();

                response = dal.GetDoctor( connection);



            }
            else
            {
                response = new Response<List<Doctor>>();
                response.StatusCode = 100;
                response.StatusMessage = "Unauthorized";


            }
            return response;
        }

        [Authorize]
        [HttpGet]
        [Route("GetPatient")]
        public Response<List<Patient>> GetPatient()
        {
            Response<List<Patient>> response;

            string sRole = User.FindFirst("Role")?.Value;
            string sId = User.FindFirst("Id")?.Value;
            if (Validate(sRole, sId))
            {
                int Id = int.Parse(sId);
                DAL dal = new DAL();

                response = dal.GetPatient( connection);



            }
            else
            {
                response = new Response<List<Patient>>();
                response.StatusCode = 100;
                response.StatusMessage = "Unauthorized";


            }
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
