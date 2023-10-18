const transportTypes = document.getElementsByClassName('transport-type');

const transportTypesEnum = {
    bus: 1,
    trolleybus: 2,
    tram: 3
}

let currentTransportId = null;

for (const transportType of transportTypes) {
    transportType.addEventListener('click', () => {
        document.getElementById('tip').style.display = 'none';
        document.getElementById('buy-form').style.display = 'none';
        currentTransportId = null;
        for (const type of transportTypes) {
            type.classList.remove('active');
        }
        transportType.classList.add('active');
        const type = transportType.getAttribute('data-type');
        
        const transportNumbersElement = document.getElementById('transportNumbers');
        transportNumbersElement.style.display = 'grid';
        transportNumbersElement.innerHTML = '';

        const transportNumbers = transports
            .filter(transport => transport.transportType == type)
            .map(transport => createTransportNumber(transport.number, transport.id));

        for (const currentTransportNumber of transportNumbers) {
            currentTransportNumber.onclick = e => {
                for (const transportNumber of transportNumbers) {
                    transportNumber.classList.remove('active');
                }
                currentTransportNumber.classList.add('active');
                const buyForm = document.getElementById('buy-form');
                buyForm.style.display = 'flex';
                currentTransportId = currentTransportNumber.getAttribute('data-transport-id');
            };
            transportNumbersElement.appendChild(currentTransportNumber);
        }
    });
}

const createTransportNumber = (number, id) => {
    const div = document.createElement('div');
    div.classList.add('transport-number');
    div.setAttribute('data-transport-id', id)
    div.innerText = number;
    return div;
};

let transports = null;

document.addEventListener('DOMContentLoaded', async () => {
    const response = await get('transports');

    if (response.status == 200) {
        transports = await response.json();
    }
});

const buyButton = document.getElementById('buyButton');

buyButton.onclick = async () => {
    if (currentTransportId) {
        const response = await post('buyTicket', currentTransportId);

        if (response.status == 200) {
            goToIndex();
        }

        if (response.status == 402) {
            document.getElementById('error').style.display = 'block';
        }
    }
};