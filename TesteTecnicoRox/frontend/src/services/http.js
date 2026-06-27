import axios from 'axios'

const http = axios.create({
  baseURL: import.meta.env.VITE_API_URL ?? 'http://localhost:5000/api'
})

http.interceptors.request.use((config) => {
  const token = localStorage.getItem('rox_token')
  if (token) {
    config.headers.Authorization = `Bearer ${token}`
  }
  return config
})

http.interceptors.response.use(
  (response) => response,
  (error) => {
    const ehRotaDeAutenticacao = error.config?.url?.startsWith('/auth/')

    if (error.response?.status === 401 && !ehRotaDeAutenticacao) {
      localStorage.removeItem('rox_token')
      localStorage.removeItem('rox_user')
      window.location.href = '/login'
    }

    return Promise.reject(error)
  }
)

export default http
