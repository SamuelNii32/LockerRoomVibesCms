// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.



// Global toast helper
function showToast(message) {
    const toastBody = document.getElementById('toastBody');
    const toastEl = document.getElementById('successToast');

    if (toastBody) toastBody.textContent = message;

    const toast = new bootstrap.Toast(toastEl);
    toast.show();
}



function cleanupModalBackdrop() {
    const backdrop = document.querySelector('.modal-backdrop');
    if (backdrop) backdrop.remove();
    document.body.classList.remove('modal-open');
    document.body.style.overflow = '';
}

document.addEventListener('DOMContentLoaded', function () {
    const stats = document.querySelectorAll('.fade-in');
    if (stats.length > 0) {
        const observerOptions = {
            threshold: 0.1,
            rootMargin: '0px 0px -50px 0px'
        };

        const observer = new IntersectionObserver(function (entries) {
            entries.forEach(entry => {
                if (entry.isIntersecting) {
                    entry.target.style.opacity = '1';
                    entry.target.style.transform = 'translateY(0)';
                }
            });
        }, observerOptions);

        stats.forEach(el => observer.observe(el));
    }

    const cards = document.querySelectorAll('.card');
    cards.forEach(card => {
        card.addEventListener('mouseenter', () => {
            card.style.transform = 'translateY(-5px)';
        });
        card.addEventListener('mouseleave', () => {
            card.style.transform = 'translateY(0)';
        });
    });

 


});
