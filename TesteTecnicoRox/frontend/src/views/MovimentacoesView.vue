<script setup>
import { onMounted, ref } from 'vue'
import { listarMovimentacoes, removerMovimentacao } from '../services/movimentacaoService'
import ConfirmDialog from '../components/ConfirmDialog.vue'
import { formatarDataHora } from '../utils/data'

const movimentacoes = ref([])
const carregando = ref(true)
const erro = ref('')
const removendoId = ref(null)
const movimentacaoParaRemover = ref(null)

function rotuloTipo(tipo) {
  return tipo === 1 ? 'Entrada' : 'Saída'
}

function classeBadge(tipo) {
  return tipo === 1 ? 'badge badge-entrada' : 'badge badge-saida'
}

async function carregar() {
  carregando.value = true
  erro.value = ''

  try {
    const { data } = await listarMovimentacoes()
    movimentacoes.value = data
  } catch {
    erro.value = 'Não foi possível carregar as movimentações.'
  } finally {
    carregando.value = false
  }
}

function solicitarRemocao(movimentacao) {
  movimentacaoParaRemover.value = movimentacao
}

function cancelarRemocao() {
  movimentacaoParaRemover.value = null
}

async function confirmarRemocao() {
  const movimentacao = movimentacaoParaRemover.value
  if (!movimentacao) return

  erro.value = ''
  removendoId.value = movimentacao.id
  movimentacaoParaRemover.value = null

  try {
    await removerMovimentacao(movimentacao.id)
    movimentacoes.value = movimentacoes.value.filter((m) => m.id !== movimentacao.id)
  } catch (e) {
    erro.value = e.response?.data?.erro ?? 'Não foi possível remover a movimentação.'
  } finally {
    removendoId.value = null
  }
}

onMounted(carregar)
</script>

<template>
  <section>
    <div class="page-header">
      <h1>Movimentações de estoque</h1>
      <RouterLink class="btn btn-primary" to="/movimentacoes/nova">Nova movimentação</RouterLink>
    </div>

    <div class="card">
      <p v-if="erro" class="alert alert-error">{{ erro }}</p>

      <p v-if="carregando" class="empty-state">Carregando movimentações...</p>

      <p v-else-if="movimentacoes.length === 0" class="empty-state">
        Nenhuma movimentação registrada até o momento.
      </p>

      <table v-else>
        <thead>
          <tr>
            <th>Produto</th>
            <th>Tipo</th>
            <th>Quantidade</th>
            <th>Observação</th>
            <th>Data</th>
            <th></th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="movimentacao in movimentacoes" :key="movimentacao.id">
            <td>{{ movimentacao.produtoNome }}</td>
            <td><span :class="classeBadge(movimentacao.tipo)">{{ rotuloTipo(movimentacao.tipo) }}</span></td>
            <td>{{ movimentacao.quantidade }}</td>
            <td class="cell-truncate" :title="movimentacao.observacao || ''">{{ movimentacao.observacao || '—' }}</td>
            <td>{{ formatarDataHora(movimentacao.criadoEm) }}</td>
            <td>
              <div class="row-actions">
                <RouterLink
                  :to="{ name: 'editar-movimentacao', params: { id: movimentacao.id } }"
                  class="icon-btn"
                  title="Editar movimentação"
                >
                  <svg viewBox="0 0 24 24" width="16" height="16" fill="none" stroke="currentColor" stroke-width="2">
                    <path d="M16.5 3.5a2.1 2.1 0 0 1 3 3L7 19l-4 1 1-4Z" stroke-linecap="round" stroke-linejoin="round" />
                  </svg>
                </RouterLink>
                <button
                  class="btn btn-secondary"
                  type="button"
                  :disabled="removendoId === movimentacao.id"
                  @click="solicitarRemocao(movimentacao)"
                >
                  {{ removendoId === movimentacao.id ? 'Removendo...' : 'Remover' }}
                </button>
              </div>
            </td>
          </tr>
        </tbody>
      </table>
    </div>

    <ConfirmDialog
      :aberto="movimentacaoParaRemover !== null"
      titulo="Remover movimentação"
      :mensagem="`Remover esta movimentação de ${rotuloTipo(movimentacaoParaRemover?.tipo ?? 1).toLowerCase()} do produto &quot;${movimentacaoParaRemover?.produtoNome}&quot;? O estoque será ajustado novamente.`"
      texto-confirmar="Remover"
      @confirmar="confirmarRemocao"
      @cancelar="cancelarRemocao"
    />
  </section>
</template>
