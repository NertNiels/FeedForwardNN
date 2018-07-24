

$(function () {

    var hub = $.connection.dataHub;

    hub.client.successTerminalMessage = function (message) {
        var term = document.getElementById(terminal);

        var entry = document.createElement("li");
        entry.appendChild(document.createTextNode(message));

        term.insertBefore(entry, term.childNodes[0]);
    };

    hub.client.errorTerminalMessage = function (message) {
        var term = document.getElementById(terminal);

        var entry = document.createElement("li");
        entry.appendChild(document.createTextNode(message));
        entry.style.color = "red";

        term.insertBefore(entry, term.childNodes[0]);
    };

    $.connection.hub.start().done(function () {

        sendTerminalCommand = function (command) {
            hub.server.terminalCommand(command);
        }

    });

});