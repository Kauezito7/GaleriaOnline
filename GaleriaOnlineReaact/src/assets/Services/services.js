import axios from "axios";

// Deixando o codigo mais limpo e facilitado na troca da porta caso haja mudancas.
const apiPorta = "7000";

//apiLocal ela recebe o endereco da api.
const apiLocal = `https://localhost:${apiPorta}/api/`

// const apiAzure = "https://apieventkaue-dvebe3gch6fgbwdj.canadacentral-01.azurewebsites.net/api/";


//Criamos um acesso que vai ter a base nossa api.
const api = axios.create({
    baseURL: apiLocal
});

export default api;