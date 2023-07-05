let categorySelect = document.getElementById('category-select');
let characteristicsList = document.getElementById('characteristics-list');

if (document.title == "Створення продукту") {
    categorySelect.selectedIndex = -1;
}

categorySelect.addEventListener('change', getCharacteristics);

function getCharacteristics() {
    let selectedId = categorySelect.value;
    let url = `/Admin/Categories/Characteristics?categoryId=${selectedId}`;
    var response = fetch(url, { method: "GET" })
        .then(response => response.json())
        .then(data => showCharacteristics(data))
        .catch(error => console.log(error));
}

function showCharacteristics(characteristics) {
    removeChilds(characteristicsList);
    for (let i = 0; i < characteristics.length; i++) {
        let newElement = document.createElement('div');
        newElement.className = 'row mb-3';
        newElement.innerHTML = '<div class="col-4">' +
            `<label asp-for="Characteristics" class="p-3 lead">${characteristics[i]}</label>` +
            '</div>' +
            '<div class="col-8">' +
            `<input type="text" name="Characteristics[${i}]" class="form-control" />` +
            '</div>';

        characteristicsList.appendChild(newElement);
    }
}

function removeChilds(element) {
    while (element.firstChild) {
        element.removeChild(element.firstChild);
    }
}