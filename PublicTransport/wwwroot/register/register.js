const signInButton = document.getElementById('register-button');

signInButton.onclick = async () => {
    const passportNumber = document.getElementById('passportNumber').value;
    const passportId = document.getElementById('passwortId').value;
    const firstName = document.getElementById('firstName').value;
    const lastName = document.getElementById('lastName').value;
    const password = document.getElementById('password').value;

    const response = await post('register', {
        passportNumber,
        passportId,
        firstName,
        lastName,
        password,
    });

    if (response.status == 200) {
        goToIndex();
    }
    if (response.status == 409) {
        const error = document.getElementById('userExist');
        document.getElementById('invalidData').style.display = 'none';
        error.style.display = 'block';
    }
    if (response.status == 400) {
        const error = document.getElementById('invalidData');
        document.getElementById('userExist').style.display = 'none';
        error.style.display = 'block';
    }
};