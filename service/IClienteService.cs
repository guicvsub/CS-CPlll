using System.Collections.Generic;
using System.Threading.Tasks;

namespace WpfApp1.service
{
    public interface IClienteService 
    {
        Task<List<Equipamento>> ListarTodosAsync();
        Task AdicionarAsync(Cliente cliente);
        Task AtualizarAsync(Cliente cliente);
        Task RemoverAsync(int id);
        Task<Cliente?> BuscarPorIdAsync(int id);
    }
}
