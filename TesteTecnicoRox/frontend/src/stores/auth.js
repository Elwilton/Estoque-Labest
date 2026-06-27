import { defineStore } from 'pinia'
import { login as loginRequest, registrar as registrarRequest } from '../services/authService'

export const useAuthStore = defineStore('auth', {
  state: () => ({
    token: localStorage.getItem('rox_token') ?? null,
    usuario: JSON.parse(localStorage.getItem('rox_user') ?? 'null')
  }),

  getters: {
    estaAutenticado: (state) => Boolean(state.token)
  },

  actions: {
    persistirSessao(resposta) {
      this.token = resposta.token
      this.usuario = { nome: resposta.nome, email: resposta.email }
      localStorage.setItem('rox_token', resposta.token)
      localStorage.setItem('rox_user', JSON.stringify(this.usuario))
    },

    async login(credenciais) {
      const { data } = await loginRequest(credenciais)
      this.persistirSessao(data)
    },

    async registrar(dados) {
      const { data } = await registrarRequest(dados)
      this.persistirSessao(data)
    },

    logout() {
      this.token = null
      this.usuario = null
      localStorage.removeItem('rox_token')
      localStorage.removeItem('rox_user')
    }
  }
})
