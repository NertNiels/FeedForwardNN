﻿var hub;

$(function () {

    hub = $.connection.dataHub;

    hub.client.terminalMessage = function (message) {
        var term = document.getElementById("terminal");

        var entry = document.createElement("li");
        entry.appendChild(document.createTextNode(message));

        term.appendChild(entry);
    };

    hub.client.terminalError = function (message) {
        var term = document.getElementById("terminal");

        var entry = document.createElement("li");
        entry.appendChild(document.createTextNode(message));
        entry.style.color = "red";

        term.appendChild(entry);
    };

    hub.client.setNetworkInformation = function (training, type, description, isTraining) {
        var nowTraining = document.getElementById("nowTraining");
        var Type = document.getElementById("typeNetwork");
        var networkDescription = document.getElementById("networkDescription");

        if (nowTraining == null) return;

        nowTraining.innerHTML = training;
        Type.innerHTML = type;
        networkDescription.innerHTML = description;

        if (isTraining) {
            nowTraining.style.color = "green";
            Type.style.color = "green";
            networkDescription.style.color = "green";
        } else {
            nowTraining.style.color = "red";
            Type.style.color = "red";
            networkDescription.style.color = "red";
        }
    }

    $.connection.hub.start().done(function () {
        hub.server.getNetworkInformation();
    });

});

function sendTerminalCommand(command) {
    hub.server.terminalCommand(command);
}