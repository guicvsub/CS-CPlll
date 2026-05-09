using System.Collections.Generic;
using System.Threading.Tasks;

namespace WpfApp1.service
{
    public interface IDepartamentoService
    {
        Task<List<Departamento>> ListarTodosAsync();
        Task<List<Departamento>> ListarAtivosAsync();
        Task AdicionarAsync(Departamento departamento);
        Task<Departamento?> BuscarPorIdAsync(int id);
        Task AtualizarAsync(Departamento departamento);
        Task RemoverAsync(int id);
    }
}
