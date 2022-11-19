// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


//carCount();

function carCount() {
    $.ajax({
        url: `/Carts/Count-Cart/`,
        method: 'GET',
        success: function (response) {

            $("#cart_number").html(response);

            console.log('counted success')
        },
        error: function (error) {
            alert("error  cart : " + error.statusText);
        }
    });
}

