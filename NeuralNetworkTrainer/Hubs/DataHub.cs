using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNet.SignalR;

using NeuralNetwork.Core;
using NeuralNetwork.Layers;


namespace NeuralNetworkTrainer.Hubs
{
    public class DataHub : Hub
    {

        
        public void GetTerminalLog()
        {
            Clients.Caller.terminalMessage(ConsoleLogger.log);
        }

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

        public void GetNewTrainingLoss(int currentNumberOfLoss)
        {
            float[] loss = NeuralNetworkHandler.TrainLoss.ToArray();
            if (loss == null || loss.Length - currentNumberOfLoss <= 0)
            {
                Clients.Caller.giveNewTrainingLoss(null, null);
                return;
            }

            float[] newLoss = new float[loss.Length - currentNumberOfLoss];
            Array.Copy(loss, loss.Length - (loss.Length - currentNumberOfLoss), newLoss, 0, loss.Length - currentNumberOfLoss);
            
            float[] newLR = new float[loss.Length - currentNumberOfLoss];
            Array.Copy(NeuralNetworkHandler.LearningRate.ToArray(), loss.Length - (loss.Length - currentNumberOfLoss), newLR, 0, loss.Length - currentNumberOfLoss);

            Clients.Caller.giveNewTrainingLoss(newLoss, newLR);
        }

        public void GetNewValidationLoss(int currentNumberOfLoss)
        {
            float[] loss = NeuralNetworkHandler.ValidLoss.ToArray();
            if (loss == null || loss.Length - currentNumberOfLoss <= 0)
            {
                Clients.Caller.giveNewValidationLoss(null);
                return;
            }

            float[] newLoss = new float[loss.Length - currentNumberOfLoss];
            Array.Copy(loss, loss.Length - (loss.Length - currentNumberOfLoss), newLoss, 0, loss.Length - currentNumberOfLoss);

            Clients.Caller.giveNewValidationLoss(newLoss);
        }
        
