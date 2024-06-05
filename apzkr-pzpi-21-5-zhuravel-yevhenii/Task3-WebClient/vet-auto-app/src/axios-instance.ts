import axios from "axios";
import { accessToken } from "./constants/AuthenticationConstants";

const axiosInstance = axios.create({
  baseURL: import.meta.env.VITE_API_KEY,
});

axiosInstance.interceptors.request.use((config) => {
  const token = localStorage.getItem(accessToken);
  if (accessToken) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

export default axiosInstance;
