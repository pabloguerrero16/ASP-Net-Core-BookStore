document.addEventListener('DOMContentLoaded', () => {
    const decrementButton = document.getElementById('decrement-button');
    const incrementButton = document.getElementById('increment-button');
    const quantityDisplay = document.getElementById('quantity-display');
    const productQuantityInput = document.getElementById('product-quantity');
    const stock = parseInt(productQuantityInput.getAttribute('data-stock'));
    const quantityHiddenInput = document.getElementById('quantity');

    function updateQuantity(amount) {
        let currentQuantity = parseInt(productQuantityInput.value);
        let newQuantity = currentQuantity + amount;

        if (newQuantity < 1) {
            newQuantity = 1;
        } else if (newQuantity > stock) {
            newQuantity = stock;
        }

        productQuantityInput.value = newQuantity;
        quantityDisplay.textContent = newQuantity;
        quantityHiddenInput.value = newQuantity; // Actualiza el campo oculto
    }

    decrementButton.addEventListener('click', () => {
        updateQuantity(-1);
    });

    incrementButton.addEventListener('click', () => {
        updateQuantity(1);
    });

    // Actualiza el campo oculto antes de enviar el formulario
    const form = document.getElementById('addToCartForm');
    form.addEventListener('submit', () => {
        quantityHiddenInput.value = quantityDisplay.textContent;
    });
});