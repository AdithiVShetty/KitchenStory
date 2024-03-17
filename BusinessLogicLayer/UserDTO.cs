namespace BusinessLogicLayer
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string EmailId { get; set; }
        public string Password { get; set; }
        public string ContactNo { get; set; }
        public string Address { get; set; }
        public bool IsAdmin { get; set; }
    }
}