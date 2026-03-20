export function debug(scope, message) {
    console.log(scope + ": " + message);
}

export function validateImg(elementId) {
    const input = elementId;
    const file = input.files[0];
    const parent = document.querySelector(".create-body-col-para");

    debug("parent", parent);

    removeError(parent); // NO DUPLI
    if (!file) {
        displayError(parent, "Please select an image");
        return;
    }

    if (!["image/png", "image/jpeg"].includes(file.type)) {
        displayError(parent, "Only PNG and JPEG is format supported");
        return;
    }

    if (file.size > 25 * 1024 * 1024) {
        displayError(parent, "Image too large!");
        return;
    }

    return file;
}

export async function uploadImg(img) {
    const formData = new FormData();
    formData.append("img", img);

    const req = await fetch("/Home/UploadImg", {
        method: "POST",
        body: formData
    });

    const res = await req.json();
    debug("Data img", res.url);

    return res.url;
}

export async function uploadMemories(imgUrl, to, from, message) {
    const req = await fetch("/Home/UploadMemories", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({
            From: from,
            To: to,
            Message: message,
            Img: imgUrl
        })
    });

    const data = await req.json();

    debug("Qr url", data.url);
    return data.url;
}

export function checkAllInputFields(form) {
    const inputFields = form.querySelectorAll("input, textarea");

    const checkAllInputFields = Array.from(inputFields).every(function (input) {
        return input.value.trim().length > 0;
    })

    return checkAllInputFields;
}

export function validateFormInput(form) {
    const inputFields = form.querySelectorAll("input, textarea");

    inputFields.forEach(i => {
        const parent = i.closest(".form-input-con");

        if (!i.value.trim()) {
            displayInputError(parent);
        } else {
            removeInputError(parent);
        }
    });
}

export function removeInputError(parentId) {
    parentId.querySelectorAll(".error").forEach(e => e.remove());
}

export function displayInputError(parentId) {
    removeInputError(parentId); // para di mag duplicate
    parentId.appendChild(errorText("Please input something :("));
}

export function displayError(parentId, message) {
    parentId.appendChild(errorText(message));
}

export function removeError(parentId) {
    parentId.querySelectorAll(".error").forEach(e => e.remove());
}

export function errorText(message) {
    const p = document.createElement("p");
    p.textContent = message;
    p.classList.add("error");
    return p;
}

export function displayPopUp(popUpId) {
    const modal = document.querySelector(".modal"); // Possible may cause an error
    const popUp = document.getElementById(popUpId);

    modal.classList.add("show");
    popUp.classList.add("show");
}

export function closePopUp() {
    const modal = document.querySelector(".modal"); // Possible may cause an error
    modal.querySelectorAll(".show").forEach(p => p.classList.remove("show"));
    modal.classList.remove("show");
}

export async function sendEmailWithAttach(to, from, canvasId) {
    const canvas = document.getElementById(canvasId);

    const base64 = canvas.toDataURL("image/png");

    debug("Base 64", base64);

    await fetch("/Home/SendEmailWithAttach", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({
            To: to,
            From: from,
            Img: base64
        })
    });
}