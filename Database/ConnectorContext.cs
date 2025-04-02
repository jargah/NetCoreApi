using Microsoft.EntityFrameworkCore;
using CajaMoreliaApi.Models;

namespace CajaMoreliaApi.Database
{
    public class ConnectorDatabase : DbContext
    {
        public ConnectorDatabase(DbContextOptions<ConnectorDatabase> options) : base(options) { }

        public DbSet<Clients> Clients { get; set; }
        public DbSet<ApiKey> ApiKeys { get; set; }
    }
}