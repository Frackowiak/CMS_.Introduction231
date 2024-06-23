$(document).ready(function () {
    const apiBaseUrl = 'https://cmsaleksandercdv2.azurewebsites.net/';
    const token = localStorage.getItem('token');
    if (!token) {
        window.location.href = 'index.html';
        return;
    }
    $('#logoutButton').on('click', function () {
        localStorage.removeItem('token');
        window.location.href = 'index.html';
    });
    $('#addContactButton').on('click', function () {
        window.location.href = 'contactForm.html';
    });
    function fetchContacts() {
        $.ajax({
            type: 'GET',
            url: `${apiBaseUrl}/Contacts`,
            headers: { 'Authorization': 'Bearer ' + token },
            success: function (response) {
                const contactsList = $('#contactsList');
                contactsList.empty();
                response.forEach(contact => {
                    contactsList.append(`
                <div class="contact">
                <p>${contact.firstName} ${contact.lastName}</p>
                <p>Email: <a href="mailto:${contact.email}">${contact.email}</a></p>
                <p>Phone: <a href="tel:${contact.phoneNumber}">${contact.phoneNumber}</a></p>
                <div class="buttons">
                <button onclick="editContact('${contact.id}')">Edit</button>
                <button onclick="deleteContact('${contact.id}')">Delete</button>
                </div>
                </div>
            `);
                });
            },
            error: function () {
                alert('Failed to fetch contacts. Please login again.');
                localStorage.removeItem('token');
                window.location.href = 'index.html';
            }
        });
    }
    fetchContacts();
    window.editContact = function (id) {
        window.location.href = `contactForm.html?id=${id}`;
    };
    window.deleteContact = function (id) {
        if (!confirm("Are you sure you want to delete this contact?")) {
            return;
        }
        $.ajax({
            type: 'DELETE',
            url: `${apiBaseUrl}/Contacts/${id}`,
            headers: { 'Authorization': 'Bearer ' + token },
            success: function () {
                alert('Contact deleted successfully!');
                fetchContacts();
            },
            error: function () {
                alert('Failed to delete contact. Please try again.');
            }
        });
    };
});