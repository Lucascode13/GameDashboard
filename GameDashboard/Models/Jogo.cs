using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GameDashboard.Models;

public class Jogo
{
    [Key]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Nome { get; set; } = string.Empty;

    [JsonPropertyName("slug")]
    public string Slug { get; set; } = string.Empty;

    [JsonPropertyName("image")]
    public string? UrlImagem { get; set; }

    [JsonPropertyName("image_background")]
    public string? UrlImagemFundo { get; set; }

    [JsonPropertyName("description")]
    public string? Descricao { get; set; }

    [JsonPropertyName("games_count")]
    public int QuantidadeJogos { get; set; }

    [JsonPropertyName("reviews_count")]
    public int QuantidadeAvaliacoes { get; set; }

    [JsonPropertyName("rating")]
    public string? Nota { get; set; }

    [JsonPropertyName("rating_top")]
    public int NotaMaxima { get; set; }

    [JsonPropertyName("updated")]
    public DateTime AtualizadoEm { get; set; }
}