// Movie "Edit" and "Delete" buttons
const movieRows = document.getElementsByClassName('body__row');
for (let i = 0; i < movieRows.length; i++) {
    movieRows[i].addEventListener("mouseenter", () => {
        const linksBtnCheck = document.getElementById("body__row_movie_btns_menu");
        if (linksBtnCheck) {
            return;
        }

        const linksWrapper = document.createElement("div");
        linksWrapper.id = "body__row_movie_btns";
        linksWrapper.className = "body__row_movie_btns";

        const linksBtn = document.createElement("div");
        linksBtn.textContent = "●●●";
        linksBtn.id = "body__row_movie_btns_menu";
        linksBtn.className = "body__row_movie_btns_menu";
        linksWrapper.append(linksBtn);
        movieRows[i].append(linksWrapper);
        linksWrapper.addEventListener("click", addButtonsBlock);

        function addButtonsBlock() {
            const buttonsContainer = document.getElementById("body__row_movie_btns_container");
            if (!buttonsContainer) {
                const buttonsContainer = document.createElement("div");
                buttonsContainer.id = "body__row_movie_btns_container";
                buttonsContainer.className = "body__row_movie_btns_container";

                const editBtn = document.createElement("a");
                const deleteBtn = document.createElement("a");

                editBtn.innerHTML = '<img src="img/editBtn.svg" /> Edit';
                deleteBtn.innerHTML = '<img src="img/deleteBtn.svg" /> Delete';
                editBtn.className = "body__row_movie_btns_container_edit";
                deleteBtn.className = "body__row_movie_btns_container_delete";

                let rowMovieId = movieRows[i].getAttribute("data-id");
                editBtn.href = `/Movie/EditMovie/${rowMovieId}`;
                deleteBtn.href = `/Movie/DeleteMovie/${rowMovieId}`;

                const linksWrapper = document.getElementById("body__row_movie_btns");
                buttonsContainer.append(editBtn, deleteBtn);
                linksWrapper.append(buttonsContainer);

                document.addEventListener("click", (e) => {
                    const linksBtn = document.getElementById("body__row_movie_btns_menu");
                    if (e.target != linksBtn) {
                        linksWrapper.remove();
                    }
                });
            }
        }
    });
    movieRows[i].addEventListener("mouseleave", removeBtns);
}

function removeBtns() {
    const buttonsContainer = document.getElementById("body__row_movie_btns_container");
    const wrapper = document.getElementById("body__row_movie_btns");
    if (!buttonsContainer && wrapper) {
        wrapper.remove();
    }
}