document.addEventListener('DOMContentLoaded', function () {
    // Toggle do menu hambúrguer
    const hamburger = document.querySelector('.hamburger');
    const navMenu = document.querySelector('.nav-menu');

    hamburger.addEventListener('click', function () {
        navMenu.classList.toggle('active');
    });

    // Toggle do dropdown do perfil
    const loginDropdown = document.querySelector('.login-dropdown');

    loginDropdown.addEventListener('click', function (e) {
        e.stopPropagation();
        loginDropdown.classList.toggle('show');
    });

    // Fecha o dropdown ao clicar fora
    document.addEventListener('click', function () {
        loginDropdown.classList.remove('show');
    });
});

loginDropdown.addEventListener('click', function (e) {
    e.stopPropagation();
    console.log('Dropdown clicado'); // Adicionado para depurar
    loginDropdown.classList.toggle('show');
});