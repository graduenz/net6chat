const messageLimit = 50;

const queryParams = new Proxy(new URLSearchParams(window.location.search), {
    get: (searchParams, prop) => searchParams.get(prop),
});

const room = queryParams.room;

async function app() {
    const connection = new signalR.HubConnectionBuilder().withUrl(`/chatHub?room=${room}`).build();

    const createMessageElement = function ({ user, message, time }) {
        const messagesElement = document.getElementById("messages");

        if (messagesElement.children.length >= messageLimit) {
            messagesElement.removeChild(messagesElement.getElementsByTagName('div')[0]);
        }

        var div = document.createElement("div");
        div.className = "message";
        messagesElement.appendChild(div);
        div.innerHTML = `<b>${user}:</b> ${message} <span class="time">${time}</span>`;
    };

    connection.on("FetchMessages", function (messages) {
        messages.forEach(createMessageElement);
    });

    connection.on("ReceiveMessage", function (user, message, time) {
        createMessageElement({ user, message, time });
    });

    const sendMessageHandler = function (event) {
        var input = document.getElementById("messageInput");
        var message = input.value;
        input.value = "";
        connection.invoke("SendMessage", message);
        event.preventDefault();
    };

    document.getElementById("sendButton").addEventListener("click", sendMessageHandler);
    document.getElementById("messageInput").addEventListener("keypress", function (event) {
        if (event.key == 'Enter') sendMessageHandler(event);
    });

    await connection.start();
}

app();
