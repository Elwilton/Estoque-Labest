import http from './http'

export function listarMovimentacoes() {
  return http.get('/movimentacoes')
}

export function obterMovimentacao(id) {
  return http.get(`/movimentacoes/${id}`)
}

export function registrarMovimentacao(payload) {
  return http.post('/movimentacoes', payload)
}

export function atualizarMovimentacao(id, payload) {
  return http.put(`/movimentacoes/${id}`, payload)
}

export function removerMovimentacao(id) {
  return http.delete(`/movimentacoes/${id}`)
}
