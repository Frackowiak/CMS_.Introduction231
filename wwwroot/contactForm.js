$(document).ready(function () {
    const apiBaseUrl = 'https://cmsaleksandercdv2.azurewebsites.net/';
    const token = localStorage.getItem('token');
    const urlParams = new URLSearchParams(window.location.search);
    const contactId = urlParams.get('id');
    if (!token) {
        window.location.href = 'index.html';
        return;
    }
    // Check if we are editing an existing contact
    if (contactId) {
        $.ajax({
            type: 'GET',
            url: `${apiBaseUrl}/Contacts/${contactId}`,
            headers: { 'Authorization': 'Bearer ' + token },
            success: function (contact) {
                $('#contactId').val(contact.id);
                $('#firstName').val(contact.firstName);
                $('#lastName').val(contact.lastName);
                $('#phoneNumber').val(contact.phoneNumber);
                $('#email').val(contact.email);
                $('#notes').val(contact.notes);
            },
            error: function () {
                alert('Failed to fetch contact details.');
            }
        });
    }
    $('#contactForm').on('submit', function (e) {
        e.preventDefault();
        const Id = $('#contactId').val();
        const contact = {
            FirstName: $('#firstName').val(),
            LastName: $('#lastName').val(),
            PhoneNumber: $('#phoneNumber').val(),
            Email: $('#email').val(),
            Notes: $('#notes').val()
        };
        if (Id) {
            // Edit existing contact
            $.ajax({
                type: 'PUT',
                url: `${apiBaseUrl}/Contacts/${Id}`,
                contentType: 'application/json',
                headers: { 'Authorization': 'Bearer ' + token },
                data: JSON.stringify(contact),
                success: function () {
                    alert('Contact updated successfully!');
                    window.location.href = 'contacts.html';
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    alert('Failed to update contact. Please try again.');
                    console.error(jqXHR.responseText, textStatus, errorThrown);
                }
            });
        } else {
            // Add new contact
            $.ajax({
                type: 'POST',
                url: `${apiBaseUrl}/Contacts`,
                contentType: 'application/json',
                headers: { 'Authorization': 'Bearer ' + token },
                data: JSON.stringify(contact),
                success: function () {
                    alert('Contact added successfully!');
                    window.location.href = 'contacts.html';
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    alert('Failed to add contact. Please try again.');
                    console.error(jqXHR.responseText, textStatus, errorThrown);
                }
            });
        }
    });
});