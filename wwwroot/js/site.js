// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

    document.addEventListener("DOMContentLoaded", function () {
        const loginDropdown = document.querySelector('.login-dropdown')
        const loginIcon = document.querySelector('.login-icon')

        // Adiciona evento de clique no ícone de login
        loginIcon.addEventListener('click', function () {
            loginDropdown.classList.toggle('show');
        });

        // Fechar o dropdown se clicar fora, exceto nos links dentro do dropdown
        window.addEventListener('click', function (e) {
            if (!loginDropdown.contains(e.target)) {
                loginDropdown.classList.remove('show')
            }
        });
    });