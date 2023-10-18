const balanceButton = document.getElementById('signInButton');

balanceButton.onclick = async () => {
    const passportId = document.getElementById('passportId').value;
    const password = document.getElementById('password').value;

    const response = await post('login', {
        passportId,
        password,
    });

    if (response.status == 200) {
        goToIndex();
    }
    else if (response.status == 401) {
        const error = document.getElementById('error');
        error.style.display = 'block';
    }
};