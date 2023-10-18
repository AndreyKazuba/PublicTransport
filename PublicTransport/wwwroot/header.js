const logOutButton = document.getElementById('logOut');
logOutButton.onclick = async () => {
    const response = await post('logout');

    if (response.status == 200) {
        goToLogin();
    }
};

const sidebarLogOut = document.getElementById('sidebarLogOut');
sidebarLogOut.onclick = async () => {
    const response = await post('logout');

    if (response.status == 200) {
        goToLogin();
    }
};


document.addEventListener('DOMContentLoaded', async () => {
    const response = await get('currentUser');

    if (response.status == 200) {
        const user = await response.json();
        const balance = document.getElementById('balance-value');
        balance.innerText = user.balance;
        const sidebarBalance = document.getElementById('sidebar-balance-value');
        sidebarBalance.innerText = ` (${user.balance} BYN)`;
    }
});

const burgerButton = document.getElementById('burger-button');
burgerButton.onclick = () => {
    document.getElementById('sidebar').style.width = '220px';
};

const sidebarArrow = document.getElementById('sidebar-arrow');
sidebarArrow.onclick = () => {
    document.getElementById('sidebar').style.width = '0';
};