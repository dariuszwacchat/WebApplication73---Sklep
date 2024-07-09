
function DisabledButton(object, value) {
    if (value == true) {
        object.setAttribute('disabled', 'disabled')
    }
    else {
        object.removeAttribute('disabled')
    }
}




function initializeDropdownMenu() {
    var dropdowns = document.querySelectorAll('.dropdown');
    dropdowns.forEach(function (dropdown) {
        var dropbtn = dropdown.querySelector('.dropbtn');
        var content = dropdown.querySelector('.dropdown-content');

        dropbtn.addEventListener('click', function () {
            closeAllDropdowns();
            content.classList.toggle('show');
        });

        window.addEventListener('click', function (event) {
            if (!event.target.matches('.dropbtn')) {
                if (content.classList.contains('show')) {
                    content.classList.remove('show');
                }
            }
        });
    });
    function closeAllDropdowns() {
        dropdowns.forEach(function (otherDropdown) {
            var otherContent = otherDropdown.querySelector('.dropdown-content');
            if (otherContent.classList.contains('show')) {
                otherContent.classList.remove('show');
            }
        });
    }
}
document.addEventListener('DOMContentLoaded', function () {
    initializeDropdownMenu();
});










function showContainer(containerId) {
    // Ukryj wszystkie kontenery
    var containers = document.querySelectorAll('.container-payment-options');
    containers.forEach(function (container) {
        container.style.display = 'none';
    });

    // Pokaż wybrany kontener
    var selectedContainer = document.getElementById(containerId);
    selectedContainer.style.display = 'block';
}






/*
function toggleFormat(format) {
    document.execCommand(format, false, null);
}
function changeFontSize() {
    const size = document.querySelector('select').value;
    if (size) {
        document.execCommand('fontSize', false, size);
    }
}
function changeTextColor() {
    const color = document.querySelector('input[type="color"]').value;
    document.execCommand('foreColor', false, color);
}
function alignText(align) {
    document.execCommand('justify' + align.charAt(0).toUpperCase() + align.slice(1), false, null);
}
function toggleList(type) {
    document.execCommand('insert' + (type === 'ordered' ? 'Order' : 'Unordered') + 'List', false, null);
}
function insertImages(input) {
    const files = input.files;
    for (let i = 0; i < files.length; i++) {
        const reader = new FileReader();
        reader.onload = function (e) {
            const img = document.createElement('img');
            img.src = e.target.result;
            img.style.maxWidth = '100%';
            document.execCommand('insertHTML', false, img.outerHTML);
        };
        reader.readAsDataURL(files[i]);
    }
}
*/















