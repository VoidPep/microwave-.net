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
    console.log(state.totalTime)
    $("#time").text(state.totalTime != null ? `${state.totalTime}s` : "--");
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
        await loadPresets();
    } catch (err) {
        setTimeout(startConnection, 3000);
    }
}
