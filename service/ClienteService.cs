using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace WpfApp1.service
{
    public class ClienteService : IClienteService
    {
        private readonly HelpDeskContext _context;

        public ClienteService(HelpDeskContext context)
        {
            _context = context;
        }

        public async Task<List<Cliente>> ListarTodosAsync()
        {
            return await _context.Clientes.ToListAsync();
        }

        public async Task AdicionarAsync(Cliente cliente)
        {
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();
        }

        public async Task<Cliente?> BuscarPorIdAsync(int id)
        {
            return await _context.Clientes.FindAsync(id);
        }

        public async Task AtualizarAsync(Cliente cliente)
        {
            _context.Clientes.Update(cliente);
            await _context.SaveChangesAsync();
        }

        public async Task RemoverAsync(int id)
        {
            var cliente = await BuscarPorIdAsync(id);
            if (cliente != null)
            {
                _context.Clientes.Remove(cliente);
                await _context.SaveChangesAsync();
            }
        }

        Task<List<Cliente>> IClienteService.ListarTodosAsync()
        {
            throw new NotImplementedException();
        }
    }
}
