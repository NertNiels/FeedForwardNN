var hub;

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

    hub.client.giveNewTrainingLoss = function (newLoss) {
        var length = lossChart.data.datasets[0].data.length;
        var i;
        for (i = 0; i < newLoss.length; i++) {
            var data = {
                x: length + i,
                y: newLoss[i]
            }
            lossChart.data.datasets[0].data.push(data);

        }
        lossChart.update();
        setTimeout(askLossData, 2000);
    }

    $.connection.hub.start().done(function () {
        hub.server.getNetworkInformation();
    });

});

function sendTerminalCommand(command) {
    if (hub != null) hub.server.terminalCommand(command);
}

function askLossData() {
    if (hub != null) hub.server.getNewTrainingLoss(lossChart.data.datasets[0].data.length);
    setTimeout(askLossData, 2000);
}