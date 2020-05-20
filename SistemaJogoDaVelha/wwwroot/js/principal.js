var connection = new signalR.HubConnectionBuilder().withUrl("/principalHub").build();

connection.start()
    .then(() => document.getElementById("entrarUsuario").disabled = false)
    .catch(err => console.error(err.toString()));

connection.on("UsuariosLogados", usuarios => {
    var lista = document.getElementById("listaUsuarios");
    lista.innerText = "";

    for (var usuario of usuarios) {
        var li = document.createElement("li");
        li.textContent = usuario;
        lista.appendChild(li);
    }
});

connection.on("SpamMessage", usuario => {
    alert(`${usuario} está enviando uma notificação!`);
});

function entrarUsuario() {
    var usuario = document.getElementById("userName").value;

    if (usuario.length > 0)
        connection.invoke("EntrarUsuario", usuario)
            .then(() => {
                document.getElementById("userName").disabled = true;
                document.getElementById("entrarUsuario").disabled = true;
            })
            .catch(err => console.error(err.toString()));
}

function spamAll() {
    connection.invoke("SpamAll")
        .catch(err => console.error(err.toString()));
}

function spamOthers() {
    connection.invoke("SpamOthers")
        .catch(err => console.error(err.toString()));
}

function entrarSala(id) {
    connection.invoke("EntrarSala", id)
        .catch(err => console.error(err.toString()));
}

function spamGroup(id) {
    connection.invoke("SpamGroup", id)
        .catch(err => console.error(err.toString()));
}