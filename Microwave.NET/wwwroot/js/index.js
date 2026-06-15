$(document).ready(() => {
    startConnection();

    $("#presetSelect").on("change", onPresetChange);
    $("#btnIniciar").on("click", iniciar);
    $("#btnPausar").on("click", pausarParar);
    $("#btnNewPreset").on("click", () => $("#newPresetModal").modal("show"));
    $("#btnSavePreset").on("click", saveNewPreset);
    $("#btnDeletePreset").on("click", deletePreset);
});
