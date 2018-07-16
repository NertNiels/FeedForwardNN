

$(function () {

    var hub = $.connection.dataHub;

    hub.client.successMessage = function (message) {
        console.log(message);
    };

    hub.client.errorMessage = function (message) {
        console.error(message);
    };

    $.connection.hub.start().done(function () {
        hub.server.createNeuralNetwork("boi");
    });

});