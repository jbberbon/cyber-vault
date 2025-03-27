import axios from "axios";
const axiosConfig = axios.create({
  baseURL: "/api",
  timeout: 5000,
  headers: {
    "Accept": "application/json",
    "Content-Type": "application/json",
  },
  withCredentials: true
});

export {axiosConfig};
