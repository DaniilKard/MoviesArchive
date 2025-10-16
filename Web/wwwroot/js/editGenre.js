const editGenreBtn = document.querySelector("#confirmBtn");
const nameInput = document.querySelector("#Name");

function nameInputHandler() {
    editGenreBtn.disabled = nameInput.value.length < 3 || nameInput.value.length > 100;
}
nameInput.addEventListener("input", nameInputHandler);