// wwwroot/js/scripts.js
document.addEventListener('DOMContentLoaded', function() {
    const form = document.querySelector('form');
    const amountInput = document.getElementById('number');
    
    form.addEventListener('submit', function(e) {
        if(!amountInput.checkValidity()) {
            e.preventDefault();
            amountInput.classList.add('invalid-input');
            showErrorToast('Please enter a valid amount');
        }
    });

    function showErrorToast(message) {
        const toast = document.createElement('div');
        toast.className = 'toast-error';
        toast.textContent = message;
        document.body.appendChild(toast);
        
        setTimeout(() => {
            toast.remove();
        }, 3000);
    }
});