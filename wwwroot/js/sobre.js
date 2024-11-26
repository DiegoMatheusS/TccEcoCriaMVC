//Função dos botões de missão, visão e valores.
function toggleContent(id) {
  var content = document.getElementById(id);

  if (content.style.display === "none") {
    content.style.display = "block";
  } else {
    content.style.display = "none";
  }
}


document.addEventListener('DOMContentLoaded', function () {
  document.getElementById('telefone').addEventListener('input', function (e) {
      let value = e.target.value
          .replace(/\D/g, '') // Remove caracteres não numéricos
          .replace(/^(\d{2})(\d)/, '($1) $2') // Formata DDD
          .replace(/(\d{5})(\d)/, '$1-$2') // Formata o número
          .replace(/(\s+|-)+$/, ''); // Remove espaços em branco ou traços no final

      e.target.value = value; // Atualiza o valor do input
  });
});
