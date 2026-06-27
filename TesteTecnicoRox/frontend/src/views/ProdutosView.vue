<script setup>
import { onMounted, ref } from 'vue'
import { listarProdutos, removerProduto } from '../services/produtoService'
import ConfirmDialog from '../components/ConfirmDialog.vue'

const produtos = ref([])
const carregando = ref(true)
const erro = ref('')
const removendoId = ref(null)
const produtoParaRemover = ref(null)

function formatarMoeda(valor) {
  return valor.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })
}

async function carregar() {
  carregando.value = true
  erro.value = ''

  try {
    const { data } = await listarProdutos()
    produtos.value = data
  } catch {
    erro.value = 'Não foi possível carregar os produtos.'
  } finally {
    carregando.value = false
  }
}

function solicitarRemocao(produto) {
  produtoParaRemover.value = produto
}

function cancelarRemocao() {
  produtoParaRemover.value = null
}

async function confirmarRemocao() {
  const produto = produtoParaRemover.value
  if (!produto) return

  erro.value = ''
  removendoId.value = produto.id
  produtoParaRemover.value = null

  try {
    await removerProduto(produto.id)
    produtos.value = produtos.value.filter((p) => p.id !== produto.id)
  } catch (e) {
    erro.value = e.response?.data?.erro ?? 'Não foi possível remover o produto.'
  } finally {
    removendoId.value = null
  }
}

onMounted(carregar)
</script>

<template>
  <section>
    <div class="page-header">
      <h1>Produtos</h1>
      <RouterLink class="btn btn-primary" to="/produtos/novo">Novo produto</RouterLink>
    </div>

    <div class="card">
      <p v-if="erro" class="alert alert-error">{{ erro }}</p>

      <p v-if="carregando" class="empty-state">Carregando produtos...</p>

      <p v-else-if="produtos.length === 0" class="empty-state">
        Nenhum produto cadastrado até o momento.
      </p>

      <table v-else>
        <thead>
          <tr>
            <th>Nome</th>
            <th>Descrição</th>
            <th>Preço</th>
            <th>Estoque</th>
            <th></th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="produto in produtos" :key="produto.id">
            <td>{{ produto.nome }}</td>
            <td>{{ produto.descricao || '—' }}</td>
            <td>{{ formatarMoeda(produto.preco) }}</td>
            <td>{{ produto.quantidadeEmEstoque }}</td>
            <td>
              <div class="row-actions">
                <RouterLink
                  :to="{ name: 'editar-produto', params: { id: produto.id } }"
                  class="icon-btn"
                  title="Editar produto"
                >
                  <svg viewBox="0 0 24 24" width="16" height="16" fill="none" stroke="currentColor" stroke-width="2">
                    <path d="M16.5 3.5a2.1 2.1 0 0 1 3 3L7 19l-4 1 1-4Z" stroke-linecap="round" stroke-linejoin="round" />
                  </svg>
                </RouterLink>
                <button
                  class="btn btn-secondary"
                  type="button"
                  :disabled="removendoId === produto.id"
                  @click="solicitarRemocao(produto)"
                >
                  {{ removendoId === produto.id ? 'Removendo...' : 'Remover' }}
                </button>
              </div>
            </td>
          </tr>
        </tbody>
      </table>
    </div>

    <ConfirmDialog
      :aberto="produtoParaRemover !== null"
      titulo="Remover produto"
      :mensagem="`Remover o produto &quot;${produtoParaRemover?.nome}&quot;? Esta ação não pode ser desfeita.`"
      texto-confirmar="Remover"
      @confirmar="confirmarRemocao"
      @cancelar="cancelarRemocao"
    />
  </section>
</template>
