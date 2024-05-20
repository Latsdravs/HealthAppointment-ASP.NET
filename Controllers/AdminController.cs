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
        [HttpPatch]
        [Route("CancelAppointment/{AppointmentId}")]
        public Response<string> CancelAppointment(int AppointmentId)
        {
            Response<string> response;

            string sRole = User.FindFirst("Role")?.Value;
            string sId = User.FindFirst("Id")?.Value;
            if (Validate(sRole, sId))
            {
                
                DAL dal = new DAL();
                response = new Response<string>();

                Response<int> wrap = dal.GetPatientIdByAppointmentId(AppointmentId, connection);
                int Id = wrap.Data;
                response.StatusCode = wrap.StatusCode;
                response.StatusMessage = wrap.StatusMessage;
                if (wrap.StatusCode == 100) return response;


                response = dal.DeleteAppointmentByPatientId(Id, AppointmentId, connection);
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

        [Authorize]
        [HttpGet]
        [Route("Records/{Id}")]
        public Response<List<string>> GetRecords(int Id)
        {
            Response<List<string>> response;

            string sRole = User.FindFirst("Role")?.Value;
            string sId = User.FindFirst("Id")?.Value;
            if (Validate(sRole, sId))
            {
                
                DAL dal = new DAL();

                response = dal.GetRecordNameByPatientId(Id, connection);




            }
            else
            {
                response = new Response<List<string>>();
                response.StatusCode = 100;
                response.StatusMessage = "Unauthorized";


            }
            return response;
        }

        [Authorize]
        [HttpPost]
        [Route("GetRecord/{Title}/{Id}")]
        public Response<byte[]> GetRecord(string Title,int Id)
        {
            Response<byte[]> response;

            string sRole = User.FindFirst("Role")?.Value;
            string sId = User.FindFirst("Id")?.Value;
            if (Validate(sRole, sId))
            {

                DAL dal = new DAL();
                
                response = new Response<byte[]>();


                Response<Record> temp = dal.GetRecordByPatientIdAndTitle(Id, Title, connection);
                response.StatusMessage = temp.StatusMessage + " " + temp.Data.Date.ToString();
                response.StatusCode = temp.StatusCode;
                var path = Path.Combine(Directory.GetCurrentDirectory(), "uploads", Id.ToString(), temp.Data.Title);

                if (!System.IO.File.Exists(path))
                {
                    response.StatusCode = 404;
                    response.StatusMessage = "File not found Sub message:" + response.StatusMessage;
                    return response;
                }

                try
                {
                    var fileBytes = System.IO.File.ReadAllBytes(path);
                    response.StatusCode = 200;
                    response.StatusMessage = "File downloaded successfully Sub message:" + response.StatusMessage;
                    response.Data = fileBytes;

                    // If you want to include the file name in the headers, consider returning a file response along with your custom response
                    Response.Headers.Add("Content-Disposition", new System.Net.Mime.ContentDisposition
                    {
                        FileName = temp.Data.Title,
                        Inline = false
                    }.ToString());
                }
                catch (Exception ex)
                {
                    response.StatusCode = 500;
                    response.StatusMessage = $"Internal server error: {ex.Message} Sub message:" + response.StatusMessage;
                }

            }
            else
            {
                response = new Response<byte[]>();
                response.StatusCode = 100;
                response.StatusMessage = "Unauthorized";


            }
            return response;
        }

        [Authorize]
        [HttpPost]
        [Route("PostRecord/{Title}/{Id}")]
        public async Task<Response<string>> PostRecord(string Title, int Id, [FromForm] IFormFile file)
        {
            Response<string> response;

            string sRole = User.FindFirst("Role")?.Value;
            string sId = User.FindFirst("Id")?.Value;
            if (Validate(sRole, sId))
            {

                DAL dal = new DAL();
                
                response = new Response<string>();
                Record record = new Record();
                var path = Path.Combine(Directory.GetCurrentDirectory(), "uploads", Id.ToString());
                record.Title = Title;
                record.Path = path;
                record.PatientId = Id;

                response = dal.PostRecord(record, connection);

                if (file == null || file.Length == 0)
                {
                    response.StatusCode = 400;
                    response.StatusMessage = "No file uploaded";
                    return response;
                }

                path = Path.Combine(path, Title);

                try
                {
                    if (!Directory.Exists(record.Path))
                    {
                        Directory.CreateDirectory(record.Path);
                    }
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    // Deserialize metadata if necessary
                    /*
                    FileMetadata fileMetadata = null;
                    if (!string.IsNullOrEmpty(metadata))
                    {
                        fileMetadata = JsonSerializer.Deserialize<FileMetadata>(metadata);
                        // Use metadata as needed
                    }*/

                    response.StatusCode = 200;
                    response.StatusMessage = "File uploaded successfully";
                    response.Data = path; // or some relevant file identifier
                }
                catch (Exception ex)
                {
                    response.StatusCode = 500;
                    response.StatusMessage = $"Internal server error: {ex.Message}";
                }

                return response;



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
        [HttpPost]
        [Route("DeleteRecord/{Title}/{Id}")]
        public Response<string> DeleteRecord(string Title, int Id)
        {
            Response<string> response;

            string sRole = User.FindFirst("Role")?.Value;
            string sId = User.FindFirst("Id")?.Value;
            if (Validate(sRole, sId))
            {

                DAL dal = new DAL();
                
                response = new Response<string>();


                response = dal.DeleteRecord(Id, Title, connection);

                var path = Path.Combine(Directory.GetCurrentDirectory(), "uploads", Id.ToString(), Title);

                if (!System.IO.File.Exists(path))
                {
                    response.StatusCode = 404;
                    response.StatusMessage = "File not found Sub message:" + response.StatusMessage;
                    return response;
                }

                try
                {
                    System.IO.File.Delete(path);
                    response.StatusCode = 200;
                    response.StatusMessage = "File downloaded successfully Sub message:" + response.StatusMessage;



                }
                catch (Exception ex)
                {
                    response.StatusCode = 500;
                    response.StatusMessage = $"Internal server error: {ex.Message} Sub message:" + response.StatusMessage;
                }

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
        [Route("Records/{AppointmentId}")]
        public Response<List<string>> GetRecordsByAppointmentId(int AppointmentId)
        {
            Response<List<string>> response;

            string sRole = User.FindFirst("Role")?.Value;
            string sId = User.FindFirst("Id")?.Value;
            if (Validate(sRole, sId))
            {

                DAL dal = new DAL();

                response = new Response<List<string>>();

                Response<int> wrap = dal.GetPatientIdByAppointmentId(AppointmentId, connection);
                int Id = wrap.Data;
                response.StatusCode = wrap.StatusCode;
                response.StatusMessage = wrap.StatusMessage;
                if (wrap.StatusCode == 100) return response;

                response = dal.GetRecordNameByPatientId(Id, connection);




            }
            else
            {
                response = new Response<List<string>>();
                response.StatusCode = 100;
                response.StatusMessage = "Unauthorized";


            }
            return response;
        }
        [Authorize]
        [HttpPost]
        [Route("GetRecordByAppointmentId/{Title}/{AppointmentId}")]
        public Response<byte[]> GetRecordByAppointmentId(string Title, int AppointmentId)
        {
            Response<byte[]> response;

            string sRole = User.FindFirst("Role")?.Value;
            string sId = User.FindFirst("Id")?.Value;
            if (Validate(sRole, sId))
            {

                DAL dal = new DAL();

                response = new Response<byte[]>();
                Response<int> wrap = dal.GetPatientIdByAppointmentId(AppointmentId, connection);
                int Id = wrap.Data;
                response.StatusCode = wrap.StatusCode;
                response.StatusMessage = wrap.StatusMessage;
                if (wrap.StatusCode == 100) return response;

                Response<Record> temp = dal.GetRecordByPatientIdAndTitle(Id, Title, connection);
                response.StatusMessage = temp.StatusMessage + " " + temp.Data.Date.ToString();
                response.StatusCode = temp.StatusCode;
                var path = Path.Combine(Directory.GetCurrentDirectory(), "uploads", Id.ToString(), temp.Data.Title);

                if (!System.IO.File.Exists(path))
                {
                    response.StatusCode = 404;
                    response.StatusMessage = "File not found Sub message:" + response.StatusMessage;
                    return response;
                }

                try
                {
                    var fileBytes = System.IO.File.ReadAllBytes(path);
                    response.StatusCode = 200;
                    response.StatusMessage = "File downloaded successfully Sub message:" + response.StatusMessage;
                    response.Data = fileBytes;

                    // If you want to include the file name in the headers, consider returning a file response along with your custom response
                    Response.Headers.Add("Content-Disposition", new System.Net.Mime.ContentDisposition
                    {
                        FileName = temp.Data.Title,
                        Inline = false
                    }.ToString());
                }
                catch (Exception ex)
                {
                    response.StatusCode = 500;
                    response.StatusMessage = $"Internal server error: {ex.Message} Sub message:" + response.StatusMessage;
                }

            }
            else
            {
                response = new Response<byte[]>();
                response.StatusCode = 100;
                response.StatusMessage = "Unauthorized";


            }
            return response;
        }

        [Authorize]
        [HttpPost]
        [Route("PostRecordByAppointmentId/{Title}/{AppointmentId}")]
        public async Task<Response<string>> PostRecordByAppointmentId(string Title, int AppointmentId, [FromForm] IFormFile file)
        {
            Response<string> response;

            string sRole = User.FindFirst("Role")?.Value;
            string sId = User.FindFirst("Id")?.Value;
            if (Validate(sRole, sId))
            {

                DAL dal = new DAL();

                response = new Response<string>();

                Response<int> wrap = dal.GetPatientIdByAppointmentId(AppointmentId, connection);
                int Id = wrap.Data;
                response.StatusCode = wrap.StatusCode;
                response.StatusMessage = wrap.StatusMessage;
                if (wrap.StatusCode == 100) return response;

                Record record = new Record();
                var path = Path.Combine(Directory.GetCurrentDirectory(), "uploads", Id.ToString());
                record.Title = Title;
                record.Path = path;
                record.PatientId = Id;

                response = dal.PostRecord(record, connection);

                if (file == null || file.Length == 0)
                {
                    response.StatusCode = 400;
                    response.StatusMessage = "No file uploaded";
                    return response;
                }

                path = Path.Combine(path, Title);

                try
                {
                    if (!Directory.Exists(record.Path))
                    {
                        Directory.CreateDirectory(record.Path);
                    }
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    // Deserialize metadata if necessary
                    /*
                    FileMetadata fileMetadata = null;
                    if (!string.IsNullOrEmpty(metadata))
                    {
                        fileMetadata = JsonSerializer.Deserialize<FileMetadata>(metadata);
                        // Use metadata as needed
                    }*/

                    response.StatusCode = 200;
                    response.StatusMessage = "File uploaded successfully";
                    response.Data = path; // or some relevant file identifier
                }
                catch (Exception ex)
                {
                    response.StatusCode = 500;
                    response.StatusMessage = $"Internal server error: {ex.Message}";
                }

                return response;



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
        [HttpPost]
        [Route("DeleteRecordByAppointmentId/{Title}/{AppointmentId}")]
        public Response<string> DeleteRecordByAppointmentId(string Title, int AppointmentId)
        {
            Response<string> response;

            string sRole = User.FindFirst("Role")?.Value;
            string sId = User.FindFirst("Id")?.Value;
            if (Validate(sRole, sId))
            {

                DAL dal = new DAL();

                response = new Response<string>();

                Response<int> wrap = dal.GetPatientIdByAppointmentId(AppointmentId, connection);
                int Id = wrap.Data;
                response.StatusCode = wrap.StatusCode;
                response.StatusMessage = wrap.StatusMessage;
                if (wrap.StatusCode == 100) return response;

                response = dal.DeleteRecord(Id, Title, connection);

                var path = Path.Combine(Directory.GetCurrentDirectory(), "uploads", Id.ToString(), Title);

                if (!System.IO.File.Exists(path))
                {
                    response.StatusCode = 404;
                    response.StatusMessage = "File not found Sub message:" + response.StatusMessage;
                    return response;
                }

                try
                {
                    System.IO.File.Delete(path);
                    response.StatusCode = 200;
                    response.StatusMessage = "File downloaded successfully Sub message:" + response.StatusMessage;



                }
                catch (Exception ex)
                {
                    response.StatusCode = 500;
                    response.StatusMessage = $"Internal server error: {ex.Message} Sub message:" + response.StatusMessage;
                }

            }
            else
            {
                response = new Response<string>();
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
