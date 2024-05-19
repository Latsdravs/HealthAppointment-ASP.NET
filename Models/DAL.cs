using System.Data;
using System.Data.SqlClient;

namespace TheApp.Models
{
    public class DAL
    {
        public Response<string> RegisterPatient(Patient patient,SqlConnection connection)
        {
            Response<string> response = new Response<string>();
            try
            {
                SqlCommand cmd = new SqlCommand("PatientRegister", connection);
                cmd.CommandType=System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Username", patient.UserInfo.Username);
                cmd.Parameters.AddWithValue("@Password", patient.UserInfo.Password);
                cmd.Parameters.AddWithValue("@FirstName", patient.FirstName);
                cmd.Parameters.AddWithValue("@LastName", patient.LastName);

                cmd.Parameters.AddWithValue("@Gender", patient.Gender);
                cmd.Parameters.AddWithValue("@Address", patient.Info.Address);
                cmd.Parameters.AddWithValue("@Phone", patient.Info.Phone);
                cmd.Parameters.AddWithValue("@Birth", patient.BirthDay);
                cmd.Parameters.Add("@ErrorMessage", System.Data.SqlDbType.Char, 200);
                cmd.Parameters["@ErrorMessage"].Direction = System.Data.ParameterDirection.Output;
                connection.Open();
                int i = cmd.ExecuteNonQuery();
                connection.Close();
                string message = (string)cmd.Parameters["@ErrorMessage"].Value;
                if(i > 0)
                {
                    response.StatusCode = 200;
                    response.StatusMessage = message;
                    
                }
                else
                {
                    response.StatusCode = 100;
                    response.StatusMessage= message;
                }

            }
            catch(Exception ex)
            {
                response.StatusCode = 100;
                response.StatusMessage = ex.Message;
            }
            return response;
        }
        public Response<Patient> LoginPatient(User user,SqlConnection connection)
        {
            Response<Patient> response = new Response<Patient>();
            try
            {
                SqlDataAdapter da = new SqlDataAdapter("PatientLogin",connection);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.AddWithValue("@Username", user.Username);
                da.SelectCommand.Parameters.AddWithValue("@Password", user.Password);
                da.SelectCommand.Parameters.Add("@ErrorMessage", System.Data.SqlDbType.Char, 200);
                da.SelectCommand.Parameters["@ErrorMessage"].Direction = System.Data.ParameterDirection.Output;
                DataTable dt = new DataTable();
                da.Fill(dt);
                string message = (string)da.SelectCommand.Parameters["@ErrorMessage"].Value;
                Patient patient = new Patient();
                if(dt.Rows.Count > 0)
                {
                    patient.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                    patient.FirstName = Convert.ToString(dt.Rows[0]["FirstName"]);
                    patient.LastName = Convert.ToString(dt.Rows[0]["LastName"]);
                    patient.Gender = Convert.ToInt32(dt.Rows[0]["Gender"]);
                    patient.BirthDay = Convert.ToDateTime(dt.Rows[0]["Birth"]);
                    patient.Info = new Info();
                    patient.Info.Address = Convert.ToString(dt.Rows[0]["Address"]);
                    patient.Info.Phone = Convert.ToString(dt.Rows[0]["Phone"]);
                    response.Data = patient;
                    response.StatusCode =200;
                    response.StatusMessage = message;
                }
                else
                {
                    response.StatusCode = 100;
                    response.StatusMessage = message;
                }
            }
            catch(Exception ex)
            {
                response.StatusCode = 100;
                response.StatusMessage = ex.Message;
            }
            return response;
        }

