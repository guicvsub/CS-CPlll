using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;



namespace WpfApp1.config.db
{
    public class Factory : IDesignTimeDbContextFactory<HelpDeskContext>
    {
        public HelpDeskContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddUserSecrets<Factory>()
                .Build();

            var conexao = configuration.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<HelpDeskContext>();

            optionsBuilder.UseMySql(
                conexao,
                ServerVersion.AutoDetect(conexao)
            );

            return new HelpDeskContext(optionsBuilder.Options);
        }
    }
}
