namespace Ecommerce.Models
{
    public class Signup
    {
        public int Signupid { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public long Mobile { get; set; }
        public bool? Isactive { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
