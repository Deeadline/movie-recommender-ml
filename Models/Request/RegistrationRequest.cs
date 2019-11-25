namespace Recommend_Movie_System.Models.Request
{
    public class RegistrationRequest
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string password { get; set; }

        public string role => Role.User;
    }
}