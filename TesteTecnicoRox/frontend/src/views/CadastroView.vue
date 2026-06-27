<script setup>
import { reactive, ref } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '../stores/auth'

const router = useRouter()
const authStore = useAuthStore()

const form = reactive({
  nome: '',
  email: '',
  senha: ''
})

const erro = ref('')
const carregando = ref(false)

async function aoEnviar() {
  erro.value = ''
  carregando.value = true

  try {
    await authStore.registrar(form)
    router.push({ name: 'inicio' })
  } catch (e) {
    erro.value = e.response?.data?.erro ?? 'Não foi possível concluir o cadastro.'
  } finally {
    carregando.value = false
  }
}
</script>

<template>
  <div class="auth-screen">
    <div class="card auth-card">
      <h1>Criar conta</h1>
      <p class="subtitle">Cadastre-se para gerenciar seu estoque</p>

      <p v-if="erro" class="alert alert-error">{{ erro }}</p>

      <form class="form-grid" @submit.prevent="aoEnviar">
        <div class="form-field">
          <label for="nome">Nome</label>
          <input id="nome" v-model="form.nome" type="text" required autocomplete="name" />
        </div>

        <div class="form-field">
          <label for="email">E-mail</label>
          <input id="email" v-model="form.email" type="email" required autocomplete="email" />
        </div>

        <div class="form-field">
          <label for="senha">Senha</label>
          <input id="senha" v-model="form.senha" type="password" required minlength="6" autocomplete="new-password" />
        </div>

        <button class="btn btn-primary" type="submit" :disabled="carregando">
          {{ carregando ? 'Cadastrando...' : 'Cadastrar' }}
        </button>
      </form>

      <p class="auth-footer">
        Já tem uma conta? <RouterLink to="/login">Entrar</RouterLink>
      </p>
    </div>
  </div>
</template>
