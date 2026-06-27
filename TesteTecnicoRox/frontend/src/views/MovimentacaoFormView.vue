<script setup>
import { computed, onMounted, reactive, ref } from 'vue'
import { useRouter } from 'vue-router'
import { listarProdutos } from '../services/produtoService'
import { registrarMovimentacao, atualizarMovimentacao, obterMovimentacao } from '../services/movimentacaoService'

const props = defineProps({
  id: { type: String, default: null }
})

const router = useRouter()
const ehEdicao = computed(() => Boolean(props.id))

const produtos = ref([])
const carregandoProdutos = ref(true)
const erro = ref('')
const salvando = ref(false)

const form = reactive({
  produtoId: '',
  tipo: 1,
  quantidade: 1,
  observacao: ''
})

async function carregarProdutos() {
  carregandoProdutos.value = true

  try {
    const { data } = await listarProdutos()
    produtos.value = data
  } finally {
    carregandoProdutos.value = false
  }
}

async function carregarMovimentacao() {
  try {
    const { data } = await obterMovimentacao(props.id)
    form.produtoId = data.produtoId
    form.tipo = data.tipo
    form.quantidade = data.quantidade
    form.observacao = data.observacao ?? ''
  } catch {
    erro.value = 'Não foi possível carregar os dados da movimentação.'
  }
}

async function aoEnviar() {
  erro.value = ''
  salvando.value = true

  try {
    if (ehEdicao.value) {
      await atualizarMovimentacao(props.id, form)
    } else {
      await registrarMovimentacao(form)
    }

    router.push({ name: 'movimentacoes' })
  } catch (e) {
    erro.value = e.response?.data?.erro ?? 'Não foi possível salvar a movimentação.'
  } finally {
    salvando.value = false
  }
}

onMounted(async () => {
  await carregarProdutos()

  if (ehEdicao.value) {
    await carregarMovimentacao()
  }
})
</script>

<template>
  <section>
    <div class="page-header">
      <h1>{{ ehEdicao ? 'Editar movimentação' : 'Nova movimentação' }}</h1>
    </div>

    <div class="card">
      <div v-if="!ehEdicao && !carregandoProdutos && produtos.length === 0" class="empty-state-cta">
        <p>Ainda não possui nenhum Produto cadastrado, faça o cadastro para criar uma nova movimentação.</p>
        <RouterLink class="btn btn-primary" to="/produtos/novo">Cadastrar produto</RouterLink>
      </div>

      <template v-else>
        <p v-if="erro" class="alert alert-error">{{ erro }}</p>

        <form class="form-grid" @submit.prevent="aoEnviar">
          <div class="form-field">
            <label for="produto">Produto</label>
            <select id="produto" v-model="form.produtoId" required>
              <option value="" disabled>Selecione um produto</option>
              <option v-for="produto in produtos" :key="produto.id" :value="produto.id">
                {{ produto.nome }} (estoque: {{ produto.quantidadeEmEstoque }})
              </option>
            </select>
          </div>

          <div class="form-field">
            <label for="tipo">Tipo</label>
            <select id="tipo" v-model.number="form.tipo" required>
              <option :value="1">Entrada</option>
              <option :value="2">Saída</option>
            </select>
          </div>

          <div class="form-field">
            <label for="quantidade">Quantidade</label>
            <input id="quantidade" v-model.number="form.quantidade" type="number" min="1" required />
          </div>

          <div class="form-field">
            <label for="observacao">Observação</label>
            <textarea id="observacao" v-model="form.observacao" rows="3"></textarea>
          </div>

          <div style="display: flex; gap: 0.75rem;">
            <button class="btn btn-primary" type="submit" :disabled="salvando">
              {{ salvando ? 'Salvando...' : 'Salvar' }}
            </button>
            <RouterLink class="btn btn-secondary" to="/movimentacoes">Cancelar</RouterLink>
          </div>
        </form>
      </template>
    </div>
  </section>
</template>
