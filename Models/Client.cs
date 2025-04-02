using System.ComponentModel.DataAnnotations;

namespace CajaMoreliaApi.Models
{
    public class Clients
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }
        public required string Correo { get; set; }
        public required string Telefono { get; set; }
    }
}