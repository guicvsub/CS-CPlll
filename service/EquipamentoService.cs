using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WpfApp1.service
{
    public class EquipamentoService : IEquipamentoService
    {
        private readonly HelpDeskContext _context;

        public EquipamentoService(HelpDeskContext context)
        {
            _context = context;
        }

        public async Task<List<Equipamento>> ListarTodosAsync()
        {
            return await _context.Equipamentos.AsNoTracking().ToListAsync();
        }

        public async Task AdicionarAsync(Equipamento equipamento)
        {
            _context.Equipamentos.Add(equipamento);
            await _context.SaveChangesAsync();
        }

        public async Task<Equipamento?> BuscarPorIdAsync(int id)
        {
            return await _context.Equipamentos.FindAsync(id);
        }

        public async Task AtualizarAsync(Equipamento equipamento)
        {
            _context.Equipamentos.Update(equipamento);
            await _context.SaveChangesAsync();
        }

        public async Task RemoverAsync(int id)
        {
            var equipamento = await BuscarPorIdAsync(id);
            if (equipamento != null)
            {
                _context.Equipamentos.Remove(equipamento);
                await _context.SaveChangesAsync();
            }
        }
    }
}
