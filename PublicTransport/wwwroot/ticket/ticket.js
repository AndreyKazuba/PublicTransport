const transportTypesNamesEnum = { 1: 'Автобус', 2: 'Троллейбус', 3: 'Трамвай' };
const transportTypesImgEnum = { 1: '../img/bus.svg', 2: '../img/trolleybus.svg', 3: '../img/tram.svg' };

document.addEventListener('DOMContentLoaded', async () => {
    const ticketId = localStorage.getItem('ticketId');
    if (!ticketId)
        return;

    const response = await post(`ticket`, ticketId);

    if (response.status == 200) {
        const ticket = await response.json();
        document.getElementById('type').innerText = transportTypesNamesEnum[ticket.transportType];
        document.getElementById('img').src = transportTypesImgEnum[ticket.transportType];
        document.getElementById('number').innerText = ticket.transportNumber;
    }
});

const useButton = document.getElementById('use-button');
useButton.onclick = async () => {
    const ticketId = localStorage.getItem('ticketId');
    if (!ticketId)
        return;

    const response = await post('ticket/use', ticketId);

    if (response.status == 200) {
        goToIndex();
    }
};