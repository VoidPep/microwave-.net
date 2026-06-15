let allPresets = [];

async function loadPresets() {
    try {
        allPresets = await getRequest("presets");
        const select = $("#presetSelect");
        select.html('<option value="">Deixe vazio para utilizar o padrão</option>');
        console.table(allPresets)
        allPresets.forEach(preset => {
            const style = preset.isCustom ? 'style="font-style: italic;"' : '';
            select.append(`<option value="${preset.id}" data-name="${preset.nome}" ${style}>${preset.nome}</option>`);
        });
    } catch (err) {
        console.error("Erro ao carregar presets:", err);
    }
}

async function saveNewPreset() {
    const preset = {
        nome: $("#presetName").val(),
        potencia: parseInt($("#presetPotencia").val()),
        caractere: $("#presetCaractere").val(),
        tempo: parseInt($("#presetTempo").val()),
        instrucoes: $("#presetInstrucoes").val() || null
    };

    if (!preset.nome || !preset.potencia || !preset.caractere || !preset.tempo) {
        console.error("Por favor, preencha todos os campos obrigatórios.");
        return;
    }

    if (preset.caractere === ".") {
        console.error("O caractere padrão '.' não pode ser utilizado.");
        return;
    }

    if (preset.caractere.length !== 1) {
        console.error("O caractere deve ter exatamente 1 caracter.");
        return;
    }

    try {
        const response = await post("preset/create", preset);

        if (response.message.includes("sucesso")) {
            $("#newPresetModal").modal("hide");
            $("#newPresetForm")[0].reset();
            await loadPresets();
        }
    } catch (err) {
        console.error(err);
    }
}

async function deletePreset() {
    const presetId = $("#presetSelect").val();
    const selectedPreset = allPresets.find(p => p.id == presetId);

    if (!selectedPreset) {
        return;
    }

    if (!selectedPreset.isCustom) {
        return;
    }

    if (confirm(`Tem certeza que deseja remover o programa "${selectedPreset.nome}"?`)) {
        try {
            const response = await deleteRequest(`preset/delete-by-id?id=${presetId}`);
            if (response.message.includes("sucesso")) {
                await loadPresets();
                $("#presetSelect").val("");
                updateDeleteButtonState();
            }
        } catch (err) {
            console.error(err);
        }
    }
}

function updateDeleteButtonState() {
    const presetId = $("#presetSelect").val();
    const selectedPreset = allPresets.find(p => p.id == presetId);

    $("#btnDeletePreset").prop("disabled", selectedPreset && !selectedPreset.isCustom);
}

function onPresetChange() {
    const presetSelected = $("#presetSelect").val();
    console.log(presetSelected)
    $("#timerInput").prop("disabled", presetSelected && presetSelected !== "");
    $("#powerInput").prop("disabled", presetSelected && presetSelected !== "");
    updateDeleteButtonState();
}
