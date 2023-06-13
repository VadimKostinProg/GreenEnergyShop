let counter = 1;
let charactersticsListItem = document.getElementById("characteristics-list");

function OnAddClick() {
    let newElement = document.createElement('div');
    newElement.className = 'mb-3';
    newElement.innerHTML = `<input type=\"text\" name=\"CharacteristicsList[${counter}]\" class=\"form-control\" />` +
        "<span asp-validation-for=\"CharacteristicsList\" class=\"text-danger\"></span>"

    charactersticsListItem.appendChild(newElement);
    counter++;
}