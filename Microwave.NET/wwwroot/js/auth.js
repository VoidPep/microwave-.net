$(document).ready(() => {
    updateAuthStatus();

    $("#btnLogin").on("click", handleLogin);
    $("#btnLogout").on("click", clearToken);

    $("#authPassword").on("keypress", (e) => {
        if (e.key === "Enter") handleLogin();
    });
});

async function handleLogin() {
    const username = $("#authUsername").val().trim();
    const password = $("#authPassword").val();

    if (!username || !password) {
        setAuthStatus("error", "Preencha usuário e senha.");
        return;
    }

    $("#btnLogin").prop("disabled", true).text("Entrando...");

    try {
        await login(username, password);
        setAuthStatus("success", `Autenticado como ${username}`);
        setControlsEnabled(true);
        await loadPresets();
    } catch (err) {
        setAuthStatus("error", err.message || "Falha na autenticação.");
        setControlsEnabled(false);
    } finally {
        $("#btnLogin").prop("disabled", false).text("Entrar");
        $("#authPassword").val("");
    }
}

function updateAuthStatus() {
    if (isAuthenticated()) {
        setAuthStatus("success", "Autenticado");
        setControlsEnabled(true);
    } else {
        setAuthStatus("idle", "Não autenticado. Faça login para usar o microondas.");
        setControlsEnabled(false);
    }
}

function setAuthStatus(type, message) {
    const el = $("#authStatus");
    el.removeClass("text-success text-danger text-muted");

    const classMap = { success: "text-success", error: "text-danger", idle: "text-muted" };
    el.addClass(classMap[type] || "text-muted").text(message);
}

function setControlsEnabled(enabled) {
    ["#btnIniciar", "#btnPausar", "#btnNewPreset", "#btnDeletePreset",
        "#timerInput", "#powerInput", "#presetSelect"].forEach(sel => $(sel).prop("disabled", !enabled));
}
