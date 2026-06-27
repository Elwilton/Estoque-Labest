import { createRouter, createWebHistory } from 'vue-router'
import { useAuthStore } from '../stores/auth'

const routes = [
  {
    path: '/login',
    name: 'login',
    component: () => import('../views/LoginView.vue'),
    meta: { publica: true }
  },
  {
    path: '/cadastro',
    name: 'cadastro',
    component: () => import('../views/CadastroView.vue'),
    meta: { publica: true }
  },
  {
    path: '/',
    redirect: '/inicio'
  },
  {
    path: '/inicio',
    name: 'inicio',
    component: () => import('../views/HomeView.vue')
  },
  {
    path: '/produtos',
    name: 'produtos',
    component: () => import('../views/ProdutosView.vue')
  },
  {
    path: '/produtos/novo',
    name: 'novo-produto',
    component: () => import('../views/ProdutoFormView.vue')
  },
  {
    path: '/produtos/:id/editar',
    name: 'editar-produto',
    component: () => import('../views/ProdutoFormView.vue'),
    props: true
  },
  {
    path: '/movimentacoes',
    name: 'movimentacoes',
    component: () => import('../views/MovimentacoesView.vue')
  },
  {
    path: '/movimentacoes/nova',
    name: 'nova-movimentacao',
    component: () => import('../views/MovimentacaoFormView.vue')
  },
  {
    path: '/movimentacoes/:id/editar',
    name: 'editar-movimentacao',
    component: () => import('../views/MovimentacaoFormView.vue'),
    props: true
  }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

router.beforeEach((to) => {
  const authStore = useAuthStore()

  if (!to.meta.publica && !authStore.estaAutenticado) {
    return { name: 'login' }
  }

  if (to.meta.publica && authStore.estaAutenticado) {
    return { name: 'inicio' }
  }

  return true
})

export default router
