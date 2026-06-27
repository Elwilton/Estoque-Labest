<script setup>
import { computed } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from './stores/auth'

const authStore = useAuthStore()
const router = useRouter()

const estaAutenticado = computed(() => authStore.estaAutenticado)

function sair() {
  authStore.logout()
  router.push({ name: 'login' })
}
</script>

<template>
  <div class="app-shell">
    <header v-if="estaAutenticado" class="app-header">
      <RouterLink to="/inicio" class="app-brand">
        <img src="https://images-g0eefrhnbxdrefef.z02.azurefd.net/images/icons/main-logo-2.svg" alt="Labest" class="app-logo" />
      </RouterLink>

      <nav class="app-nav">
        <RouterLink to="/inicio">Início</RouterLink>
        <RouterLink to="/produtos">Produtos</RouterLink>
        <RouterLink to="/movimentacoes">Movimentações</RouterLink>
      </nav>

      <div class="app-user">
        <span>{{ authStore.usuario?.nome }}</span>
        <button class="btn btn-logout" type="button" @click="sair">Sair</button>
      </div>
    </header>

    <main class="app-content">
      <RouterView />
    </main>
  </div>
</template>
