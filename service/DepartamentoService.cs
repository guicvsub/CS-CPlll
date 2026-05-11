using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WpfApp1.service
{
    public class DepartamentoService : IDepartamentoService
    {
        private readonly HelpDeskContext _context;

        public DepartamentoService(HelpDeskContext context)
        {
            _context = context;
        }

        public async Task<List<Departamento>> ListarTodosAsync()
        {
            return await _context.Departamentos.AsNoTracking().ToListAsync();
        }

        public async Task<List<Departamento>> ListarAtivosAsync()
        {
            return await _context.Departamentos.AsNoTracking().Where(d => d.Ativo).ToListAsync();
        }

        public async Task AdicionarAsync(Departamento departamento)
        {
            _context.Departamentos.Add(departamento);
            await _context.SaveChangesAsync();
        }

        public async Task<Departamento?> BuscarPorIdAsync(int id)
        {
            return await _context.Departamentos.FindAsync(id);
        }

        public async Task AtualizarAsync(Departamento departamento)
        {
            _context.Departamentos.Update(departamento);
            await _context.SaveChangesAsync();
        }

        public async Task RemoverAsync(int id)
        {
            var departamento = await BuscarPorIdAsync(id);
            if (departamento != null)
            {
                _context.Departamentos.Remove(departamento);
                await _context.SaveChangesAsync();
            }
        }
    }
}
