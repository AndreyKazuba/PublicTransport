const goToIndex = () => {
    location.href = '/';
};

const goToLogin = () => {
    location.href = '/login/login.html';
};

const goToTicket = (ticketId) => {
    localStorage.setItem('ticketId', ticketId);
    location.href = './ticket/ticket.html';
};