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

function dragElement(elem) {
    let pos1 = 0, pos2 = 0, pos3 = 0, pos4 = 0;
    elem.addEventListener("mousedown", dragMouseDown);

    function dragMouseDown(e) {
        let btnIDs = ["modal__block_btns_confirm", "modal__block_btns_cancel", "modal__block_close"];
        if (!btnIDs.includes(e.target.id)) {
            e.preventDefault();
            pos3 = e.clientX;
            pos4 = e.clientY;
            document.addEventListener("mousemove", elementDrag);
            document.addEventListener("mouseup", closeDragElement);
        }
    }

    function elementDrag(e) {
        pos1 = pos3 - e.clientX;
        pos2 = pos4 - e.clientY;
        pos3 = e.clientX;
        pos4 = e.clientY;
        if ((elem.offsetLeft - pos1 >= 0) && (elem.offsetWidth + elem.offsetLeft - pos1 <= elem.parentElement.offsetWidth)) {
            elem.style.left = (elem.offsetLeft - pos1) + "px";
        }
        if ((elem.offsetTop - pos2 >= 0) && (elem.offsetHeight + elem.offsetTop - pos2 <= elem.parentElement.offsetHeight)) {
            elem.style.top = (elem.offsetTop - pos2) + "px";
        }
    }

    function closeDragElement() {
        document.removeEventListener("mouseup", closeDragElement);
        document.removeEventListener("mousemove", elementDrag);
    }
}