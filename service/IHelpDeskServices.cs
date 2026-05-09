using System.Collections.Generic;
using System.Threading.Tasks;

namespace WpfApp1.service
{
    public interface ITecnicoService
    {
        Task<List<Tecnico>> ListarTodosAsync();
        Task AdicionarAsync(Tecnico tecnico);
        Task AtualizarAsync(Tecnico tecnico);
        Task RemoverAsync(int id);
        Task<List<Tecnico>> ListarDisponiveisAsync();
    }

    public interface IChamadoService
    {
        Task<List<Chamado>> ListarTodosAsync();
        Task AdicionarAsync(Chamado chamado);
        Task AtualizarAsync(Chamado chamado);
        Task RemoverAsync(int id);
        Task AtenderChamadoAsync(int chamadoId, string nomeTecnico);
        Task FinalizarChamadoAsync(int chamadoId);
    }
}
