async function iniciar() {
    const presetId = $("#presetSelect").val();
    const timer = $("#timerInput").val();
    const power = $("#powerInput").val();

    if (presetId) {
        const selectedPreset = allPresets.find(p => p.id == presetId);
        if (selectedPreset) {
            await post(`set-preset?nomePrograma=${encodeURIComponent(selectedPreset.nome)}`);
        }
    } else {
        if (timer) await post(`set-timer?timeInSeconds=${timer}`);
        if (power) await post(`set-power?powerLevel=${power}`);
    }

    await post("start");
}

async function pausarParar() {
    await post("cancel");
}
