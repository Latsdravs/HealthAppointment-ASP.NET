namespace TheApp.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public DateTime AppointmentDateTime { get; set; }
        public string State { get; set; }
        public Doctor Doctor { get; set; }
        public Patient Patient { get; set; }
    }
}
