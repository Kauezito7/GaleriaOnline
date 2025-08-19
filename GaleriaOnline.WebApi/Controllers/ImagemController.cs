using GaleriaOnline.WebApi.DTO;
using GaleriaOnline.WebApi.Interfaces;
using GaleriaOnline.WebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GaleriaOnline.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagemController : ControllerBase
    {
        private readonly IImagemRepository _repository; // Injeção de dependência para o repositório
        private readonly IWebHostEnvironment _env; // Ambiente para acessar o sistema de arquivos

        public ImagemController(IImagemRepository repository, IWebHostEnvironment env)
        {
            _repository = repository;
            _env = env;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetImagemPorId(int id) // Busca uma imagem pelo ID
        {
            var imagem = await _repository.GetByIdAsync(id); // Chama o repositório para buscar a imagem
            if (imagem == null)
            {
                return NotFound("Imagem não encontrada");
            }
            return Ok(imagem);
        }

        [HttpGet]
        public async Task<IActionResult> GetTodasImagens() // Busca todas as imagens
        {
            var imagens = await _repository.GetAllAsync(); // Chama o repositório para buscar todas as imagens
            return Ok(imagens);
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage([FromForm] ImagemDto imagemDto) // Upload de imagem
        {
            if (imagemDto.Arquivo == null || imagemDto.Arquivo.Length == 0 || String.IsNullOrWhiteSpace(imagemDto.Nome))
            {
                return BadRequest("Deve ser enviado um Nome e uma Imagem");
            }

            var extensao = Path.GetExtension(imagemDto.Arquivo.FileName).ToLowerInvariant(); // Obtém a extensão do arquivo
            var nomeArquivo = $"{Guid.NewGuid()}{extensao}"; // Gera um nome único para o arquivo

            var pastaRelativa = "wwwroot/imagens"; // Pasta onde as imagens serão salvas

            var caminhoPasta = Path.Combine(Directory.GetCurrentDirectory(), pastaRelativa); // Caminho completo da pasta  

            if (!Directory.Exists(caminhoPasta)) // Verifica se a pasta existe, se não, cria uma pasta para mim
            {
                Directory.CreateDirectory(caminhoPasta);
            }

            var caminhoCompleto = Path.Combine(caminhoPasta, nomeArquivo); // Caminho completo do arquivo

            using (var stream = new FileStream(caminhoCompleto, FileMode.Create)) // Cria o arquivo no sistema de arquivos
            {
                await imagemDto.Arquivo.CopyToAsync(stream); // Copia o conteúdo do arquivo enviado para o sistema de arquivos
            }

            var imagem = new Imagem // Cria uma nova instância(caracteristica) de Imagem
            {
                Nome = imagemDto.Nome,
                Caminho = Path.Combine(pastaRelativa, nomeArquivo).Replace("\\", "/") // Caminho relativo da imagem
            };

            await _repository.CreateAsync(imagem); // Salva a imagem no banco de dados

            return CreatedAtAction(nameof(GetImagemPorId), new { id = imagem.Id }, imagem); // Retorna o status 201 Created com a imagem criada
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarImagem(int id, PutImagemDto imagemAtualizada)
        {
            var imagem = await _repository.GetByIdAsync(id); // Busca a imagem pelo ID
            if (imagem == null)
            {
                return NotFound("Imagem não encontrada");
            }

            if (imagemAtualizada.Arquivo == null && string.IsNullOrWhiteSpace(imagemAtualizada.Nome)) //Verificar se os campos vem vazio, caso sim retorna o badrequest
            {
                return BadRequest("Deve ser enviado um Nome ou uma Imagem para atualizar");
            }

            if (!string.IsNullOrWhiteSpace(imagemAtualizada.Nome)) // Verifica se o nome foi atualizado
            {
                imagem.Nome = imagemAtualizada.Nome; // Atualiza o nome da imagem
            }

            var caminhoAntigo = Path.Combine
                (Directory.GetCurrentDirectory(),
                imagem.Caminho.Replace("/",
                Path.DirectorySeparatorChar.ToString()));

            if (imagemAtualizada.Arquivo != null &&
                imagemAtualizada.Arquivo.Length > 0) // Verifica se um novo arquivo foi enviado
            {
                if (System.IO.File.Exists(caminhoAntigo)) // Verifica se o arquivo antigo existe
                {
                    System.IO.File.Delete(caminhoAntigo); // Deleta o arquivo antigo
                }

                var extensao = Path.GetExtension(imagemAtualizada.Arquivo.FileName).ToLowerInvariant(); // Obtém a extensão do arquivo
                var nomeArquivo = $"{Guid.NewGuid()}{extensao}"; // Gera um nome único para o arquivo

                var pastaRelativa = "wwwroot/imagens"; // Pasta onde as imagens serão salvas

                var caminhoPasta = Path.Combine(Directory.GetCurrentDirectory(), pastaRelativa); // Caminho completo da pasta  

                if (!Directory.Exists(caminhoPasta)) // Verifica se a pasta existe, se não, cria uma pasta para mim
                {
                    Directory.CreateDirectory(caminhoPasta);
                }

                var caminhoCompleto = Path.Combine(caminhoPasta, nomeArquivo); // Caminho completo do arquivo

                using (var stream = new FileStream(caminhoCompleto, FileMode.Create)) // Cria o arquivo no sistema de arquivos
                {
                    await imagemAtualizada.Arquivo.CopyToAsync(stream); // Copia o conteúdo do arquivo enviado para o sistema de arquivos
                }

                imagem.Caminho = Path.Combine(pastaRelativa, nomeArquivo).Replace("\\", "/"); // Atualiza o caminho da imagem

            }

            var atualizado = await _repository.UpdateAsync(imagem); // Atualiza a imagem no banco de dados
            if (!atualizado)
            {
                return StatusCode(500, "Erro ao atualizar a imagem");
            }

            return Ok(imagem); // Retorna a imagem atualizada
        }

        [HttpDelete("{id}")]

        public async Task<IActionResult> DeletarImagem(int id) // Deleta uma imagem pelo ID
        {
            var imagem = await _repository.GetByIdAsync(id); // Busca a imagem pelo ID
            if (imagem == null)
            {
                return NotFound("Imagem não encontrada");
            }
            var caminhoCompleto = Path.Combine(Directory.GetCurrentDirectory(), imagem.Caminho.Replace("/", Path.DirectorySeparatorChar.ToString())); // Caminho completo do arquivo
            if (System.IO.File.Exists(caminhoCompleto)) // Verifica se o arquivo existe
            {
                try
                {
                    System.IO.File.Delete(caminhoCompleto); // Deleta o arquivo do sistema de arquivos
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Erro ao deletar o arquivo: {ex.Message}"); // Retorna erro se não conseguir deletar o arquivo
                }
            }
            var deletado = await _repository.DeleteAsync(id); // Deleta a imagem do banco de dados
            if (!deletado)
            {
                return StatusCode(500, "Erro ao deletar a imagem");
            }
            return NoContent(); // Retorna o status 204 No Content
        }
    }
}