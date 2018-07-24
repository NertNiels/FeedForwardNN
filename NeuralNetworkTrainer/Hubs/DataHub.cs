using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Microsoft.AspNet.SignalR;

using NeuralNetwork.Core;
using NeuralNetwork.Layers;


namespace NeuralNetworkTrainer.Hubs
{
    public class DataHub : Hub
    {
        
        public void GetNetworkInformation()
        {
            Model network = NeuralNetworkHandler.network;
            if(network == null)
            {
                Clients.Caller.setNetworkInformation("No Network Training", "No Network Training", "No Network Training", false);
                return;
            }

            Clients.Caller.setNetworkInformation(network.Name, network.Type, "No Discription Available", true);
        }

        public void TerminalCommand(String command)
        {
            try
            {
                command = command.ToLower();
                if (String.IsNullOrEmpty(command) || String.IsNullOrWhiteSpace(command))
                {
                    Clients.Caller.terminalError("That's not a valid command... Type \'help\' if you don\'t know what command to use.");
                    return;
                }
                String[] commands = command.Split(' ');
                String head = commands[0];



                if (head == "neuralnetwork")
                {
                    Model network = NeuralNetworkHandler.network;
                    if (commands.Length == 1)
                    {
                        if (network == null) Clients.Caller.terminalMessage("There is no Network currently loaded.");
                        else
                        {
                            String message =
                                "Current loaded network:\n" +
                                "\tName:\t" + network.Name + "\n" +
                                "\tType:\t" + network.Type;
                            Clients.Caller.terminalMessage(message);
                        }
                    }
                    else if (commands[1] == "create")
                    {
                        if (network == null)
                        {
                            List<LayerBase> layers = new List<LayerBase>();

                            for (int i = 4; i < commands.Length; i += 2)
                            {
                                LayerBase l = null;
                                if (commands[i] == "input")
                                {
                                    l = new InputLayer();
                                }
                                else if (commands[i] == "lrelu")
                                {
                                    l = new LeakyReluLayer();
                                }
                                else
                                {
                                    Clients.Caller.terminalError(String.Format("Layer type {0} does not exist", commands[i]));
                                    return;
                                }
                                l.nodes = int.Parse(commands[i + 1]);
                                layers.Add(l);
                            }
                            NeuralNetworkHandler.CreateNetwork(layers.ToArray(), commands[2], commands[3]);
                            Clients.Caller.terminalMessage("Neural Network successfully created.");
                            return;
                        }
                    }
                }
                else Clients.Caller.terminalError("That's not a valid command... Type \'help\' if you don\'t know what command to use.");
            } catch (Exception e)
            {
                Clients.Caller.terminalError("Command resolved in an error.");
                Clients.Caller.terminalError(e.Message);
                Clients.Caller.terminalError(e.StackTrace);
            }
        }

    }
}