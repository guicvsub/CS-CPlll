using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace WpfApp1
{
    public class HelpDeskContext : DbContext
    {
        public HelpDeskContext(DbContextOptions<HelpDeskContext> options)
            : base(options)
        {
        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Tecnico> Tecnicos { get; set; }
        public DbSet<Chamado> Chamados { get; set; }
        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<Equipamento> Equipamentos {get ; set;}
    }
}
