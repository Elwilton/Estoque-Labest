<script setup>
import { computed, onMounted, reactive, ref } from 'vue'
import { useRouter } from 'vue-router'
import { criarProduto, atualizarProduto, obterProduto } from '../services/produtoService'

const props = defineProps({
  id: { type: String, default: null }
})

const router = useRouter()
const ehEdicao = computed(() => Boolean(props.id))

const form = reactive({
  nome: '',
  descricao: '',
  quantidadeInicial: 0
})

const precoTexto = ref('0,00')
const erro = ref('')
const salvando = ref(false)
const carregando = ref(false)

function formatarPrecoTexto(valor) {
  return valor.toFixed(2).replace('.', ',')
}

function converterPrecoTexto(texto) {
  const normalizado = texto.trim().replace(/\./g, '').replace(',', '.')
  const valor = parseFloat(normalizado)
  return Number.isFinite(valor) ? valor : 0
}

async function carregarProduto() {
  carregando.value = true

  try {
    const { data } = await obterProduto(props.id)
    form.nome = data.nome
    form.descricao = data.descricao ?? ''
    precoTexto.value = formatarPrecoTexto(data.preco)
  } catch {
    erro.value = 'Não foi possível carregar os dados do produto.'
  } finally {
    carregando.value = false
  }
}

async function aoEnviar() {
  erro.value = ''
  salvando.value = true

  const preco = converterPrecoTexto(precoTexto.value)

  try {
    if (ehEdicao.value) {
      await atualizarProduto(props.id, {
        nome: form.nome,
        descricao: form.descricao,
        preco
      })
    } else {
      await criarProduto({ ...form, preco })
    }

    router.push({ name: 'produtos' })
  } catch (e) {
    erro.value = e.response?.data?.erro ?? 'Não foi possível salvar o produto.'
  } finally {
    salvando.value = false
  }
}

onMounted(() => {
  if (ehEdicao.value) {
    carregarProduto()
  }
})
</script>

<template>
  <section>
    <div class="page-header">
      <h1>{{ ehEdicao ? 'Editar produto' : 'Novo produto' }}</h1>
    </div>

    <div class="card">
      <p v-if="erro" class="alert alert-error">{{ erro }}</p>

      <p v-if="carregando" class="empty-state">Carregando dados do produto...</p>

      <form v-else class="form-grid" @submit.prevent="aoEnviar">
        <div class="form-field">
          <label for="nome">Nome</label>
          <input id="nome" v-model="form.nome" type="text" required />
        </div>

        <div class="form-field">
          <label for="descricao">Descrição</label>
          <textarea id="descricao" v-model="form.descricao" rows="3"></textarea>
        </div>

        <div class="form-field">
          <label for="preco">Preço</label>
          <input
            id="preco"
            v-model="precoTexto"
            type="text"
            inputmode="decimal"
            placeholder="0,00"
            required
          />
        </div>

        <div v-if="!ehEdicao" class="form-field">
          <label for="quantidade">Quantidade inicial</label>
          <input id="quantidade" v-model.number="form.quantidadeInicial" type="number" min="0" required />
        </div>

        <div style="display: flex; gap: 0.75rem;">
          <button class="btn btn-primary" type="submit" :disabled="salvando">
            {{ salvando ? 'Salvando...' : 'Salvar' }}
          </button>
          <RouterLink class="btn btn-secondary" to="/produtos">Cancelar</RouterLink>
        </div>
      </form>
    </div>
  </section>
</template>
