import './Galeria.css'
import { Botao } from '../../assets/components/botao/Botao'
import { Card } from '../../assets/components/card/Card'

import icon from "../../assets/img/upload.svg"

export const Galeria = () => {
    return(
        <>
        <h1 className='tituloGaleria'>Galeria Online</h1>

        <form className="formulario" onSubmit="" >
            <div className='campoNome'>
                <label className="">Nome</label>
                <input type="text"
                className='inputNome' />
            </div>
            <div className='campoImagem'>
                <label className='arquivoLabel'>
                    <i><img src={icon} alt='Icone de upload de imagem'></img></i>
                    <input type='file' className='arquivoInput'></input>
                </label>
            </div>
            <Botao nomeBotao="Cadastrar"/>
        </form>

        <div className='campoCards'>
            <Card tituloCard="Shurek de zoios"/>
            <Card tituloCard="Shurek de zoios"/>
            <Card tituloCard="Shurek de zoios"/>
            <Card tituloCard="Shurek de zoios"/>
            <Card tituloCard="Shurek de zoios"/>
            <Card tituloCard="Shurek de zoios"/>
            <Card tituloCard="Shurek de zoios"/>
            <Card tituloCard="Shurek de zoios"/>
            <Card tituloCard="Shurek de zoios"/>
            <Card tituloCard="Shurek de zoios"/>
            <Card tituloCard="Shurek de zoios"/>
            <Card tituloCard="Shurek de zoios"/>
            <Card tituloCard="Shurek de zoios"/>
            <Card tituloCard="Shurek de zoios"/>
            <Card tituloCard="Shurek de zoios"/>
            <Card tituloCard="Shurek de zoios"/>
            <Card tituloCard="Shurek de zoios"/>
        </div>
        </>
    )
}