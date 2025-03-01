window.sendEmail = function() {
    let params = {
        name: document.getElementById("name").value,
        email: document.getElementById("email").value,
        message: document.getElementById("message").value
    };

    emailjs.send('service_kxsv50d', 'template_fu0coi5', params)
        .then(() => {
            alert("Your message has been sent to us");
        })
        .catch(error => console.error("EmailJS Error: ", error));
};
