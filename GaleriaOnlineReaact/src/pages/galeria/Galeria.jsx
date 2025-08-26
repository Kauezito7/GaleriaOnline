import './Galeria.css'
import { Botao } from '../../assets/components/botao/Botao'
import { Card } from '../../assets/components/card/Card'
import { useEffect } from 'react'
import { useState } from 'react'
import api from '../../assets/Services/services'

import icon from "../../assets/img/upload.svg"
import { use } from 'react'


export const Galeria = () => {

    const [cards, setCards] = useState([])

    const [imagem, setImagem] = useState()

    const [nomeImagem, setNomeImagem] = useState("")

    async function listarCards() {
        try {
            const resposta = await api.get("Imagem");
            // console.log(resposta.data)
            setCards(resposta.data)

        } catch (error) {
            console.log(error)
            alert("Erro ao listar cards")
        }
    }

    async function cadastrarCard(e) {
        e.preventDefault();
        if (imagem && nomeImagem) {

            try {
                const formData = new FormData(); // Cria um objeto FormData para enviar dados do formulário
                formData.append("Nome", nomeImagem); // Adiciona o nome da imagem ao FormData
                formData.append("Arquivo", imagem); // Adiciona o arquivo de imagem ao FormData

                const resposta = await api.post("Imagem/upload", formData, {  // Envia o FormData para o endpoint de upload
                    headers: {
                        "Content-Type": "multipart/form-data"  // Define o cabeçalho para indicar que é um upload de arquivo, (essencial para uploads)
                    }
                });

                console.log(resposta.data)
                alert("Card cadastrado com sucesso!")
                setNomeImagem("")
                setImagem("")
                listarCards(); 

            } catch (error) {
                alert("Nao foi possivel realizar o cadastro!")
                console.error(error);
            }
        } else {
            alert("Por favor, preencha todos os campos!")
        }
    }

     function editarCard(id, nomeAntigo) {
        const novoNome = prompt("Digite o novo nome da imagem:", nomeAntigo);

        const inputArquivo = document.createElement("input");
        inputArquivo.type = "file";
        //Aceita imagens independente das extensões
        inputArquivo.accept = "image/*";
        inputArquivo.style = "display: none";
        // <input type="file" accept="image/*"></input>

        // Define o que acontece quando o usuário selecionar um arquivo
        inputArquivo.onchange = async (e) => {
            const novoArquivo = e.target.files[0];

            const formData = new FormData();
            //adicionar o novo nome no formData:
            formData.append("Nome", novoNome);
            formData.append("Arquivo", novoArquivo);

            if (formData) {
                try {
                    await api.put(`Imagem/${id}`, formData, {
                        headers: {
                            "Content-Type": "multipart/form-data"
                        }
                    })

                    alert("Ebaaa deu certo!😁✨");
                    listarCards();
                } catch (error) {
                    alert("Não foi possível alterar o card!");
                    console.error(error);
                }
            }
        };


        inputArquivo.click();

    }


    async function excluirCard(id) {
        try {
            const resposta = await api.delete(`Imagem/${id}`);
            console.log(resposta.data)
            alert("Card excluido com sucesso!")
            listarCards(); 
        } catch (error) {
            alert("ebbaaaaa, excluiu")
            console.log(error);


        }
    }
    useEffect(() => {
        listarCards();
    }, []);

    return (
        <>
            <h1 className='tituloGaleria'>Galeria Online</h1>

            <form className="formulario" onSubmit={cadastrarCard} >
                <div className='campoNome'>
                    <label className="">Nome</label>
                    <input
                        type="text"
                        className='inputNome'
                        onChange={(e) => setNomeImagem(e.target.value)}
                        value={nomeImagem}
                    />
                </div>

                <div className='campoImagem'>
                    <label className='arquivoLabel'>
                        <i><img src={icon} alt='Icone de upload de imagem'></img></i>
                        <input
                            type='file'
                            className='arquivoInput'
                            onChange={(e) => setImagem(e.target.files[0])}

                        >
                        </input>
                    </label>
                </div>
                <Botao nomeBotao="Cadastrar" />
            </form>

            <div className='campoCards'>
                {cards.length > 0 ? (
                    cards.map((e) => (
                        <Card
                            key={e.id}
                            tituloCard={e.nome} // Passando o nome do card
                            imgCard={`https://localhost:7000/${e.caminho.replace("wwwroot/", "")}`} // Ajuste para acessar a imagem corretamente
                            funcaoExcluir={() => excluirCard(e.id)} // Passando a função de exclusão 
                            funcaoEditar={() => editarCard(e.id, e.nome)} // Passando a função de edição
                        />
                    ))
                ) : (
                    <p>Nenhum card encontrado.</p>
                )
                }



            </div>
        </>
    )
}