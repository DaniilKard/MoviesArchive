const modal = document.getElementById("modal");
const modalConfirmBtn = document.getElementById("modal__block_btns_confirm");
const modalCloseBtn = document.getElementById("modal__block_close");

modalConfirmBtn.addEventListener("click", removeModalWindow);
modalCloseBtn.addEventListener("click", removeModalWindow);

function removeModalWindow() {
    modal.remove();
}

// Modal movement functionality
const modalBlock = document.getElementById("modal__block");
let blockPosX = 0, blockPosY = 0, mousePosX = 0, mousePosY = 0;
modalBlock.addEventListener("mousedown", dragOnMouseDown);

function dragOnMouseDown(e) {
    let btnIDs = ["modal__block_btns_confirm", "modal__block_close"];
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