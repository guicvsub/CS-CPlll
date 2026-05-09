using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfApp1.service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace WpfApp1.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        // Services
        private readonly IClienteService? _clienteService;
        private readonly IDepartamentoService? _departamentoService;
        private readonly ITecnicoService? _tecnicoService;
        private readonly IChamadoService? _chamadoService;

        // Navigation
        private bool _showChamados = true;
        private bool _showClientes;
        private bool _showDepartamentos;
        private bool _showTecnicos;

        public bool ShowChamados
        {
            get => _showChamados;
            set { _showChamados = value; OnPropertyChanged(); OnPropertyChanged(nameof(CurrentPageTitle)); OnPropertyChanged(nameof(CurrentPageSubtitle)); }
        }
        public bool ShowClientes
        {
            get => _showClientes;
            set { _showClientes = value; OnPropertyChanged(); OnPropertyChanged(nameof(CurrentPageTitle)); OnPropertyChanged(nameof(CurrentPageSubtitle)); }
        }
        public bool ShowDepartamentos
        {
            get => _showDepartamentos;
            set { _showDepartamentos = value; OnPropertyChanged(); OnPropertyChanged(nameof(CurrentPageTitle)); OnPropertyChanged(nameof(CurrentPageSubtitle)); }
        }
        public bool ShowTecnicos
        {
            get => _showTecnicos;
            set { _showTecnicos = value; OnPropertyChanged(); OnPropertyChanged(nameof(CurrentPageTitle)); OnPropertyChanged(nameof(CurrentPageSubtitle)); }
        }

        public string CurrentPageTitle => ShowChamados ? "Chamados" : ShowClientes ? "Clientes" : ShowDepartamentos ? "Departamentos" : "Técnicos";
        public string CurrentPageSubtitle => ShowChamados ? "Gerencie todos os chamados de suporte" : ShowClientes ? "Cadastro e gestão de clientes" : ShowDepartamentos ? "Setores e departamentos da empresa" : "Equipe técnica de atendimento";

        // Collections
        public ObservableCollection<Cliente> Clientes { get; } = new();
        public ObservableCollection<Departamento> Departamentos { get; } = new();
        public ObservableCollection<Tecnico> Tecnicos { get; } = new();
        public ObservableCollection<Chamado> Chamados { get; } = new();
        public List<string> Prioridades { get; } = new() { "Alta", "Média", "Baixa" };

        // Selected items
        private Chamado? _selectedChamado;
        private Cliente? _selectedCliente;
        private Departamento? _selectedDepartamento;
        private Tecnico? _selectedTecnico;
        private Tecnico? _tecnicoParaAtender;

        public Chamado? SelectedChamado { get => _selectedChamado; set { _selectedChamado = value; OnPropertyChanged(); } }
        public Cliente? SelectedCliente { get => _selectedCliente; set { _selectedCliente = value; OnPropertyChanged(); } }
        public Departamento? SelectedDepartamento { get => _selectedDepartamento; set { _selectedDepartamento = value; OnPropertyChanged(); } }
        public Tecnico? SelectedTecnico { get => _selectedTecnico; set { _selectedTecnico = value; OnPropertyChanged(); } }
        public Tecnico? TecnicoParaAtender { get => _tecnicoParaAtender; set { _tecnicoParaAtender = value; OnPropertyChanged(); } }

        // Stats
        public int TotalChamados => Chamados.Count;
        public int ChamadosAbertos => GetCount("Aberto");
        public int ChamadosResolvidos => GetCount("Resolvido");
        private int GetCount(string status) { int c = 0; foreach (var ch in Chamados) if (ch.Status == status) c++; return c; }

        // Status message
        private string _statusMessage = "Carregando...";
        public string StatusMessage { get => _statusMessage; set { _statusMessage = value; OnPropertyChanged(); } }

        // Form overlay
        private bool _showForm;
        private bool _isEditing;
        public bool ShowForm { get => _showForm; set { _showForm = value; OnPropertyChanged(); } }
        public string FormTitle => _isEditing ? "Editar Registro" : "Novo Registro";

        // Form fields (shared)
        private string _formNome = "";
        private string _formEmpresa = "";
        private string _formEmail = "";
        private string _formTelefone = "";
        private string _formResponsavel = "";
        private string _formEspecialidade = "";
        private bool _formAtivo = true;
        private bool _formDisponivel = true;
        private string _formTitulo = "";
        private string _formDescricao = "";
        private string _formPrioridade = "Média";
        private string _formNomeCliente = "";
        private Tecnico? _formTecnicoSelecionado;

        public string FormNome { get => _formNome; set { _formNome = value; OnPropertyChanged(); } }
        public string FormEmpresa { get => _formEmpresa; set { _formEmpresa = value; OnPropertyChanged(); } }
        public string FormEmail { get => _formEmail; set { _formEmail = value; OnPropertyChanged(); } }
        public string FormTelefone { get => _formTelefone; set { _formTelefone = value; OnPropertyChanged(); } }
        public string FormResponsavel { get => _formResponsavel; set { _formResponsavel = value; OnPropertyChanged(); } }
        public string FormEspecialidade { get => _formEspecialidade; set { _formEspecialidade = value; OnPropertyChanged(); } }
        public bool FormAtivo { get => _formAtivo; set { _formAtivo = value; OnPropertyChanged(); } }
        public bool FormDisponivel { get => _formDisponivel; set { _formDisponivel = value; OnPropertyChanged(); } }
        public string FormTitulo { get => _formTitulo; set { _formTitulo = value; OnPropertyChanged(); } }
        public string FormDescricao { get => _formDescricao; set { _formDescricao = value; OnPropertyChanged(); } }
        public string FormPrioridade { get => _formPrioridade; set { _formPrioridade = value; OnPropertyChanged(); } }
        public string FormNomeCliente { get => _formNomeCliente; set { _formNomeCliente = value; OnPropertyChanged(); } }
        public Tecnico? FormTecnicoSelecionado { get => _formTecnicoSelecionado; set { _formTecnicoSelecionado = value; OnPropertyChanged(); } }

        // Commands
        public ICommand OpenAddDialogCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand CancelFormCommand { get; }
        public ICommand AtenderCommand { get; }
        public ICommand FinalizarCommand { get; }

        public MainViewModel()
        {
            try
            {
                var config = new ConfigurationBuilder().AddUserSecrets<App>().Build();
                var conn = config.GetConnectionString("DefaultConnection");
                if (!string.IsNullOrEmpty(conn))
                {
                    var opts = new DbContextOptionsBuilder<HelpDeskContext>().UseMySql(conn, ServerVersion.AutoDetect(conn)).Options;
                    var ctx = new HelpDeskContext(opts);
                    _clienteService = new ClienteService(ctx);
                    _departamentoService = new DepartamentoService(ctx);
                    _tecnicoService = new TecnicoService(ctx);
                    _chamadoService = new ChamadoService(ctx);
                    _ = CarregarDadosAsync();
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Erro: {ex.Message}";
            }

            OpenAddDialogCommand = new RelayCommand(_ => AbrirFormularioNovo());
            EditCommand = new RelayCommand(_ => AbrirFormularioEdicao());
            DeleteCommand = new RelayCommand(_ => _ = ExcluirAsync());
            SaveCommand = new RelayCommand(_ => _ = SalvarAsync());
            CancelFormCommand = new RelayCommand(_ => ShowForm = false);
            AtenderCommand = new RelayCommand(_ => _ = AtenderChamadoAsync());
            FinalizarCommand = new RelayCommand(_ => _ = FinalizarChamadoAsync());
        }

        public async Task CarregarDadosAsync()
        {
            if (_clienteService == null) return;
            StatusMessage = "Sincronizando...";

            var clientes = await _clienteService.ListarTodosAsync();
            var departamentos = await _departamentoService!.ListarTodosAsync();
            var tecnicos = await _tecnicoService!.ListarTodosAsync();
            var chamados = await _chamadoService!.ListarTodosAsync();

            App.Current.Dispatcher.Invoke(() =>
            {
                Clientes.Clear(); foreach (var c in clientes) Clientes.Add(c);
                Departamentos.Clear(); foreach (var d in departamentos) Departamentos.Add(d);
                Tecnicos.Clear(); foreach (var t in tecnicos) Tecnicos.Add(t);
                Chamados.Clear(); foreach (var ch in chamados) Chamados.Add(ch);
                OnPropertyChanged(nameof(TotalChamados));
                OnPropertyChanged(nameof(ChamadosAbertos));
                OnPropertyChanged(nameof(ChamadosResolvidos));
                StatusMessage = $"Atualizado • {clientes.Count} clientes • {chamados.Count} chamados";
            });
        }

        private void AbrirFormularioNovo()
        {
            _isEditing = false;
            LimparFormulario();
            OnPropertyChanged(nameof(FormTitle));
            ShowForm = true;
        }

        private void AbrirFormularioEdicao()
        {
            _isEditing = true;
            if (ShowChamados && SelectedChamado != null)
            {
                FormTitulo = SelectedChamado.Titulo;
                FormDescricao = SelectedChamado.Descricao;
                FormPrioridade = SelectedChamado.Prioridade;
                FormNomeCliente = SelectedChamado.NomeCliente;
            }
            else if (ShowClientes && SelectedCliente != null)
            {
                FormNome = SelectedCliente.Nome;
                FormEmpresa = SelectedCliente.Empresa;
                FormEmail = SelectedCliente.Email;
                FormTelefone = SelectedCliente.Telefone;
            }
            else if (ShowDepartamentos && SelectedDepartamento != null)
            {
                FormNome = SelectedDepartamento.Nome;
                FormResponsavel = SelectedDepartamento.Responsavel;
                FormAtivo = SelectedDepartamento.Ativo;
            }
            else if (ShowTecnicos && SelectedTecnico != null)
            {
                FormNome = SelectedTecnico.Nome;
                FormEspecialidade = SelectedTecnico.Especialidade;
                FormEmail = SelectedTecnico.Email;
                FormDisponivel = SelectedTecnico.Disponivel;
            }
            OnPropertyChanged(nameof(FormTitle));
            ShowForm = true;
        }

        private async Task SalvarAsync()
        {
            if (ShowChamados) await SalvarChamadoAsync();
            else if (ShowClientes) await SalvarClienteAsync();
            else if (ShowDepartamentos) await SalvarDepartamentoAsync();
            else if (ShowTecnicos) await SalvarTecnicoAsync();
            ShowForm = false;
            await CarregarDadosAsync();
        }

        private async Task SalvarChamadoAsync()
        {
            if (_chamadoService == null) return;
            if (_isEditing && SelectedChamado != null)
            {
                SelectedChamado.Titulo = FormTitulo;
                SelectedChamado.Descricao = FormDescricao;
                SelectedChamado.Prioridade = FormPrioridade;
                SelectedChamado.NomeCliente = FormNomeCliente;
                if (FormTecnicoSelecionado != null)
                    SelectedChamado.NomeTecnico = FormTecnicoSelecionado.Nome;
                await _chamadoService.AtualizarAsync(SelectedChamado);
            }
            else
            {
                var nomeTec = FormTecnicoSelecionado?.Nome ?? string.Empty;
                await _chamadoService.AdicionarAsync(new Chamado { Titulo = FormTitulo, Descricao = FormDescricao, Prioridade = FormPrioridade, NomeCliente = FormNomeCliente, NomeTecnico = nomeTec });
            }
        }

        private async Task SalvarClienteAsync()
        {
            if (_clienteService == null) return;
            if (_isEditing && SelectedCliente != null)
            {
                SelectedCliente.Nome = FormNome; SelectedCliente.Empresa = FormEmpresa;
                SelectedCliente.Email = FormEmail; SelectedCliente.Telefone = FormTelefone;
                await _clienteService.AtualizarAsync(SelectedCliente);
            }
            else { await _clienteService.AdicionarAsync(new Cliente { Nome = FormNome, Empresa = FormEmpresa, Email = FormEmail, Telefone = FormTelefone }); }
        }

        private async Task SalvarDepartamentoAsync()
        {
            if (_departamentoService == null) return;
            if (_isEditing && SelectedDepartamento != null)
            {
                SelectedDepartamento.Nome = FormNome; SelectedDepartamento.Responsavel = FormResponsavel; SelectedDepartamento.Ativo = FormAtivo;
                await _departamentoService.AtualizarAsync(SelectedDepartamento);
            }
            else { await _departamentoService.AdicionarAsync(new Departamento { Nome = FormNome, Responsavel = FormResponsavel, Ativo = FormAtivo }); }
        }

        private async Task SalvarTecnicoAsync()
        {
            if (_tecnicoService == null) return;
            if (_isEditing && SelectedTecnico != null)
            {
                SelectedTecnico.Nome = FormNome; SelectedTecnico.Especialidade = FormEspecialidade;
                SelectedTecnico.Email = FormEmail; SelectedTecnico.Disponivel = FormDisponivel;
                await _tecnicoService.AtualizarAsync(SelectedTecnico);
            }
            else { await _tecnicoService.AdicionarAsync(new Tecnico { Nome = FormNome, Especialidade = FormEspecialidade, Email = FormEmail, Disponivel = FormDisponivel }); }
        }

        private async Task ExcluirAsync()
        {
            if (ShowChamados && SelectedChamado != null) { await _chamadoService!.RemoverAsync(SelectedChamado.Id); }
            else if (ShowClientes && SelectedCliente != null) { await _clienteService!.RemoverAsync(SelectedCliente.Id); }
            else if (ShowDepartamentos && SelectedDepartamento != null) { await _departamentoService!.RemoverAsync(SelectedDepartamento.Id); }
            else if (ShowTecnicos && SelectedTecnico != null) { await _tecnicoService!.RemoverAsync(SelectedTecnico.Id); }
            await CarregarDadosAsync();
        }

        private async Task AtenderChamadoAsync()
        {
            if (SelectedChamado != null && _chamadoService != null)
            {
                var nomeTecnico = TecnicoParaAtender?.Nome ?? SelectedChamado.NomeTecnico;
                if (string.IsNullOrEmpty(nomeTecnico)) nomeTecnico = "Sem tecnico atribuido";
                await _chamadoService.AtenderChamadoAsync(SelectedChamado.Id, nomeTecnico);
                TecnicoParaAtender = null;
                await CarregarDadosAsync();
            }
        }

        private async Task FinalizarChamadoAsync()
        {
            if (SelectedChamado != null && _chamadoService != null)
            {
                await _chamadoService.FinalizarChamadoAsync(SelectedChamado.Id);
                await CarregarDadosAsync();
            }
        }

        private void LimparFormulario()
        {
            FormNome = FormEmpresa = FormEmail = FormTelefone = FormResponsavel =
            FormEspecialidade = FormTitulo = FormDescricao = FormNomeCliente = "";
            FormPrioridade = "Média";
            FormAtivo = FormDisponivel = true;
            FormTecnicoSelecionado = null;
        }
    }
}
