let orderButton = document.getElementById('addToCart-button');
let productIdInput = document.getElementById('productId-input');
let productNameInput = document.getElementById('productName-input');
let productImageInput = document.getElementById('productImage-input');
let productPriceInput = document.getElementById('productPrice-input');
let quantityInput = document.getElementById('quantity-input');

orderButton.addEventListener('click', function () {
    let id = productIdInput.value;
    let name = productNameInput.value;
    let image = productImageInput.value;
    let price = parseFloat(productPriceInput.value);
    let quantity = 1;

    if (quantityInput.value) {
        quantity = parseInt(quantityInput.value);
    }

    const savedOrder = localStorage.getItem('shopping-cart');

    let order = {};

    if (savedOrder) {
        order = JSON.parse(savedOrder);

        if (order.items.some(item => item.productId === id)) {
            order.items.filter(item => item.productId === id).map(item => {
                item.price = price;
                item.quantity = quantity;
            });
        }
        else {
            order.items.push({ productId: id, productName: name, imageUrl: image, productPrice: price, quantity: quantity });
        }
    }
    else {
        order = {
            items: [
                { productId: id, productName: name, imageUrl: image, productPrice: price, quantity: quantity }
            ]
        };
    }
    
    localStorage.setItem('shopping-cart', JSON.stringify(order));
    window.location.assign('/Customer/Home/ShoppingCart');
});

function quantityChanged() {
    let quantity = parseInt(quantityInput.value);
    if (quantity == null) {
        return;
    }
    if (quantity < 1) {
        quantityInput.value = 1;
    }
}