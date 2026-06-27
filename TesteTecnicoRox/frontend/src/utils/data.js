export function formatarDataHora(valor) {
  const textoIso = valor.endsWith('Z') ? valor : `${valor}Z`
  return new Date(textoIso).toLocaleString('pt-BR')
}
