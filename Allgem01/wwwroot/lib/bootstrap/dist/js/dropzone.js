function setupDropZone() {
    const dropZone = document.querySelector('.drop-zone');
    const fileInput = document.getElementById('fileInput');

    dropZone.addEventListener('click', () => fileInput.click());
}

function getFileName(element) {
    return element.files && element.files.length > 0 ? element.files[0].name : '';
}

function getDroppedFileName() {
    return ''; // Implement actual file handling logic as needed
}

function addClass(selector, className) {
    document.querySelector(selector).classList.add(className);
}

function removeClass(selector, className) {
    document.querySelector(selector).classList.remove(className);
}