import axios from "axios";
const axiosConfig = axios.create({
  baseURL: "/api",
  timeout: 1000 * 60 * 3, // 3 min
  headers: {
    "Accept": "application/json",
    "Content-Type": "application/json",
  },
  withCredentials: true
});

export {axiosConfig};
