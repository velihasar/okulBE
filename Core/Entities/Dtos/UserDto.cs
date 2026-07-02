namespace Core.Entities.Dtos
{
    public class UserDto : IEntity
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string MobilePhones { get; set; }
        public string Password { get; set; }
        public bool Status { get; set; }
        public string RefreshToken { get; set; }
    }
}