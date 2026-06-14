const API_URL = "http://localhost:5050";
const HUB_URL = `${API_URL}/microwaveHub`;

const connection = new signalR.HubConnectionBuilder()
    .withUrl(HUB_URL, {
        transport: signalR.HttpTransportType.WebSockets |
            signalR.HttpTransportType.ServerSentEvents |
            signalR.HttpTransportType.LongPolling
    })
    .withAutomaticReconnect()
    .build();

connection.on("PropertyChanged", (state) => {
    $("#timeRemaining").text(state.remainingTime != null ? `${state.remainingTime}s` : "--");
    $("#power").text(state.powerLevel ?? "--");
    $("#progress").text(state.progress ?? "");
    $("#isRunning").text(state.isRunning
        ? (state.isPaused ? "Pausado" : "Aquecendo...")
        : "Parado");
});

async function startConnection() {
    try {
        await connection.start();
        console.log("SignalR conectado.");
    } catch (err) {
        setTimeout(startConnection, 3000);
    }
}

function post(endpoint) {
    return $.post(`${API_URL}/api/microwave/${endpoint}`);
}

async function iniciar() {
    const timer = $("#timerInput").val();
    const power = $("#powerInput").val();

    if (timer) await $.post(`${API_URL}/api/microwave/set-timer?timeInSeconds=${timer}`);
    if (power) await $.post(`${API_URL}/api/microwave/set-power?powerLevel=${power}`);

    await post("start");
}

async function pausarParar() {
    await post("cancel");
}

$(document).ready(() => {
    startConnection();

    $("#btnIniciar").on("click", iniciar);
    $("#btnPausar").on("click", pausarParar);
});
