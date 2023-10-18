const transportTypesEnum = {
    bus: 1,
    trolleybus: 2,
    tram: 3
};

const transportTypesNamesEnum = {1: 'Автобус', 2: 'Троллейбус', 3: 'Трамвай'};
const transportTypesImgEnum = {1: './img/bus.svg', 2: './img/trolleybus.svg', 3: './img/tram.svg'};

document.addEventListener('DOMContentLoaded', async () => {
    const response = await get('tickets');

    if (response.status == 200) {
        const main = document.getElementById('main');
        const tickets = await response.json();
        main.innerHTML = '';
        
        if (tickets.length) {
            document.getElementById('tip').style.display = 'none';
        }
        for (const ticket of tickets) {
            main.appendChild(createTicket(ticket.id, 
                transportTypesImgEnum[ticket.transportType], 
                transportTypesNamesEnum[ticket.transportType],
                ticket.transportNumber));
        }
    }
});

const createTicket = (id, src, typeName, num) => {
    const ticket = document.createElement('div');
    ticket.classList.add('ticket');
    ticket.setAttribute('data-ticket-id', id);

    const img = document.createElement('img');
    img.classList.add('ticket-img');
    img.src = src;

    const type = document.createElement('p');
    const typeLabel = document.createElement('span');
    typeLabel.innerText = 'Тип: ';
    const typeValue = document.createElement('b');
    typeValue.innerText = typeName;
    type.appendChild(typeLabel);
    type.appendChild(typeValue);

    const number = document.createElement('p');
    const numberLabel = document.createElement('span');
    numberLabel.innerText = 'Номер: ';
    const numberValue = document.createElement('b');
    numberValue.innerText = num;
    number.appendChild(numberLabel);
    number.appendChild(numberValue);

    const price = document.createElement('p');
    const priceLabel = document.createElement('span');
    priceLabel.innerText = 'Стоимость: ';
    const priceValue = document.createElement('b');
    priceValue.innerText = '2 BYN';
    price.appendChild(priceLabel);
    price.appendChild(priceValue);

    ticket.appendChild(img);
    ticket.appendChild(type);
    ticket.appendChild(number);
    ticket.appendChild(price);

    ticket.onclick = () => {
        goToTicket(id);
    };

    return ticket;
};