const API_URL = "http://localhost:5050";

function isAuthenticated() {
    const token = localStorage.getItem('token');

    if (token == "undefined") return false;

    return !!token;
}

function setToken(token) {
    localStorage.setItem('token', token);
}

function clearToken() {
    localStorage.setItem('token', undefined);
    updateAuthStatus()
}

function authHeaders() {
    var bearerToken = localStorage.getItem('token')

    if (bearerToken == "undefined") return { "Content-Type": "application/json" };

    return {
        "Authorization": `Bearer ${bearerToken}`,
        "Content-Type": "application/json"
    };
}

async function login(username, password) {
    const response = await fetch(`${API_URL}/api/auth/login`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ username, password })
    });

    const result = await response.json();

    if (!result.success) {
        throw new Error(result.error || "Falha na autenticação.");
    }

    

    setToken(JSON.parse(result.data).token);
    return JSON.parse(result.data).token;
}

async function post(endpoint, data = null) {
    const response = await fetch(`${API_URL}/api/microwave/${endpoint}`, {
        method: "POST",
        headers: authHeaders(),
        body: data ? JSON.stringify(data) : undefined
    });
    return response.json();
}

async function deleteRequest(endpoint) {
    const response = await fetch(`${API_URL}/api/microwave/${endpoint}`, {
        method: "DELETE",
        headers: authHeaders()
    });
    return response.json();
}

async function getRequest(endpoint) {
    const response = await fetch(`${API_URL}/api/microwave/${endpoint}`, {
        method: "GET",
        headers: authHeaders()
    });
    const result = await response.json();
    return JSON.parse(result.data) ?? result;
}
