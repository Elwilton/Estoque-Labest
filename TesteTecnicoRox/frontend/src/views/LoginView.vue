<script setup>
import { reactive, ref } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '../stores/auth'

const router = useRouter()
const authStore = useAuthStore()

const form = reactive({
  email: '',
  senha: ''
})

const erro = ref('')
const carregando = ref(false)

async function aoEnviar() {
  erro.value = ''
  carregando.value = true

  try {
    await authStore.login(form)
    router.push({ name: 'inicio' })
  } catch (e) {
    erro.value = e.response?.data?.erro ?? 'Não foi possível efetuar o login.'
  } finally {
    carregando.value = false
  }
}
</script>

<template>
  <div class="auth-screen">
    <div class="card auth-card">
      <img
        src="https://images-g0eefrhnbxdrefef.z02.azurefd.net/images/icons/main-logo-2.svg"
        alt="Labest"
        class="auth-logo"
      />

      <h1>Entrar</h1>
      <p class="subtitle">Acesse o sistema de gestão de estoque</p>

      <form class="form-grid" @submit.prevent="aoEnviar">
        <div class="form-field">
          <label for="email">E-mail</label>
          <input id="email" v-model="form.email" type="email" required autocomplete="email" />
        </div>

        <div class="form-field">
          <label for="senha">Senha</label>
          <input id="senha" v-model="form.senha" type="password" required autocomplete="current-password" />
        </div>

        <p v-if="erro" class="alert alert-error">{{ erro }}</p>

        <button class="btn btn-primary" type="submit" :disabled="carregando">
          {{ carregando ? 'Entrando...' : 'Entrar' }}
        </button>
      </form>

      <p class="auth-footer">
        Não tem uma conta? <RouterLink to="/cadastro">Cadastre-se</RouterLink>
      </p>
    </div>
  </div>
</template>
