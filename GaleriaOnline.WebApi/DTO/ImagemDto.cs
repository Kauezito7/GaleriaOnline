namespace GaleriaOnline.WebApi.DTO
{
    public class ImagemDto
    {
        public IFormFile? Arquivo { get; set; } // Propriedade para receber o arquivo de imagem
        public string? Nome { get; set; } // Propriedade para o nome da imagem

    }
}
