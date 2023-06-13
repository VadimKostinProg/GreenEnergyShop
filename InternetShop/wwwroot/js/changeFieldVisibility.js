const checkbox = document.getElementById('isDiscountActiveCheckBox');
const input = document.getElementById('discountPriceInput');

if (checkbox.checked) {
    input.removeAttribute('disabled');
} else {
    input.setAttribute('disabled', true);
    input.value = '';
}

checkbox.addEventListener('change', function () {
    if (this.checked) {
        input.removeAttribute('disabled');
    } else {
        input.setAttribute('disabled', true);
        input.value = '';
    }
});