        public Response<string> RegisterDoctor(Doctor doctor, SqlConnection connection)
        {
            Response<string> response = new Response<string>();
            try
            {
                SqlCommand cmd = new SqlCommand("DoctorRegister", connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Username", doctor.UserInfo.Username);
                cmd.Parameters.AddWithValue("@Password", doctor.UserInfo.Password);
                cmd.Parameters.AddWithValue("@FirstName", doctor.FirstName);
                cmd.Parameters.AddWithValue("@LastName", doctor.LastName);


                cmd.Parameters.AddWithValue("@Speciality", doctor.Clinic.Speciality);
                cmd.Parameters.AddWithValue("@Hospital", doctor.Clinic.Hospital);
                cmd.Parameters.Add("@ErrorMessage", System.Data.SqlDbType.Char, 200);
                cmd.Parameters["@ErrorMessage"].Direction = System.Data.ParameterDirection.Output;
                connection.Open();
                int i = cmd.ExecuteNonQuery();
                connection.Close();
                string message = (string)cmd.Parameters["@ErrorMessage"].Value;
                if (i > 0)
                {
                    response.StatusCode = 200;
                    response.StatusMessage = message;

                }
                else
                {
                    response.StatusCode = 100;
                    response.StatusMessage = message;
                }

            }
            catch (Exception ex)
            {
                response.StatusCode = 100;
                response.StatusMessage = ex.Message;
            }
            return response;
        }
        public Response<Doctor> LoginDoctor(User user, SqlConnection connection)
        {
            Response<Doctor> response = new Response<Doctor>();
            try
            {
                SqlDataAdapter da = new SqlDataAdapter("DoctorLogin", connection);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.AddWithValue("@Username", user.Username);
                da.SelectCommand.Parameters.AddWithValue("@Password", user.Password);
                da.SelectCommand.Parameters.Add("@ErrorMessage", System.Data.SqlDbType.Char, 200);
                da.SelectCommand.Parameters["@ErrorMessage"].Direction = System.Data.ParameterDirection.Output;
                DataTable dt = new DataTable();
                da.Fill(dt);
                string message = (string)da.SelectCommand.Parameters["@ErrorMessage"].Value;
                Doctor doctor = new Doctor();
                if (dt.Rows.Count > 0)
                {
                    doctor.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                    doctor.FirstName = Convert.ToString(dt.Rows[0]["FirstName"]);
                    doctor.LastName = Convert.ToString(dt.Rows[0]["LastName"]);
                    doctor.Clinic = new Clinic();
                    doctor.Clinic.Speciality = Convert.ToString(dt.Rows[0]["Speciality"]);
                    doctor.Clinic.Hospital = Convert.ToString(dt.Rows[0]["Hospital"]);
                    response.Data = doctor;
                    response.StatusCode = 200;
                    response.StatusMessage = message;
                }
                else
                {
                    response.StatusCode = 100;
                    response.StatusMessage = message;
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 100;
                response.StatusMessage = ex.Message;
            }
            return response;
        }




        public Response<string> UpdatePatientPhoneAddressById(int patientId,Info info, SqlConnection connection)
        {
            Response<string> response = new Response<string>();
            try
            {
                SqlCommand cmd = new SqlCommand("PatientUpdatePhoneAddressById", connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", patientId);
                cmd.Parameters.AddWithValue("@Address", info.Address);
                cmd.Parameters.AddWithValue("@Phone", info.Phone);
                
                cmd.Parameters.Add("@ErrorMessage", System.Data.SqlDbType.Char, 200);
                cmd.Parameters["@ErrorMessage"].Direction = System.Data.ParameterDirection.Output;
                connection.Open();
                int i = cmd.ExecuteNonQuery();
                connection.Close();
                string message = (string)cmd.Parameters["@ErrorMessage"].Value;
                if (i > 0)
                {
                    response.StatusCode = 200;
                    response.StatusMessage = message;

                }
                else
                {
                    response.StatusCode = 100;
                    response.StatusMessage = message;
                }

            }
            catch (Exception ex)
            {
                response.StatusCode = 100;
                response.StatusMessage = ex.Message;
            }
            return response;
        }
        public Response<string> UpdatePatientPasswordById(int PatientId, User user, SqlConnection connection)
        {
            Response<string> response = new Response<string>();
            try
            {
                SqlCommand cmd = new SqlCommand("PatientUpdatePasswordById", connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", PatientId);
                cmd.Parameters.AddWithValue("@Password", user.Username);
                cmd.Parameters.AddWithValue("@PasswordNew", user.Password);

                cmd.Parameters.Add("@ErrorMessage", System.Data.SqlDbType.Char, 200);
                cmd.Parameters["@ErrorMessage"].Direction = System.Data.ParameterDirection.Output;
                connection.Open();
                int i = cmd.ExecuteNonQuery();
                connection.Close();
                string message = (string)cmd.Parameters["@ErrorMessage"].Value;
                if (i > 0)
                {
                    response.StatusCode = 200;
                    response.StatusMessage = message;

                }
                else
                {
                    response.StatusCode = 100;
                    response.StatusMessage = message;
                }

            }
            catch (Exception ex)
            {
                response.StatusCode = 100;
                response.StatusMessage = ex.Message;
            }
            return response;
        }
        public Response<string> DeleteAppointmentByPatientId(int PatientId,int AppointmentId,SqlConnection connection)
        {
            Response<string> response = new Response<string>();
            try
            {
                SqlCommand cmd = new SqlCommand("AppointmentDeleteByPatientId", connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@AppointmentId", AppointmentId);
                cmd.Parameters.AddWithValue("@PatientId", PatientId);
                
                cmd.Parameters.Add("@ErrorMessage", System.Data.SqlDbType.Char, 200);
                cmd.Parameters["@ErrorMessage"].Direction = System.Data.ParameterDirection.Output;
                connection.Open();
                int i = cmd.ExecuteNonQuery();
                connection.Close();
                string message = (string)cmd.Parameters["@ErrorMessage"].Value;
                if (i > 0)
                {
                    response.StatusCode = 200;
                    response.StatusMessage = message;

                }
                else
                {
                    response.StatusCode = 100;
                    response.StatusMessage = message;
                }
            }
            catch(Exception ex)
            {
                response.StatusCode = 100;
                response.StatusMessage = ex.Message;
            }
            return response;
        }
        public Response<string> PostAppointmentByPatientId(int PatientId, int AppointmentId, SqlConnection connection)
        {
            Response<string> response = new Response<string>();
            try
            {
                SqlCommand cmd = new SqlCommand("AppointmentPostByPatientId", connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@AppointmentId", AppointmentId);
                cmd.Parameters.AddWithValue("@PatientId", PatientId);

                cmd.Parameters.Add("@ErrorMessage", System.Data.SqlDbType.Char, 200);
                cmd.Parameters["@ErrorMessage"].Direction = System.Data.ParameterDirection.Output;
                connection.Open();
                int i = cmd.ExecuteNonQuery();
                connection.Close();
                string message = (string)cmd.Parameters["@ErrorMessage"].Value;
                if (i > 0)
                {
                    response.StatusCode = 200;
                    response.StatusMessage = message;

                }
                else
                {
                    response.StatusCode = 100;
                    response.StatusMessage = message;
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 100;
                response.StatusMessage = ex.Message;
            }
            return response;
        }
        public Response<List<Appointment>> GetAppointmentByPatientId(int PatientId, SqlConnection connection)
        {
            Response<List<Appointment>> response = new Response<List<Appointment>>();
            try
            {
                SqlDataAdapter da = new SqlDataAdapter("AppointmentGetByPatientId", connection);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.AddWithValue("@PatientId", PatientId);
                da.SelectCommand.Parameters.Add("@ErrorMessage", System.Data.SqlDbType.Char, 200);
                da.SelectCommand.Parameters["@ErrorMessage"].Direction = System.Data.ParameterDirection.Output;
                DataTable dt = new DataTable();
                da.Fill(dt);
                string message = (string)da.SelectCommand.Parameters["@ErrorMessage"].Value;
                List<Appointment> list = new List<Appointment>();
                if (dt.Rows.Count > 0)
                {

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Appointment item = new Appointment();
                        item.Doctor = new Doctor();
                        item.Doctor.FirstName = Convert.ToString(dt.Rows[i]["FirstName"]);
                        item.Doctor.LastName = Convert.ToString(dt.Rows[i]["LastName"]);
                        item.Doctor.Clinic = new Clinic();
                        item.Doctor.Clinic.Hospital = Convert.ToString(dt.Rows[i]["Hospital"]);
                        item.Doctor.Clinic.Speciality = Convert.ToString(dt.Rows[i]["Speciality"]);
                        item.Doctor.Id = -1;

                        item.Id = Convert.ToInt32(dt.Rows[i]["AppointmentId"]);
                        item.AppointmentDateTime = Convert.ToDateTime(dt.Rows[i]["AppointmentDateTime"]);
                        item.State = Convert.ToString(dt.Rows[i]["State"]);
                        list.Add(item);
                    }

                    response.Data = list;
                    response.StatusCode = 200;
                    response.StatusMessage = message;
                }
                else
                {
                    response.StatusCode = 100;
                    response.StatusMessage = message;
                }

            }
            catch (Exception ex)
            {
                response.StatusCode = 100;
                response.StatusMessage = ex.Message;
            }
            return response;
        }
        public Response<List<Appointment>> GetAppointmentBySearch(Clinic clinic, SqlConnection connection)
        {
            Response<List<Appointment>> response = new Response<List<Appointment>>();
            try
            {
                SqlDataAdapter da = new SqlDataAdapter("AppointmentGetBySearch", connection);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.AddWithValue("@Speciality", clinic.Speciality);
                da.SelectCommand.Parameters.AddWithValue("@Hospital", clinic.Hospital);
                da.SelectCommand.Parameters.Add("@ErrorMessage", System.Data.SqlDbType.Char, 200);
                da.SelectCommand.Parameters["@ErrorMessage"].Direction = System.Data.ParameterDirection.Output;
                DataTable dt = new DataTable();
                da.Fill(dt);
                string message = (string)da.SelectCommand.Parameters["@ErrorMessage"].Value;
                List<Appointment> list = new List<Appointment>();
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Appointment item = new Appointment();
                        item.Doctor = new Doctor();
                        item.Doctor.FirstName = Convert.ToString(dt.Rows[i]["FirstName"]);
                        item.Doctor.LastName = Convert.ToString(dt.Rows[i]["LastName"]);
                        item.Doctor.Clinic = new Clinic();
                        item.Doctor.Clinic.Hospital = Convert.ToString(dt.Rows[i]["Hospital"]);
                        item.Doctor.Clinic.Speciality = Convert.ToString(dt.Rows[i]["Speciality"]);
                        item.Doctor.Id = -1;

                        item.Id = Convert.ToInt32(dt.Rows[i]["AppointmentId"]);
                        item.AppointmentDateTime = Convert.ToDateTime(dt.Rows[i]["AppointmentDateTime"]);
                        item.State = Convert.ToString(dt.Rows[i]["State"]);
                        list.Add(item);
                    }

                    response.Data = list;
                    response.StatusCode = 200;
                    response.StatusMessage = message;
                }
                else
                {
                    response.StatusCode = 100;
                    response.StatusMessage = message;
                }

            }
            catch (Exception ex)
            {
                response.StatusCode = 100;
                response.StatusMessage = ex.Message;
            }
            return response;
        }




        public Response<List<Appointment>> GetAppointmentByDoctorId(int DoctorId,SqlConnection connection) 
        {
            Response<List<Appointment>> response = new Response<List<Appointment>>();
            try
            {
                SqlDataAdapter da = new SqlDataAdapter("AppointmentGetByDoctorId", connection);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.AddWithValue("@DoctorId", DoctorId);
                da.SelectCommand.Parameters.Add("@ErrorMessage", System.Data.SqlDbType.Char, 200);
                da.SelectCommand.Parameters["@ErrorMessage"].Direction = System.Data.ParameterDirection.Output;
                DataTable dt = new DataTable();
                da.Fill(dt);
                string message = (string)da.SelectCommand.Parameters["@ErrorMessage"].Value;
                List<Appointment> list = new List<Appointment>();
                if (dt.Rows.Count > 0)
                {
                    
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Appointment item = new Appointment();
                        item.Patient=new Patient();
                        item.Patient.FirstName= Convert.ToString(dt.Rows[i]["FirstName"]);
                        item.Patient.LastName = Convert.ToString(dt.Rows[i]["LastName"]);
                        item.Patient.Gender = Convert.ToInt32(dt.Rows[i]["Gender"]);
                        item.Patient.BirthDay = Convert.ToDateTime(dt.Rows[i]["Birth"]);
                        item.Patient.Id = -1;
                        item.Id = Convert.ToInt32(dt.Rows[i]["AppointmentId"]);
                        item.AppointmentDateTime = Convert.ToDateTime(dt.Rows[i]["AppointmentDateTime"]);
                        item.State= Convert.ToString(dt.Rows[i]["State"]);
                        list.Add(item);
                    }
                    
                    response.Data = list;
                    response.StatusCode = 200;
                    response.StatusMessage = message;
                }
                else
                {
                    response.StatusCode = 100;
                    response.StatusMessage = message;
                }

            }
            catch(Exception ex)
            {
                response.StatusCode = 100;
                response.StatusMessage = ex.Message;
            }
            return response;
        }




        //
        public Response<List<Patient>> GetPatient(SqlConnection connection)
        {
            Response<List<Patient>> response = new Response<List<Patient>>();
            try
            {
                SqlDataAdapter da = new SqlDataAdapter("PatientGet", connection);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.Add("@ErrorMessage", System.Data.SqlDbType.Char, 200);
                da.SelectCommand.Parameters["@ErrorMessage"].Direction = System.Data.ParameterDirection.Output;
                DataTable dt = new DataTable();
                da.Fill(dt);
                string message = (string)da.SelectCommand.Parameters["@ErrorMessage"].Value;
                List<Patient> list = new List<Patient>();
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Patient item = new Patient();
                        item.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                        item.FirstName = Convert.ToString(dt.Rows[0]["FirstName"]);
                        item.LastName = Convert.ToString(dt.Rows[0]["LastName"]);
                        item.Gender = Convert.ToInt32(dt.Rows[0]["Gender"]);
                        item.BirthDay = Convert.ToDateTime(dt.Rows[0]["Birth"]);
                        item.Info = new Info();
                        item.Info.Address = Convert.ToString(dt.Rows[0]["Address"]);
                        item.Info.Phone = Convert.ToString(dt.Rows[0]["Phone"]);
                        list.Add(item);
                    }

                    response.Data = list;
                    response.StatusCode = 200;
                    response.StatusMessage = message;
                }
                else
                {
                    response.StatusCode = 100;
                    response.StatusMessage = message;
                }

            }
            catch (Exception ex)
            {
                response.StatusCode = 100;
                response.StatusMessage = ex.Message;
            }
            return response;
        }
        public Response<string> DeletePatientById(int PatientId, SqlConnection connection)
        {
            Response<string> response = new Response<string>();
            try
            {
                SqlCommand cmd = new SqlCommand("PatientDeleteById", connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PatientId", PatientId);

                cmd.Parameters.Add("@ErrorMessage", System.Data.SqlDbType.Char, 200);
                cmd.Parameters["@ErrorMessage"].Direction = System.Data.ParameterDirection.Output;
                connection.Open();
                int i = cmd.ExecuteNonQuery();
                connection.Close();
                string message = (string)cmd.Parameters["@ErrorMessage"].Value;
                if (i > 0)
                {
                    response.StatusCode = 200;
                    response.StatusMessage = message;

                }
                else
                {
                    response.StatusCode = 100;
                    response.StatusMessage = message;
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 100;
                response.StatusMessage = ex.Message;
            }
            return response;
        }
        public Response<Patient> GetPatientById(int PatientId, SqlConnection connection)
        {
            Response<Patient> response = new Response<Patient>();
            try
            {

                SqlDataAdapter da = new SqlDataAdapter("PatientGetById", connection);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.AddWithValue("@PatientId", PatientId);
                da.SelectCommand.Parameters.Add("@ErrorMessage", System.Data.SqlDbType.Char, 200);
                da.SelectCommand.Parameters["@ErrorMessage"].Direction = System.Data.ParameterDirection.Output;
                DataTable dt = new DataTable();
                da.Fill(dt);
                string message = (string)da.SelectCommand.Parameters["@ErrorMessage"].Value;
                Patient patient = new Patient();
                if (dt.Rows.Count > 0)
                {
                    patient.Id = -1;
                    patient.FirstName = Convert.ToString(dt.Rows[0]["FirstName"]);
                    patient.LastName = Convert.ToString(dt.Rows[0]["LastName"]);
                    patient.Gender = Convert.ToInt32(dt.Rows[0]["Gender"]);
                    patient.BirthDay = Convert.ToDateTime(dt.Rows[0]["Birth"]);
                    patient.Info = new Info();
                    patient.Info.Address = Convert.ToString(dt.Rows[0]["Address"]);
                    patient.Info.Phone = Convert.ToString(dt.Rows[0]["Phone"]);
                    response.Data = patient;
                    response.StatusCode = 200;
                    response.StatusMessage = message;
                }
                else
                {
                    response.StatusCode = 100;
                    response.StatusMessage = message;
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 100;
                response.StatusMessage = ex.Message;
            }
            return response;
        }
        public Response<List<Doctor>> GetDoctor(SqlConnection connection)
        {
            Response<List<Doctor>> response = new Response<List<Doctor>>();
            try
            {
                SqlDataAdapter da = new SqlDataAdapter("DoctorGet", connection);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.Add("@ErrorMessage", System.Data.SqlDbType.Char, 200);
                da.SelectCommand.Parameters["@ErrorMessage"].Direction = System.Data.ParameterDirection.Output;
                DataTable dt = new DataTable();
                da.Fill(dt);
                string message = (string)da.SelectCommand.Parameters["@ErrorMessage"].Value;
                List<Doctor> list = new List<Doctor>();
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Doctor item = new Doctor();
                        item.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                        item.FirstName = Convert.ToString(dt.Rows[0]["FirstName"]);
                        item.LastName = Convert.ToString(dt.Rows[0]["LastName"]);
                        item.Clinic = new Clinic();
                        item.Clinic.Hospital = Convert.ToString(dt.Rows[0]["Hospital"]);
                        item.Clinic.Speciality = Convert.ToString(dt.Rows[0]["Speciality"]);
                        list.Add(item);
                    }

                    response.Data = list;
                    response.StatusCode = 200;
                    response.StatusMessage = message;
                }
                else
                {
                    response.StatusCode = 100;
                    response.StatusMessage = message;
                }

            }
            catch (Exception ex)
            {
                response.StatusCode = 100;
                response.StatusMessage = ex.Message;
            }
            return response;
        }

        public Response<string> DeleteDoctorById(int DoctorId, SqlConnection connection)
        {
            Response<string> response = new Response<string>();
            try
            {
                SqlCommand cmd = new SqlCommand("DoctorDeleteById", connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DoctorId", DoctorId);

                cmd.Parameters.Add("@ErrorMessage", System.Data.SqlDbType.Char, 200);
                cmd.Parameters["@ErrorMessage"].Direction = System.Data.ParameterDirection.Output;
                connection.Open();
                int i = cmd.ExecuteNonQuery();
                connection.Close();
                string message = (string)cmd.Parameters["@ErrorMessage"].Value;
                if (i > 0)
                {
                    response.StatusCode = 200;
                    response.StatusMessage = message;

                }
                else
                {
                    response.StatusCode = 100;
                    response.StatusMessage = message;
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 100;
                response.StatusMessage = ex.Message;
            }
            return response;
        }
        public Response<Doctor> GetDoctorById(int DoctorId, SqlConnection connection)
        {
            Response<Doctor> response = new Response<Doctor>();
            try
            {

                SqlDataAdapter da = new SqlDataAdapter("DoctorGetById", connection);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.AddWithValue("@DoctorId", DoctorId);
                da.SelectCommand.Parameters.Add("@ErrorMessage", System.Data.SqlDbType.Char, 200);
                da.SelectCommand.Parameters["@ErrorMessage"].Direction = System.Data.ParameterDirection.Output;
                DataTable dt = new DataTable();
                da.Fill(dt);
                string message = (string)da.SelectCommand.Parameters["@ErrorMessage"].Value;
                Doctor doctor = new Doctor();
                if (dt.Rows.Count > 0)
                {
                    doctor.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                    doctor.FirstName = Convert.ToString(dt.Rows[0]["FirstName"]);
                    doctor.LastName = Convert.ToString(dt.Rows[0]["LastName"]);
                    doctor.Clinic = new Clinic();
                    doctor.Clinic.Speciality = Convert.ToString(dt.Rows[0]["Speciality"]);
                    doctor.Clinic.Hospital = Convert.ToString(dt.Rows[0]["Hospital"]);
                    response.Data = doctor;
                    response.StatusCode = 200;
                    response.StatusMessage = message;
                }
                else
                {
                    response.StatusCode = 100;
                    response.StatusMessage = message;
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 100;
                response.StatusMessage = ex.Message;
            }
            return response;
        }

        

        public Response<string> PostAppointment(DateTime AppointmentDateTime, int DoctorId, SqlConnection connection)
        {
            Response<string> response = new Response<string>();
            try
            {
                SqlCommand cmd = new SqlCommand("AppointmentPost", connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@AppointmentDateTime", AppointmentDateTime);
                cmd.Parameters.AddWithValue("@DoctorId", DoctorId);

                cmd.Parameters.Add("@ErrorMessage", System.Data.SqlDbType.Char, 200);
                cmd.Parameters["@ErrorMessage"].Direction = System.Data.ParameterDirection.Output;
                connection.Open();
                int i = cmd.ExecuteNonQuery();
                connection.Close();
                string message = (string)cmd.Parameters["@ErrorMessage"].Value;
                if (i > 0)
                {
                    response.StatusCode = 200;
                    response.StatusMessage = message;

                }
                else
                {
                    response.StatusCode = 100;
                    response.StatusMessage = message;
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 100;
                response.StatusMessage = ex.Message;
            }
            return response;
        }

    }
}
