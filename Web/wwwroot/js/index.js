// Add file to database button and its modal window
document.getElementById("fetchMoviesBtn").addEventListener("click", () => 
{
    fetch('/Movie/AddFileToDatabase')
        .then(response => response.status)
        .then(data => {
            const modal = document.createElement("div");
            const modalBlock = document.createElement("div");
            const modalCloseBtn = document.createElement("button");
            const modalStatusMsg = document.createElement("p");
            const modalConfirmBtn = document.createElement("button");
            const modalBtnsContainer = document.createElement("div");
            const modalCancelBtn = document.createElement("button");

            modal.id = "modal";
            modalBlock.id = "modal__block";
            modalCloseBtn.id = "modal__block_close";
            modalStatusMsg.id = "modal__block_description";
            modalBtnsContainer.id = "modal__block_btns";
            modalConfirmBtn.id = "modal__block_btns_confirm";
            modalCancelBtn.id = "modal__block_btns_cancel";

            modalCloseBtn.textContent = "✖";
            modalConfirmBtn.textContent = "Ok";
            modalCancelBtn.textContent = "Cancel";
            switch (data) {
                case 200:
                    modalStatusMsg.textContent = "Movies were added successfully";
                    break;
                case 400:
                    modalStatusMsg.textContent = "An error occurred: file with acceptable extension (.md or .docx) does not exist or your profile already contains all movies listed in file";
                    break;
                default:
                    modalStatusMsg.textContent = "Unknown error, movies are not added";
            }

            modalBtnsContainer.append(modalConfirmBtn, modalCancelBtn);
            modalBlock.append(modalCloseBtn, modalStatusMsg);
            if (data == 200) {
                const modalReloadMsg = document.createElement("p");
                modalReloadMsg.id = "modal_reload";
                modalReloadMsg.textContent = "Reload this page to view new movies?";
                modalBlock.append(modalReloadMsg);
            }
            modalBlock.append(modalBtnsContainer);
            modal.append(modalBlock);
            const main = document.getElementById("main");
            main.append(modal);

            modalCloseBtn.addEventListener("click", removeModal);
            if (data == 200) {
                modalConfirmBtn.addEventListener("click", () => {
                    location.reload();
                });
            }
            else {
                modalConfirmBtn.addEventListener("click", removeModal);
            }
            modalCancelBtn.addEventListener("click", removeModal)

            function removeModal() {
                modal.remove();
            }

            // Modal movement functionality
            dragElement(modalBlock)

            function dragElement(mblock) {
                let blockPosX = 0, blockPosY = 0, mousePosX = 0, mousePosY = 0;
                mblock.addEventListener("mousedown", dragMouseDown);

                function dragMouseDown(e) {
                    let btnIDs = ["modal__block_btns_confirm", "modal__block_btns_cancel", "modal__block_close"];
                    if (!btnIDs.includes(e.target.id)) {
                        e.preventDefault();
                        mousePosX = e.clientX;
                        mousePosY = e.clientY;
                        mblock.addEventListener("mousemove", elementDrag);
                        mblock.addEventListener("mouseup", closeDragElement);
                    }
                }

                function elementDrag(e) {
                    blockPosX = mousePosX - e.clientX;
                    blockPosY = mousePosY - e.clientY;
                    mousePosX = e.clientX;
                    mousePosY = e.clientY;
                    mblock.style.left = (mblock.offsetLeft - blockPosX) + "px";
                    mblock.style.top = (mblock.offsetTop - blockPosY) + "px";
                }

                function closeDragElement() {
                    mblock.removeEventListener("mouseup", closeDragElement);
                    mblock.removeEventListener("mousemove", elementDrag);
                }
            }
        })
        .catch(error => console.error('Error in movie fetch: ', error));
});

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

        const linksBtn = document.createElement("div");
        linksBtn.textContent = "●●●";
        linksBtn.id = "body__row_movie_btns_menu";
        linksWrapper.append(linksBtn);
        movieRows[i].append(linksWrapper);
        linksWrapper.addEventListener("click", addButtonsBlock);

        function addButtonsBlock(e) {
            const buttonsContainer = document.getElementById("body__row_movie_btns_container");
            if (!buttonsContainer) {
                const buttonsContainer = document.createElement("div");
                buttonsContainer.id = "body__row_movie_btns_container";

                const editBtn = document.createElement("a");
                const deleteBtn = document.createElement("a");

                editBtn.textContent = "Edit";
                deleteBtn.textContent = "Delete";
                editBtn.id = "body__row_movie_btns_container_edit";
                deleteBtn.id = "body__row_movie_btns_container_delete";

                let rowMovieId = movieRows[i].getAttribute("data-id");
                editBtn.href = `/Movie/EditMovie/${rowMovieId}`;
                deleteBtn.href = `/Movie/DeleteMovie/${rowMovieId}`;

                buttonsContainer.append(editBtn, deleteBtn);
                linksWrapper.append(buttonsContainer);

                document.addEventListener("click", (e) => {
                    if (e.target != linksBtn) {
                        linksWrapper.remove();
                    }
                });
            }
        }

    });

    movieRows[i].addEventListener("mouseleave", (e) => {
        const buttonsContainer = document.getElementById("body__row_movie_btns_container");
        if (!buttonsContainer) {
            const wrapper = document.getElementById("body__row_movie_btns");
            wrapper.remove();
        }
    });
}