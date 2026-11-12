// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


document.querySelectorAll(".task-date").forEach(input => {
    input.addEventListener("click", function () {
        if (typeof this.showPicker === "function") {
            this.showPicker();
        }
    });
});
