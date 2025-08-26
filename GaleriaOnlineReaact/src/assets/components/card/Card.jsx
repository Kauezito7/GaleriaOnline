import "./Card.css";

import imgPen from "../../img/pen.svg";
import imgTrash from "../../img/trash.svg";

export const Card = ({ tituloCard, imgCard, funcaoEditar, funcaoExcluir }) => {
    return (
        <>
            <div className="cardDaImagem" >
                <p>{tituloCard}</p>
                <img className="imgDoCard" src={imgCard} alt="Imagem relacionada ao card" />
                <div className="icons">
                    <img src={imgPen} onClick={funcaoEditar} alt="Icone de caneta pararealizar uma alteracao" />
                    <img src={imgTrash} onClick={funcaoExcluir} alt="icone de uma lixeira para realizar a exclusao" />
                </div>
            </div>
        </>
    )
}