using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using TheApp.Models;

namespace TheApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        SqlConnection connection = null;
        public DoctorController(IConfiguration configuration)
        {
            _configuration = configuration;
            connection = new SqlConnection(_configuration.GetConnectionString("DBCS").ToString());
        }

        [Authorize]
        [HttpGet]
        [Route("MyAppointments")]
        public Response<List<Appointment>> GetMyAppointment()
        {
            Response<List<Appointment>> response;

            string sRole = User.FindFirst("Role")?.Value;
            string sId = User.FindFirst("Id")?.Value;
            if (Validate(sRole, sId))
            {
                int Id = int.Parse(sId);
                DAL dal = new DAL();

                response = dal.GetAppointmentByDoctorId(Id, connection);

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
        [Route("Records/{AppointmentId}")]
        public Response<List<string>> GetRecords(int AppointmentId)
        {
            Response<List<string>> response;

            string sRole = User.FindFirst("Role")?.Value;
            string sId = User.FindFirst("Id")?.Value;
            if (Validate(sRole, sId))
            {

                DAL dal = new DAL();

                response= new Response<List<string>>();

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
        [Route("GetRecord/{Title}/{AppointmentId}")]
        public Response<byte[]> GetRecord(string Title, int AppointmentId)
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
                if(wrap.StatusCode == 100)return response;

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
        [Route("PostRecord/{Title}/{AppointmentId}")]
        public async Task<Response<string>> PostRecord(string Title, int AppointmentId, [FromForm] IFormFile file)
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
        [Route("DeleteRecord/{Title}/{AppointmentId}")]
        public Response<string> DeleteRecord(string Title, int AppointmentId)
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
            if (int.Parse(sRole) == 1) return true;
            return false;

        }

    }
}
