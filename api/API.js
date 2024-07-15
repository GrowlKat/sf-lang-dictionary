import axios from "axios";

// API URL
const apiUrl = process.env.NEXT_PUBLIC_API_URL;
console.log("API URL: " + apiUrl);
/**
 * Shortcut for consulting API and recieving it's data
 */
const api = axios.create({baseURL: apiUrl, headers: {"Access-Control-Allow-Origin": "*"}}); // Localhost
api.interceptors.response.use((response) => response.data);
export default api;