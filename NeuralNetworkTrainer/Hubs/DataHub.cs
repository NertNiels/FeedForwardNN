using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;


namespace NeuralNetworkTrainer.Hubs
{
    public class DataHub : Hub
    {

        public void Send(String name, String message)
        {
            // Call the broadcastMessage method to update clients.
            Clients.All.broadcastMessage(name, message);

        }

        public void CreateNeuralNetwork(String data)
        {
            if (String.IsNullOrEmpty(data) || String.IsNullOrWhiteSpace(data))
            {
                Clients.Caller.errorMessage("The data sent were Empty or Null.");
                return;
            }
            Clients.Caller.errorMessage("The data sent were Empty or Null.");

            Clients.Caller.successMessage("The Neural Network was successfully made!");

        }

        public void TerminalCommand(String command)
        {
            Clients.Caller.errorTerminalMessage(command + "YA TEST");
            Clients.Caller.successTerminalMessage(command + "YA TEST");
        }
    }
}