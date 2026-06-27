import http from './http'

export function registrar(payload) {
  return http.post('/auth/registrar', payload)
}

export function login(payload) {
  return http.post('/auth/login', payload)
}
