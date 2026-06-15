const HUB_URL = `${API_URL}/microwaveHub`;

const connection = new signalR.HubConnectionBuilder()
    .withUrl(HUB_URL, {
        transport: signalR.HttpTransportType.WebSockets |
            signalR.HttpTransportType.ServerSentEvents |
            signalR.HttpTransportType.LongPolling
    })
    .withAutomaticReconnect()
    .build();

function formatTimeDisplay(seconds) {
    if (seconds == null) return "--";
    if (seconds < 60) {
        return `${seconds}s`;
    } else {
        const minutes = Math.floor(seconds / 60);

        // a linha abaixo foi feita com ajuda de IA
        const secs = seconds % 60;
        return `${minutes}:${String(secs).padStart(2, '0')}m`;
    }
}

connection.on("PropertyChanged", (state) => {
    console.log(state.totalTime)
    $("#time").text(formatTimeDisplay(state.totalTime));
    $("#timeRemaining").text(formatTimeDisplay(state.remainingTime));
    $("#power").text(state.powerLevel ?? "--");
    $("#progress").text(state.progress ?? "");
    $("#isRunning").text(state.isRunning
        ? (state.isPaused ? "Pausado" : "Aquecendo...")
        : "Parado");
});

async function startConnection() {
    try {
        await connection.start();
        await loadPresets();
    } catch (err) {
        setTimeout(startConnection, 3000);
    }
}
