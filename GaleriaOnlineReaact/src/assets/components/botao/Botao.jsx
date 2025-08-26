import "./Botao.css"

export const Botao = ({nomeBotao}) => {
    return(
        <button className="botao"  type="submit">
            {nomeBotao}
        </button>
    )
}