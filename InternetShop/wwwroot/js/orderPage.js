let orderBox = document.getElementById('orderBox');
let content = document.getElementById('content');
let submitButton = document.getElementById('submit-button');

refreshCart();

const savedOrder = localStorage.getItem('shopping-cart');

if (savedOrder) {
    removeChilds(orderBox);

    order = JSON.parse(savedOrder);

    try {
        let totalPrice = 0;
        for (let i = 0; i < order.items.length; i++) {
            let productElement = document.createElement('div');
            productElement.className = 'row mb-3';
            productElement.innerHTML = '<hr />' +
                `<input type="hidden" name="ProductsId[${i}]" value="${order.items[i].productId}"></input>` +
                '<div class="d-none d-md-block col-2">' +
                `<img src="${order.items[i].imageUrl}" class="w-75"/>` +
                '</div>' +
                '<div class="col-7 col-md-5">' +
                `<h3 class="text-primary">${order.items[i].productName}</h3>` +
                '</div>' +
                '<div class="col-3">' +
                `<h2 class="text-primary">${order.items[i].productPrice}</h2>` +
                '</div>' +
                '<div class="col-2">' +
                `<input type="hidden" name="Quantities[${i}]" value="${order.items[i].quantity}"></input>` +
                `<h3 class="text-primary">${order.items[i].quantity}</input>` +
                '</div>';
            orderBox.appendChild(productElement);

            totalPrice += parseFloat(order.items[i].productPrice) * parseFloat(order.items[i].quantity);
        }

        let totalPriceElement = document.createElement('div');
        totalPriceElement.className = 'row mb-3';
        totalPriceElement.innerHTML = '<hr />' +
            '<div class="col-7">' +
            '<h3>Загальна сума:</h3>' +
            '</div>' +
            '<div class="col-3">' +
            `<h3 class="text-primary" id="totalPrice">${totalPrice}</h3>` +
            '</div>' +
            '<div class="col-2"></div>';

        orderBox.appendChild(totalPriceElement);
    }
    catch (ex) {
        console.log(ex);
    }
}
else {
    removeChilds(content);
    content.innerHTML = '<h3 class="text-center mt-3">Корзина порожня! Наповніть корзину і зможете оформити замовлення.</h3>';
}

submitButton.addEventListener('click', function () {
    localStorage.removeItem('shopping-cart');
});

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

function removeChilds(element) {
    while (element.firstChild) {
        element.removeChild(element.firstChild);
    }
}