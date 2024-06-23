$(document).ready(function () {
    const apiBaseUrl = 'https://cmsaleksandercdv2.azurewebsites.net/';

    const token = localStorage.getItem('token');
        if (window.location.pathname.endsWith('index.html') || window.location.pathname === '/') {
            if (token) {
                window.location.href = 'contacts.html';
                return;
            }
        }

    // Adjust to your API URL
    $('#loginForm').on('submit', function (e) {
        e.preventDefault();
        const username = $('#username').val();
        const password = $('#password').val();

        $.ajax({
            type: 'POST',
            url: `${apiBaseUrl}/auth/login`,
            contentType: 'application/json',
            data: JSON.stringify({ Username: username, Password: password }),
            success: function (response) {
                localStorage.setItem('token', response.token);
                window.location.href = 'contacts.html';
            },

            error: function () {
                alert('Login failed. Please check your credentials.');
            }
        });
    });

    $('#registerForm').on('submit', function (e) {

        e.preventDefault();
        const username = $('#regUsername').val();
        const password = $('#regPassword').val();

        $.ajax({
            type: 'POST',
            url: `${apiBaseUrl}/auth/register`,
            contentType: 'application/json',
            data: JSON.stringify({ Username: username, Password: password }),
            success: function () {
                alert('Registration successful! Please login.');
                window.location.href = 'index.html';
            },

            error: function () {
                alert('Registration failed. Please try again.');
            }
        });
    });


    if (window.location.pathname.endsWith('index.html')) {
        const token = localStorage.getItem('token');
        if (token) {
            window.location.href = 'contacts.html';
            return;
        }
    }

    if (window.location.pathname.endsWith('contacts.html')) {
        const token = localStorage.getItem('token');
        if (!token) {
            window.location.href = 'index.html';
            return;
        }

        $.ajax({
            type: 'GET',
            url: `${apiBaseUrl}/contacts`,
            headers: {
                'Authorization': 'Bearer ' + token
            },

            success: function (response) {
                const contactsList = $('#contactsList');
                contactsList.empty();
                response.forEach(contact => {
                    contactsList.append(`<p>${contact.firstName} ${contact.lastName}</p>`);
                });
            },

            error: function () {
                alert('Failed to fetch contacts. Please login again.');
                localStorage.removeItem('token');
                window.location.href = 'index.html';
            }
        });

        $('#logoutButton').on('click', function () {
            localStorage.removeItem('token');
            window.location.href = 'index.html';
        });
    }
});