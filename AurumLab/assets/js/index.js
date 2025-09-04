


document.addEventListener('DOMContentLoaded', function() {
    const form = document.querySelector('.form-login');
    const emailInput = document.getElementById('email');
    const senhaInput = document.getElementById('senha');
    const iconeOlho = document.querySelector('.icone-olho');
    const erroLogin = document.querySelector('.erro-login');

    // Hide error message initially
    erroLogin.style.display = 'none';

    // Toggle password visibility
    iconeOlho.addEventListener('click', function() {
        if (senhaInput.type === 'password') {
            senhaInput.type = 'text';
            // Optionally change icon to closed eye if available
        } else {
            senhaInput.type = 'password';
            // Change back to open eye
        }
    });

    // Form submission
    form.addEventListener('submit', function(event) {
        event.preventDefault();

        const email = emailInput.value.trim();
        const senha = senhaInput.value.trim();

        // Simple validation
        if (email === '' || senha === '') {
            erroLogin.textContent = 'Por favor, preencha todos os campos.';
            erroLogin.style.display = 'block';
            return;
        }

        // Simulate login check (replace with actual authentication)
        if (email === 'admin@aurumlab.com' && senha === '123456') {
            // Successful login
            window.location.href = 'home.html';
        } else {
            // Failed login
            erroLogin.textContent = 'E-mail ou senha incorretos.';
            erroLogin.style.display = 'block';
        }
    });
});