        public void TerminalCommand(String command)
        {
            Console.WriteLine(command);
            try
            {
                command = command.ToLower();
                if (String.IsNullOrEmpty(command) || String.IsNullOrWhiteSpace(command))
                {
                    Console.WriteLine("That's not a valid command... Type \'help\' if you don\'t know what command to use.");
                    return;
                }
                String[] commands = command.Split(' ');
                String head = commands[0];



                if (head == "neuralnetwork")
                {
                    Model network = NeuralNetworkHandler.network;
                    if (commands.Length == 1)
                    {
                        if (network == null) Console.WriteLine("There is no Network currently loaded.");
                        else
                        {
                            String message =
                                "Current loaded network:\n" +
                                "\tName:\t" + network.Name + "\n" +
                                "\tType:\t" + network.Type;
                            Console.WriteLine(message);
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
                                else if (commands[i] == "sigmoid")
                                {
                                    l = new SigmoidLayer();
                                }
                                else
                                {
                                    Console.Error.WriteLine(String.Format("Layer type {0} does not exist.", commands[i]));
                                    return;
                                }
                                l.nodes = int.Parse(commands[i + 1]);
                                layers.Add(l);
                            }
                            NeuralNetworkHandler.CreateNetwork(layers.ToArray(), commands[2], commands[3]);
                            Console.WriteLine("Neural Network successfully created.");
                            return;
                        }
                    } else if(commands[1] == "load")
                    {
                        Console.WriteLine(NeuralNetworkHandler.LoadNetwork(commands[2]));
                    } else if(commands[1] == "save")
                    {
                        if(commands.Length == 2) Console.WriteLine(NeuralNetworkHandler.SaveNetwork());
                        else Console.WriteLine(NeuralNetworkHandler.SaveNetwork(commands[2]));
                    } else if(commands[1] == "randomize")
                    {
                        Console.WriteLine(NeuralNetworkHandler.RandomizeNetwork());
                    } else if(commands[1] == "train")
                    {
                        if (network == null)
                        {
                            Console.Error.WriteLine("No network loaded.");
                            return;
                        }
                        if (NeuralNetworkHandler.keeper == null)
                        {
                            Console.Error.WriteLine("No dataset loaded.");
                            return;
                        }

                        int epochs = 1;
                        if (commands.Length != 2) epochs = int.Parse(commands[2]);
                        NeuralNetworkHandler.train = new Thread(() => NeuralNetworkHandler.TrainNetwork(epochs));
                        NeuralNetworkHandler.train.IsBackground = true;
                        NeuralNetworkHandler.train.Name = "Training Thread";


                        NeuralNetworkHandler.train.Start();
                    } else if(commands[1] == "learningrate")
                    {
                        if (commands.Length == 2) Console.WriteLine("Learning rate: " + Model.LearningRate);
                        else Model.LearningRate = float.Parse(commands[2]);
                    }
                } else if (head == "test")
                {

                    Data[] data = new Data[3600];

                    float x = 0;

                    for(int i = 0; i < data.Length; i++)
                    {
                        data[i] = new Data();
                        data[i].Inputs = new Matrix(1, 1) { data = new float[,] { { (1f/360f)*x } } };
                        data[i].Targets = new Matrix(1, 1) { data = new float[,] { { (float)Math.Sin(x * (Math.PI / 180)) } } };
                        x += 0.1f;
                    }

                    NeuralNetworkHandler.keeper = new DataKeeper();
                    NeuralNetworkHandler.keeper.DataSet = data;
                    NeuralNetworkHandler.keeper.Name = "sinusfunction";

                } else if(head == "dataset")
                {
                    if(commands.Length == 1)
                    {
                        if (NeuralNetworkHandler.keeper == null) Console.WriteLine("There is no dataset loaded yet.");
                        else Console.WriteLine(String.Format("The dataset with the name {0} is currently loaded.", NeuralNetworkHandler.keeper.Name));
                    } else if(commands[1] == "load")
                    {
                        Console.WriteLine(DataKeeper.LoadDataSet(commands[2]));
                    } else if (commands[1] == "save")
                    {
                        if (commands.Length == 2) Console.WriteLine(DataKeeper.SaveDataSet());
                        else Console.WriteLine(DataKeeper.SaveDataSet(commands[2]));
                    } else if(commands[1] == "unload")
                    {
                        NeuralNetworkHandler.keeper = null;
                    } else if (commands[1] == "view")
                    {
                        if(NeuralNetworkHandler.keeper == null)
                        {
                            Console.Error.WriteLine("No dataset loaded.");
                            return;
                        }
                        for(int i = 0; i < NeuralNetworkHandler.keeper.DataSet.Length; i++)
                        {
                            Console.WriteLine("Data " + i + ":");
                            Console.WriteLine("Inputs:");
                            Console.WriteLine(NeuralNetworkHandler.keeper.DataSet[i].getInputString());
                            Console.WriteLine("Targets:");
                            Console.WriteLine(NeuralNetworkHandler.keeper.DataSet[i].getTargetsString());
                            Console.WriteLine("");
                        }
                    }
                } else if(head == "googledrive")
                {
                    if(commands[1] == "login")
                    {
                        GoogleDriveHandler.GoogleDriveLogin(HttpRuntime.AppDomainAppPath + "/credentials.json");
                        Console.WriteLine("Successfully logged in on Google Drive.");
                    } else if (commands[1] == "logout")
                    {
                        GoogleDriveHandler.GoogleDriveLogout("credentials.json");
                        Console.WriteLine("Successfully logged out from Google Drive.");
                    } else if (commands[1] == "list")
                    {
                        IList<Google.Apis.Drive.v3.Data.File> list = GoogleDriveHandler.GetFileList();

                        foreach (var file in list)
                        {
                            Console.WriteLine(String.Format("{0} ({1})", file.Name, file.Id));
                        }
                    }

                    
                }
                else Console.Error.WriteLine("That's not a valid command... Type \'help\' if you don\'t know what command to use.");
            } catch (Exception e)
            {
                Console.Error.WriteLine("Command resolved in an error.");
                Console.Error.WriteLine(e.Message);
                Console.Error.WriteLine(e.StackTrace);
            }
        }
        

        public static void MessageClients(String message)
        {
            GlobalHost.ConnectionManager.GetHubContext<DataHub>().Clients.All.terminalMessage(message);
        }

        
    }
}