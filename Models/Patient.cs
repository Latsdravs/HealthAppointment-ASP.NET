namespace TheApp.Models
{
    public class Patient : Person
    {
        public int Gender { get; set; } 
        public DateTime BirthDay { get; set; }
        public Info Info { get; set; }

    }
}
