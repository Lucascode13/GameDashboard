using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GameDashboard.Models;
using GameDashboard.Services;

namespace GameDashboard.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly JogoService _jogoService;

    public MainViewModel(JogoService jogoService)
    {
        _jogoService = jogoService;
    }

    [ObservableProperty]
    private ObservableCollection<Jogo> _jogos = [];

    [ObservableProperty]
    private Jogo? _jogoSelecionado;

    [ObservableProperty]
    private bool _carregando;

    [ObservableProperty]
    private string _mensagemStatus = "Pronto.";

    // Total de jogos únicos na coleção
    public int TotalJogos => Jogos.Count;

    // Melhor nota da coleção atual
    public string MelhorNota => Jogos
        .OrderByDescending(j => j.Nota)
        .FirstOrDefault()?.Nota ?? "N/A";

    [RelayCommand]
    private async Task CarregarJogosAsync()
    {
        Carregando = true;
        MensagemStatus = "Buscando dados da API...";

        try
        {
            var lista = await _jogoService.BuscarESalvarJogosAsync();

            Jogos = new ObservableCollection<Jogo>(lista);

            // Notifica as propriedades derivadas
            OnPropertyChanged(nameof(TotalJogos));
            OnPropertyChanged(nameof(MelhorNota));

            MensagemStatus = $"{Jogos.Count} jogos carregados em {DateTime.Now:HH:mm:ss}";
        }
        catch (Exception ex)
        {
            MensagemStatus = $"Erro inesperado: {ex.Message}";
        }
        finally
        {
            Carregando = false;
        }
    }
}