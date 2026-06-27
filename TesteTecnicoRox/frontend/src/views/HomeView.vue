<script setup>
import { useAuthStore } from '../stores/auth'

const authStore = useAuthStore()
</script>

<template>
  <section class="home-screen">
    <div class="card home-card">
      <img
        src="https://images-g0eefrhnbxdrefef.z02.azurefd.net/images/icons/main-logo-2.svg"
        alt="Labest"
        class="home-logo"
      />

      <h1>Bem-vindo, {{ authStore.usuario?.nome }}!</h1>

      <p class="home-text">
        O <strong>Labest Estoque</strong> é o sistema de cadastro de produtos e gestão de
        estoque desenvolvido para o teste técnico da Labest. Aqui você pode cadastrar produtos,
        controlar entradas e saídas do seu estoque e acompanhar o histórico completo de
        movimentações, tudo em um único lugar.
      </p>

      <div class="home-actions">
        <RouterLink class="btn btn-primary" to="/produtos">Ver produtos</RouterLink>
        <RouterLink class="btn btn-secondary" to="/movimentacoes">Ver movimentações</RouterLink>
      </div>
    </div>
  </section>
</template>
