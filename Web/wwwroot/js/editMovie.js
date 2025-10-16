const confirmBtn = document.querySelector("#confirmBtn");
const nameInput = document.querySelector("#Title");
const genreIdInput = document.querySelector("#GenreId");
const ratingInput = document.querySelector("#Rating");
const releaseYearInput = document.querySelector("#ReleaseYear");
const commentInput = document.querySelector("#Comment");

function inputHandler() {
    confirmBtn.disabled = nameInput.value.length < 1;
}

nameInput.addEventListener("input", inputHandler);
genreIdInput.addEventListener("input", inputHandler);
ratingInput.addEventListener("input", inputHandler);
releaseYearInput.addEventListener("input", inputHandler);
commentInput.addEventListener("input", inputHandler);