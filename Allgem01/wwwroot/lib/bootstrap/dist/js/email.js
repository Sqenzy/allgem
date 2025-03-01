document.addEventListener("DOMContentLoaded", function () {
    if (window.emailjs) {
        emailjs.init("z1A3H3jP6cEQpB65Z");
    } else {
        console.error("EmailJS is not loaded.");
    }
});

window.sendEmail = function () {
    let parms = {
        name: document.getElementById("name").value,
        email: document.getElementById("email").value,
        message: document.getElementById("message").value
    };

    emailjs.send('service_kxsv50d', 'template_bdgs15l', parms)
        .then(() => {
            alert("Your message has been sent to us");
        })
        .catch(error => console.error("EmailJS Error: ", error));
};
