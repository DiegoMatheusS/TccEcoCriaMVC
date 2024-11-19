let currentIndex = 0;
const images = document.querySelectorAll('.carousel img');
const indicators = document.querySelectorAll('.carousel-indicators button');

function updateCarousel(index) {
    const totalImages = images.length;
    currentIndex = (index + totalImages) % totalImages; // Mantém o índice dentro do limite
    const offset = -currentIndex * 100; // Move o carrossel
    document.querySelector('.carousel').style.transform = `translateX(${offset}%)`;

    // Atualiza os indicadores
    indicators.forEach((indicator, i) => {
        indicator.classList.toggle('active', i === currentIndex);
    });
}

// Função para avançar automaticamente
function autoAdvance() {
    updateCarousel(currentIndex + 1);
}

// Intervalo de 5 segundos
setInterval(autoAdvance, 5000); // 5000 milissegundos = 5 segundos

// Eventos dos botões
document.getElementById('prev').addEventListener('click', () => updateCarousel(currentIndex - 1));
document.getElementById('next').addEventListener('click', () => updateCarousel(currentIndex + 1));

// Eventos dos indicadores
indicators.forEach((indicator, i) => {
    indicator.addEventListener('click', () => updateCarousel(i));
});


//Função para formatação do cep no card.
function formatarCEP(input) {
    // Remove caracteres não numéricos
    let valor = input.value.replace(/\D/g, '');

    // Adiciona o traço automaticamente
    if (valor.length > 5) {
        valor = valor.slice(0, 5) + '-' + valor.slice(5);
    }

    // Atualiza o valor do input
    input.value = valor;
}

//Função para voltar a página para cima
const toTop = document.querySelector(".to-top");

window.addEventListener("scroll", () => {
    if(window.scrollY > 1500){
        toTop.classList.add("active");
    }
    else{
        toTop.classList.remove("active");
    }
})
