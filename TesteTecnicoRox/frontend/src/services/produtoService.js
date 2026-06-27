import http from './http'

export function listarProdutos() {
  return http.get('/produtos')
}

export function obterProduto(id) {
  return http.get(`/produtos/${id}`)
}

export function criarProduto(payload) {
  return http.post('/produtos', payload)
}

export function atualizarProduto(id, payload) {
  return http.put(`/produtos/${id}`, payload)
}

export function removerProduto(id) {
  return http.delete(`/produtos/${id}`)
}
