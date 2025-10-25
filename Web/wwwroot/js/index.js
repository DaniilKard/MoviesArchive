// Add file to database button and its modal window
document.getElementById("fetchMoviesBtn").addEventListener("click", () => 
{
    fetch('/Movie/AddFileToDatabase')
        .then(response => response.status)
        .then(data => {
            createModal(data);
        })
        .catch(error => console.error('Error in movie fetch: ', error));
});

function createModal(data) {
    const modal = document.createElement("div");
    const modalBlock = document.createElement("div");
    const modalCloseBtn = document.createElement("button");
    const modalStatusMsg = document.createElement("p");
    const modalConfirmBtn = document.createElement("button");
    const modalCancelBtn = document.createElement("button");
    const modalBtnsContainer = document.createElement("div");

    modal.className = "modal";
    modalBlock.className = "modal__block";
    modalCloseBtn.id = "modal__block_close";
    modalCloseBtn.className = "modal__block_close";
    modalStatusMsg.className = "modal__block_description";
    modalBtnsContainer.className = "modal__block_btns";
    modalConfirmBtn.id = "modal__block_btns_confirm";
    modalConfirmBtn.classList.add("modal__block_btns_confirm", "button");

    modalCloseBtn.textContent = "✖";
    modalConfirmBtn.textContent = "Ok";
    switch (data) {
        case 200:
            modalStatusMsg.textContent = "Movies were added successfully";
            break;
        case 400:
            modalStatusMsg.textContent = "An error occurred: your profile already contains all movies listed in file or folder contains multiple .md or .docx files";
            break;
        case 404:
            modalStatusMsg.textContent = "An error occurred: folder does not exist or contains no files with appropriate extension (.md or .docx)"
        default:
            modalStatusMsg.textContent = "Unknown error, movies are not added";
    }

    modalBtnsContainer.append(modalConfirmBtn);
    modalBlock.append(modalCloseBtn, modalStatusMsg);
    if (data == 200) {
        const modalReloadMsg = document.createElement("p");
        modalReloadMsg.className = "modal_reload";
        modalReloadMsg.textContent = "Reload this page to view new movies?";
        modalBlock.append(modalReloadMsg);

        modalCancelBtn.id = "modal__block_btns_cancel";
        modalCancelBtn.classList.add("modal__block_btns_cancel", "button");
        modalCancelBtn.textContent = "Cancel";
        modalBtnsContainer.append(modalCancelBtn);
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
        modalCancelBtn.addEventListener("click", removeModal)
    }
    else {
        modalConfirmBtn.addEventListener("click", removeModal);
    }

    function removeModal() {
        modal.remove();
    }

    // Modal movement functionality
    let blockPosX = 0, blockPosY = 0, mousePosX = 0, mousePosY = 0;
    modalBlock.addEventListener("mousedown", dragOnMouseDown);

    function dragOnMouseDown(e) {
        let btnIDs = ["modal__block_btns_confirm", "modal__block_btns_cancel", "modal__block_close"];
        if (!btnIDs.includes(e.target.id)) {
            e.preventDefault();
            mousePosX = e.clientX;
            mousePosY = e.clientY;
            modal.addEventListener("mousemove", startDragElement);
            modal.addEventListener("mouseup", stopDragElement);
            modal.addEventListener("mouseleave", stopDragElement);
        }
    }

    function startDragElement(e) {
        blockPosX = mousePosX - e.clientX;
        blockPosY = mousePosY - e.clientY;
        mousePosX = e.clientX;
        mousePosY = e.clientY;
        modalBlock.style.left = (modalBlock.offsetLeft - blockPosX) + "px";
        modalBlock.style.top = (modalBlock.offsetTop - blockPosY) + "px";
    }

    function stopDragElement() {
        modal.removeEventListener("mouseup", stopDragElement);
        modal.removeEventListener("mousemove", startDragElement);
        modal.removeEventListener("mouseleave", stopDragElement);
    }
}

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

    movieRows[i].addEventListener("mouseleave", removeBtnsContainerOnLeave);
}

function removeBtnsContainerOnLeave() {
    const buttonsContainer = document.getElementById("body__row_movie_btns_container");
    const wrapper = document.getElementById("body__row_movie_btns");
    if (!buttonsContainer && wrapper) {
        wrapper.remove();
    }
}