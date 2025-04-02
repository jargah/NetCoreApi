namespace CajaMoreliaApi.Models
{
    public class ApiKey
    {
        public int Id { get; set; }
        public required string Token { get; set; }
        public bool IsActive { get; set; }
    }
}