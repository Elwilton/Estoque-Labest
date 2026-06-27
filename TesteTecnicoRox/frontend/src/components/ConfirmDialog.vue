<script setup>
defineProps({
  aberto: { type: Boolean, required: true },
  titulo: { type: String, default: 'Confirmar ação' },
  mensagem: { type: String, required: true },
  textoConfirmar: { type: String, default: 'Confirmar' },
  textoCancelar: { type: String, default: 'Cancelar' }
})

const emit = defineEmits(['confirmar', 'cancelar'])
</script>

<template>
  <Teleport to="body">
    <div v-if="aberto" class="modal-overlay" @click.self="emit('cancelar')">
      <div class="modal-card">
        <h2>{{ titulo }}</h2>
        <p>{{ mensagem }}</p>
        <div class="modal-actions">
          <button class="btn btn-secondary" type="button" @click="emit('cancelar')">
            {{ textoCancelar }}
          </button>
          <button class="btn btn-danger" type="button" @click="emit('confirmar')">
            {{ textoConfirmar }}
          </button>
        </div>
      </div>
    </div>
  </Teleport>
</template>
