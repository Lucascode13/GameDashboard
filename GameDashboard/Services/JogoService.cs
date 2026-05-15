using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using GameDashboard.Data;
using GameDashboard.Models;
using Microsoft.EntityFrameworkCore;

namespace GameDashboard.Services;

public class JogoService
{
    private readonly HttpClient _httpClient;

    private const string UrlApi = "https://sua-api.com/api/jogos";

    private static readonly JsonSerializerOptions OpcoesJson = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public JogoService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.Timeout = TimeSpan.FromSeconds(30);
    }

    public async Task<IReadOnlyList<Jogo>> BuscarESalvarJogosAsync(
        CancellationToken ct = default)
    {
        await using var context = new GameDashboardContext();
        await context.Database.EnsureCreatedAsync(ct);

        try
        {
            var jogosApi = await _httpClient
                .GetFromJsonAsync<List<Jogo>>(UrlApi, OpcoesJson, ct)
                ?? new List<Jogo>();

            foreach (var jogo in jogosApi)
            {
                var existente = await context.Jogos
                    .FirstOrDefaultAsync(j => j.Slug == jogo.Slug, ct);

                if (existente is null)
                    context.Jogos.Add(jogo);
                else
                    context.Entry(existente).CurrentValues.SetValues(jogo);
            }

            await context.SaveChangesAsync(ct);
        }
        catch (HttpRequestException ex)
        {
            Console.Error.WriteLine(
                $"[AVISO] Falha ao conectar à API: {ex.Message}. " +
                "Carregando dados do cache local.");
        }
        catch (TaskCanceledException)
        {
            Console.Error.WriteLine("[AVISO] Requisição cancelada ou timeout atingido.");
        }
        catch (JsonException ex)
        {
            Console.Error.WriteLine($"[ERRO] Resposta da API inválida: {ex.Message}");
        }

        return await context.Jogos
            .OrderByDescending(j => j.Nota)
            .ToListAsync(ct);
    }
}