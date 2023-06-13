let content = document.getElementById('content');
let orderButton = document.getElementById('order-button');

(async () => {
    await refreshCart();

    const savedOrder = localStorage.getItem('shopping-cart');

    if (savedOrder) {
        removeChilds(content);

        order = JSON.parse(savedOrder);

        let totalPrice = 0;

        for (let i = 0; i < order.items.length; i++) {
            let newElement = document.createElement('div');
            newElement.className = 'mb-3 d-flex flex-column flex-md-row';
            newElement.innerHTML = '<hr />' +
                '<div class="col-md-1">' +
                `<img src="${order.items[i].imageUrl}" class="w-75"/>` +
                '</div>' +
                '<div class="col-md-3">' +
                `<h3 class="text-primary">${order.items[i].productName}</h3>` +
                '</div>' +
                '<div class="col-md-5">' +
                `<h2 class="text-primary">${order.items[i].productPrice}</h2>` +
                '</div>' +
                '<div class="col-md-2 mb-3">' +
                `<input type="number" min="" id="quantityInput${i}" oninput="quantityChanged(${i})" class="form-control" value=${order.items[i].quantity}>` +
                '</div>' +
                '<div class="col-md-1">' +
                `<button type="button" onclick="deleteProduct(${i})" class="btn btn-danger">` +
                '<i class="bi bi-trash"></i>' +
                '</button>' +
                '</div>';

            totalPrice += parseFloat(order.items[i].productPrice) * parseFloat(order.items[i].quantity);

            content.appendChild(newElement);
        }

        let totalPriceElement = document.createElement('div');
        totalPriceElement.className = 'flex-md-row mb-3 d-sm-flex flex-sm-column';
        totalPriceElement.innerHTML = '<hr />' +
            '<div class="item-hl">' +
            '<h3>Загальна сума:</h3>' +
            '</div>' +
            '<div class="item-hl">' +
            `<h3 class="text-primary" id="totalPrice">${totalPrice}</h3>` +
            '</div>';

        content.appendChild(totalPriceElement);
    }
    else {
        content.innerHTML = '<h3 class="text-center mt-3">Корзина порожня!</h3>';
        orderButton.hidden = true;
    }
})();

function removeChilds(element) {
    while (element.firstChild) {
        element.removeChild(element.firstChild);
    }
}

function quantityChanged(number) {
    let i = parseInt(number);
    let quantityInput = document.getElementById(`quantityInput${i}`);
    let value = parseInt(quantityInput.value);

    if (value < 1) {
        quantityInput.value = 1;
        value = 1;
    }

    if (value) {
        order.items[i].quantity = value;
    } else {
        order.items[i].quantity = 1;
    }
    

    updateTotalPrice();

    localStorage.setItem('shopping-cart', JSON.stringify(order));
}

async function refreshCart() {
    const savedOrder = localStorage.getItem('shopping-cart');

    if (savedOrder) {
        order = JSON.parse(savedOrder);

        let toUpdate = true;

        let i = 0;
        while (i < order.items.length) {
            const currentPrice = await getCurrentPrice(order.items[i].productId);
            if (currentPrice) {
                order.items[i].productPrice = currentPrice;
                i++;
            } else {
                order.items = order.items.filter(item => item != order.items[i]);
            }

            if (order.items.length == 0) {
                toUpdate = false;
            }
        }

        if (toUpdate) {
            localStorage.setItem('shopping-cart', JSON.stringify(order));
        } else {
            localStorage.removeItem('shopping-cart');
        }
    }
}

async function getCurrentPrice(productId) {
    let url = `/Customer/Home/ProductInfo?productId=${productId}`;

    try {
        const response = await fetch(url, { method: "GET" });
        let data = await response.json();

        if (data.isDiscountActive) {
            return data.discountPrice;
        }
        return data.price;
    }
    catch (error) {
        console.log(error);
        return null;
    }
}

function deleteProduct(number) {
    let i = parseInt(number);

    order.items.splice(i, 1);

    localStorage.setItem('shopping-cart', JSON.stringify(order));

    if (order.items.length === 0) {
        localStorage.removeItem('shopping-cart');
    }

    location.reload();
}

function updateTotalPrice() {
    let totalPrice = 0;

    for (let i = 0; i < order.items.length; i++) {

        totalPrice += parseFloat(order.items[i].productPrice) * parseFloat(order.items[i].quantity);
    }

    let totalPriceH2 = document.getElementById('totalPrice');
    totalPriceH2.innerHTML = totalPrice;
}