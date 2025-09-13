async function loadData() {
    try {
        const response = await fetch("https://localhost:7006/camera");
        const data = await response.json();

        data.forEach(item => {
            let targetTable;

            if (item.number % 15 === 0) {
                targetTable = document.getElementById("column15");
            } else if (item.number % 3 === 0) {
                targetTable = document.getElementById("column3");
            } else if (item.number % 5 === 0) {
                targetTable = document.getElementById("column5");
            } else {
                targetTable = document.getElementById("columnOther");
            }

            const tbody = targetTable.querySelector(`tbody`);
            const row = tbody.insertRow();

            row.insertCell().textContent = item.number;
            row.insertCell().textContent = item.name;
            row.insertCell().textContent = item.latitude;
            row.insertCell().textContent = item.longitude;
        });

        renderMap(data)

    } catch (err) {
        console.error("Error fetching data:", err);
    }
}

function renderMap(data) {
    const map = L.map("map").setView([52.0914, 5.1115], 14);

    L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
        maxZoom: 19,
        attribution: '&copy; <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a>'
    }).addTo(map);

    data.forEach(cam => {
        L.marker([cam.latitude, cam.longitude])
            .addTo(map)
            .bindPopup(`<b>${cam.name}</b><br>Number: ${cam.number}`)
            .openPopup();
    });
}

// Run it
loadData();