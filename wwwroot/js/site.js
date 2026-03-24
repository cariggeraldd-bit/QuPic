import * as utils from "./utils.js";

// GLOBAL VARIABLE
const modal = document.querySelector(".modal");
const closeIcon = document.querySelector(".close-icon");


// CLOSE ALL POP UPS
closeIcon.addEventListener("click", function () {
    utils.closePopUp();
});

modal.addEventListener("click", function (e) {
    // outside click to
    if (e.target === modal) {
        utils.closePopUp();
    }
});

const PageScripts = {
    home: function () {
        utils.debug("Page", "Home");

        // LOCAL VARIABLE
        const memoryForm = document.getElementById("memoryForm");
        const from = document.getElementById("from");
        const to = document.getElementById("to");
        const message = document.getElementById("message");
        const img = document.getElementById("img");
        const submitBtn = document.querySelector(".generate-qr");

        memoryForm.addEventListener("submit", async function (e) {
            e.preventDefault();

            submitBtn.disabled = true;
            submitBtn.textContent = "Processing...";

            try {
                const imgData = utils.validateImg(img); // CHECK IMG
                utils.validateFormInput(memoryForm); // CHECKS FORM

                const checkAllInputFields = utils.checkAllInputFields(memoryForm);

                if (checkAllInputFields) {
                    const imgUrl = await utils.uploadImg(imgData);

                    utils.debug("Image url", imgUrl);
                    
                    utils.debug("Data", from.value + " " + to.value + " " + message.value);
                    const qr = await utils.uploadMemories(imgUrl, to.value, from.value, message.value);

                    await QRCode.toCanvas(document.getElementById("qrCanvas"), qr);

                    await utils.sendEmailWithAttach("amandasanjuan25@gmail.com",  from.value, "qrCanvas"); // CHANGE email

                    utils.displayPopUp("memorySaved");
                    utils.displayQr("qr");
                }
            } catch (err) {
                utils.debug("Error", err);
            } finally {
                submitBtn.disabled = false;
                submitBtn.textContent = "Generate QR";
            }
        });
    }
}

document.addEventListener("DOMContentLoaded", function () {
    const page = document.body.dataset.page;

    if (PageScripts[page]) {
        PageScripts[page]();
    } 
});