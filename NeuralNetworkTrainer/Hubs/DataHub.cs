using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;

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
            Clients.All.terminalMessage(command);
            try
            {
                command = command.ToLower();
                if (String.IsNullOrEmpty(command) || String.IsNullOrWhiteSpace(command))
                {
                    Clients.All.terminalError("That's not a valid command... Type \'help\' if you don\'t know what command to use.");
                    return;
                }
                String[] commands = command.Split(' ');
                String head = commands[0];



                if (head == "neuralnetwork")
                {
                    Model network = NeuralNetworkHandler.network;
                    if (commands.Length == 1)
                    {
                        if (network == null) Clients.All.terminalMessage("There is no Network currently loaded.");
                        else
                        {
                            String message =
                                "Current loaded network:\n" +
                                "\tName:\t" + network.Name + "\n" +
                                "\tType:\t" + network.Type;
                            Clients.All.terminalMessage(message);
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
                                    Clients.All.terminalError(String.Format("Layer type {0} does not exist.", commands[i]));
                                    return;
                                }
                                l.nodes = int.Parse(commands[i + 1]);
                                layers.Add(l);
                            }
                            NeuralNetworkHandler.CreateNetwork(layers.ToArray(), commands[2], commands[3]);
                            Clients.All.terminalMessage("Neural Network successfully created.");
                            return;
                        }
                    } else if(commands[1] == "load")
                    {
                        Clients.All.terminalMessage(NeuralNetworkHandler.LoadNetwork(commands[2]));
                    } else if(commands[1] == "save")
                    {
                        if(commands.Length == 2) Clients.All.terminalMessage(NeuralNetworkHandler.SaveNetwork());
                        else Clients.All.terminalMessage(NeuralNetworkHandler.SaveNetwork(commands[2]));
                    } else if(commands[1] == "randomize")
                    {
                        Clients.All.terminalMessage(NeuralNetworkHandler.RandomizeNetwork());
                    }
                } else if (head == "test")
                {

                    NeuralNetworkHandler.keeper = new DataKeeper();
                    Data[] data = new Data[2];

                    data[0] = new Data();
                    data[0].Inputs = new Matrix(5, 1)
                    {
                        data = new float[5, 1]
                        {
                            { 5 }, { 3 }, { 5 }, { 2 }, { 1 }
                        }
                    };
                    data[0].Targets = new Matrix(5, 1)
                    {
                        data = new float[5, 1]
                                            {
                            { 5 }, { 3 }, { 5 }, { 2 }, { 1 }
                                            }
                    };

                    data[1] = new Data();
                    data[1].Inputs = new Matrix(5, 1)
                    {
                        data = new float[5, 1]
                        {
                            { 5 }, { 3 }, { 5 }, { 2 }, { 1 }
                        }
                    };
                    data[1].Targets = new Matrix(5, 1)
                    {
                        data = new float[5, 1]
                                            {
                            { 5 }, { 3 }, { 5 }, { 2 }, { 1 }
                                            }
                    };
                    NeuralNetworkHandler.keeper.DataSet = data;
                    NeuralNetworkHandler.keeper.Name = "datasettest";

                } else if(head == "dataset")
                {
                    if(commands.Length == 1)
                    {
                        if (NeuralNetworkHandler.keeper == null) Clients.All.terminalMessage("There is no dataset loaded yet.");
                        else Clients.All.terminalMessage(String.Format("The dataset with the name {0} is currently loaded.", NeuralNetworkHandler.keeper.Name));
                    } else if(commands[1] == "load")
                    {
                        Clients.All.terminalMessage(DataKeeper.LoadDataSet(commands[2]));
                    } else if (commands[1] == "save")
                    {
                        if (commands.Length == 2) Clients.All.terminalMessage(DataKeeper.SaveDataSet());
                        else Clients.All.terminalMessage(DataKeeper.SaveDataSet(commands[2]));
                    } else if( commands[1] == "unload")
                    {
                        NeuralNetworkHandler.keeper = null;
                    }
                } else if(head == "googledrive")
                {
                    if(commands[1] == "login")
                    {
                        GoogleDriveHandler.GoogleDriveLogin(HttpRuntime.AppDomainAppPath + "/credentials.json");
                        Clients.All.terminalMessage("Successfully logged in on Google Drive.");
                    } else if (commands[1] == "logout")
                    {
                        GoogleDriveHandler.GoogleDriveLogout("credentials.json");
                        Clients.All.terminalMessage("Successfully logged out from Google Drive.");
                    } else if (commands[1] == "list")
                    {
                        IList<Google.Apis.Drive.v3.Data.File> list = GoogleDriveHandler.GetFileList();

                        foreach (var file in list)
                        {
                            Clients.All.terminalMessage(String.Format("{0} ({1})", file.Name, file.Id));
                        }
                    }
                }
                else Clients.All.terminalError("That's not a valid command... Type \'help\' if you don\'t know what command to use.");
            } catch (Exception e)
            {
                Clients.All.terminalError("Command resolved in an error.");
                Clients.All.terminalError(e.Message);
                Clients.All.terminalError(e.StackTrace);
            }
        }
    }
}