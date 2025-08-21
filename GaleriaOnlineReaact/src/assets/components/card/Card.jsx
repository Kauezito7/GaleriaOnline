import "./Card.css";

import imgCard from "../../img/imagem.png";
import imgPen from "../../img/pen.svg";
import imgTrash from "../../img/trash.svg";

export const Card = ({ tituloCard }) => {
    return (
        <>
            <div className="cardDaImagem">
                <p>{tituloCard}</p>
                <img className="imgDoCard" src={imgCard} alt="Imagem relacionada ao card" />
                <div className="icons">
                    <img src={imgPen} alt="Icone de caneta pararealizar uma alteracao" />
                    <img src={imgTrash} alt="icone de uma lixeira para realizar a exclusao" />
                </div>
            </div>
        </>
    )
}