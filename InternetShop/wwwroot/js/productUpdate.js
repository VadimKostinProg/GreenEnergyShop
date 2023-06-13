import { getCharacterstics } from './product.js';
import { showCharacteristics } from './product.js';
import { removeChilds } from './product.js';

let categorySelect = document.getElementById('category-select');
let characteristicsList = document.getElementById('characteristics-list');

categorySelect.addEventListener('change', getCharacteristics);

getCharacteristics();