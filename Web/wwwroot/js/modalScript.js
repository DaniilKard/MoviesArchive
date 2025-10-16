const modal = document.getElementById("modal");
const modalConfirmBtn = document.getElementById("modal__block_btns_confirm");
const modalCloseBtn = document.getElementById("modal__block_close");
const modalCancelBtn = document.getElementById("modal__block_btns_cancel");

function removeModalWindow() {
    modal.remove();
}

modalConfirmBtn.addEventListener("click", removeModalWindow);
modalCloseBtn.addEventListener("click", removeModalWindow);
modalCancelBtn.addEventListener("click", removeModalWindow);

// Modal movement functionality
const modalBlock = document.getElementById("modal__block");
dragElement(modalBlock);

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