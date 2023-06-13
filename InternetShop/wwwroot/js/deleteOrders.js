const deleteForm = document.getElementById('deleteForm');
const startDateInput = document.getElementById('startDate');
const endDateInput = document.getElementById('endDate');

deleteForm.addEventListener('submit', function (event) {
    if (startDateInput.value > endDateInput.value) {
        event.preventDefault(); // Передбігаємо відправки форми, якщо валідація не пройшла
        alert('Початок інтервалу не може бути більшим за кінець інтервалу.');
    }
});