const API_URL = "http://localhost:5050";

function post(endpoint, data = null) {
    if (data) {
        return $.ajax({
            url: `${API_URL}/api/microwave/${endpoint}`,
            type: "POST",
            data: JSON.stringify(data),
            contentType: "application/json"
        });
    }
    return $.post(`${API_URL}/api/microwave/${endpoint}`);
}

function deleteRequest(endpoint) {
    return $.ajax({
        url: `${API_URL}/api/microwave/${endpoint}`,
        type: "DELETE"
    });
}

function getRequest(endpoint) {
    return $.get(`${API_URL}/api/microwave/${endpoint}`);
}
