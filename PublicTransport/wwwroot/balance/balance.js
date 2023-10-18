const balanceButton = document.getElementById('balanceButton');

balanceButton.onclick = async () => {
    const money = document.getElementById('money').value;

    const response = await post('increaseBalance', money);

    if (response.status == 200) {
        goToIndex();
    }
};