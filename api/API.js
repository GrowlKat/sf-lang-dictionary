import axios from "axios";

/**
 * Shortcut for consulting API and recieving it's data
 */
const api = axios.create({baseURL: 'https://sflangmodels.azurewebsites.net/api'});
api.interceptors.response.use((response) => response.data);
export default api;