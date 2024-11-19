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

// Eventos dos botões
document.getElementById('prev').addEventListener('click', () => updateCarousel(currentIndex - 1));
document.getElementById('next').addEventListener('click', () => updateCarousel(currentIndex + 1));

// Eventos dos indicadores
indicators.forEach((indicator, i) => {
    indicator.addEventListener('click', () => updateCarousel(i));
});