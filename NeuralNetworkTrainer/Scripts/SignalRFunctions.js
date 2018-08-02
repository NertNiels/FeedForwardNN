var hub;
var initialized = false;

$(function () {

    hub = $.connection.dataHub;

    hub.client.terminalMessage = function (message) {
        message = message.replace(/\r?\n/g, '<br />').replace("(ex)", "<span style='color:red'>").replace("(/ex)", "</span>").replace("\t", "&#9;");
        
        var term = document.getElementById("terminal");
        if (term == null) return;

        term.innerHTML += message;
    };

    hub.client.terminalError = function (message) {
        message = message.replace(/\r?\n/g, '<br />');
        var term = document.getElementById("terminal");
        if (term = null) return;

        term.innerHTML += "<span style='color:red'>" + message + "</span>";
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

    hub.client.giveNewTrainingLoss = function (newTrainLoss, lr) {
        if (newTrainLoss == null) {
            return;
        }
        
        var length = lossChart.data.datasets[0].data.length;
        var i;
        for (i = 0; i < newTrainLoss.length; i++) {
            var y = newTrainLoss[i];
            if (isNaN(y)) y = -1;
            if (isFinite(y)) {
                var data = {
                    x: length + i,
                    y: y
                }
                lossChart.data.datasets[0].data.push(data);
                
            }
            var data = {
                x: lossChart.data.datasets[2].data.length,
                y: lr[i]
            }
            lossChart.data.datasets[2].data.push(data);
        }
        
    }

    hub.client.giveNewValidationLoss = function (newValidLoss) {
        if (newValidLoss == null) {
            return;
        }

        var length = lossChart.data.datasets[1].data.length;
        var i;
        for (i = 0; i < newValidLoss.length; i++) {
            var y = newValidLoss[i];
            if (isNaN(y)) y = -1;
            if (isFinite(y)) {
                var data = {
                    x: length + i,
                    y: y
                }
                lossChart.data.datasets[1].data.push(data);
            }
        }
    }
    

    $.connection.hub.start().done(function () {
        hub.server.getNetworkInformation();
        initialized = true;
    });

});

function sendTerminalCommand(command) {
    if (initialized) hub.server.terminalCommand(command);
}

function askLossData() {
    if (initialized) {
        hub.server.getNewValidationLoss(lossChart.data.datasets[1].data.length);
        hub.server.getNewTrainingLoss(lossChart.data.datasets[0].data.length);

        lossChart.update();
        setTimeout(askLossData, 2000);
    }
    else setTimeout(askLossData, 2000);
}

function getTerminalLog() {
    if (initialized) {
        hub.server.getTerminalLog();
    } else setTimeout(getTerminalLog, 100);
}