using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WpfApp1.service
{
    public class TecnicoService : ITecnicoService
    {
        private readonly HelpDeskContext _context;
        public TecnicoService(HelpDeskContext context) => _context = context;

        public async Task<List<Tecnico>> ListarTodosAsync() => await _context.Tecnicos.ToListAsync();
        public async Task AdicionarAsync(Tecnico tecnico) { _context.Tecnicos.Add(tecnico); await _context.SaveChangesAsync(); }
        public async Task AtualizarAsync(Tecnico tecnico) { _context.Tecnicos.Update(tecnico); await _context.SaveChangesAsync(); }
        public async Task RemoverAsync(int id)
        {
            var t = await _context.Tecnicos.FindAsync(id);
            if (t != null) { _context.Tecnicos.Remove(t); await _context.SaveChangesAsync(); }
        }
        public async Task<List<Tecnico>> ListarDisponiveisAsync() => await _context.Tecnicos.Where(t => t.Disponivel).ToListAsync();
    }

    public class ChamadoService : IChamadoService
    {
        private readonly HelpDeskContext _context;
        public ChamadoService(HelpDeskContext context) => _context = context;

        public async Task<List<Chamado>> ListarTodosAsync() => await _context.Chamados.ToListAsync();
        public async Task AdicionarAsync(Chamado chamado) 
        { 
            chamado.Status = "Aberto";
            _context.Chamados.Add(chamado); 
            await _context.SaveChangesAsync(); 
        }
        public async Task AtualizarAsync(Chamado chamado) { _context.Chamados.Update(chamado); await _context.SaveChangesAsync(); }
        public async Task RemoverAsync(int id)
        {
            var c = await _context.Chamados.FindAsync(id);
            if (c != null) { _context.Chamados.Remove(c); await _context.SaveChangesAsync(); }
        }

        public async Task AtenderChamadoAsync(int chamadoId, string nomeTecnico)
        {
            var c = await _context.Chamados.FindAsync(chamadoId);
            if (c != null)
            {
                c.Status = "Em atendimento";
                c.NomeTecnico = nomeTecnico;
                await _context.SaveChangesAsync();
            }
        }

        public async Task FinalizarChamadoAsync(int chamadoId)
        {
            var c = await _context.Chamados.FindAsync(chamadoId);
            if (c != null)
            {
                c.Status = "Resolvido";
                await _context.SaveChangesAsync();
            }
        }
    }
}
