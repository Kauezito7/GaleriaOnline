namespace GaleriaOnline.WebApi.DTO
{
    public class PutImagemDto
    {
        public IFormFile? Arquivo { get; set; } // Propriedade para atualizar o arquivo de imagem
        public string? Nome { get; set; } // propriedade para atualizar o nome da imagem
    }
}
