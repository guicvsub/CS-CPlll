using System.Collections.Generic;
using System.Threading.Tasks;

namespace WpfApp1.service
{
    public interface IEquipamentoService 
    {
        Task<List<Equipamento>> ListarTodosAsync();
        Task AdicionarAsync(Equipamento equipamento);
        Task AtualizarAsync(Equipamento equipamento);
        Task RemoverAsync(int id);
        Task<Equipamento?> BuscarPorIdAsync(int id);
    }
}
