﻿const togglePassword = document.querySelector('#togglePassword');const paswd = document.querySelector('#password');togglePassword.addEventListener('click', function (e) {const type = password.getAttribute('type') === 'password' ? 'text' : 'password';paswd.setAttribute('type', type);this.classList.toggle('fa-eye-slash');});