using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab_24_Danylko
{
    public partial class Form1 : Form
    {
        private TextBox[] outputBoxes = new TextBox[3];
        private Button[] startButtons = new Button[3];
        private Button[] stopButtons = new Button[3];
        private Button startAllButton;
        private Button stopAllButton;
        private Label[] algorithmLabels = new Label[3];

        // Array to hold cancellation tokens for each crypto process
        private CancellationTokenSource[] cancellationTokenSources = new CancellationTokenSource[3];

        public Form1()
        {
            InitializeComponent();
            InitializeCustomComponents();
        }

        private void InitializeCustomComponents()
        {
            // Initialize the text boxes, labels, and buttons
            this.outputBoxes = new TextBox[3];
            this.startButtons = new Button[3];
            this.stopButtons = new Button[3];
            this.algorithmLabels = new Label[3];

            string[] algorithms = { "FEAL", "MD-2", "McEliece" };

            for (int i = 0; i < 3; i++)
            {
                // Labels
                this.algorithmLabels[i] = new Label();
                this.algorithmLabels[i].Text = algorithms[i];
                this.algorithmLabels[i].Location = new Point(10 + (i * 260), 30);
                this.algorithmLabels[i].Size = new Size(100, 20);
                this.Controls.Add(this.algorithmLabels[i]);

                // Text Boxes
                this.outputBoxes[i] = new TextBox();
                this.outputBoxes[i].Multiline = true;
                this.outputBoxes[i].ScrollBars = ScrollBars.Vertical;
                this.outputBoxes[i].Location = new Point(10 + (i * 260), 70);
                this.outputBoxes[i].Size = new Size(250, 200);
                this.Controls.Add(this.outputBoxes[i]);

                // Start Buttons
                this.startButtons[i] = new Button();
                this.startButtons[i].Text = "Start";
                this.startButtons[i].Location = new Point(10 + (i * 260), 10);
                this.startButtons[i].Size = new Size(100, 30);
                this.Controls.Add(this.startButtons[i]);

                // Stop Buttons
                this.stopButtons[i] = new Button();
                this.stopButtons[i].Text = "Stop";
                this.stopButtons[i].Location = new Point(120 + (i * 260), 10);
                this.stopButtons[i].Size = new Size(100, 30);
                this.stopButtons[i].Enabled = false;
                this.Controls.Add(this.stopButtons[i]);
            }

            // Start All Button
            this.startAllButton = new Button();
            this.startAllButton.Text = "Start All";
            this.startAllButton.Location = new Point(10, 300);
            this.startAllButton.Size = new Size(100, 30);
            this.Controls.Add(this.startAllButton);

            // Stop All Button
            this.stopAllButton = new Button();
            this.stopAllButton.Text = "Stop All";
            this.stopAllButton.Location = new Point(120, 300);
            this.stopAllButton.Size = new Size(100, 30);
            this.Controls.Add(this.stopAllButton);

            // Assign event handlers for buttons
            startButtons[0].Click += (sender, e) => StartCryptoProcess(0, "FEAL");
            startButtons[1].Click += (sender, e) => StartCryptoProcess(1, "MD-2");
            startButtons[2].Click += (sender, e) => StartCryptoProcess(2, "McEliece");

            stopButtons[0].Click += (sender, e) => StopCryptoProcess(0);
            stopButtons[1].Click += (sender, e) => StopCryptoProcess(1);
            stopButtons[2].Click += (sender, e) => StopCryptoProcess(2);

            startAllButton.Click += (sender, e) => {
                for (int i = 0; i < 3; i++) StartCryptoProcess(i, i == 0 ? "FEAL" : i == 1 ? "MD-2" : "McEliece");
            };

            stopAllButton.Click += (sender, e) => {
                for (int i = 0; i < 3; i++) StopCryptoProcess(i);
            };
        }

        private void StopCryptoProcess(int index)
        {
            if (cancellationTokenSources[index] != null)
            {
                cancellationTokenSources[index].Cancel();
                cancellationTokenSources[index].Dispose();
                cancellationTokenSources[index] = null;
            }

            stopButtons[index].Enabled = false;
            startButtons[index].Enabled = true;
        }

        private Task<string> FealSimulation(string input)
        {
            // Simulate FEAL algorithm
            return Task.Run(() => "FEAL:" + BitConverter.ToString(System.Text.Encoding.UTF8.GetBytes(input)).Replace("-", ""));
        }

        private Task<string> Md2Simulation(string input)
        {
            // Simulate MD-2 algorithm (for demonstration purposes, using a simple hash)
            return Task.Run(() => "MD-2:" + Convert.ToBase64String(System.Security.Cryptography.MD5.Create().ComputeHash(System.Text.Encoding.UTF8.GetBytes(input))));
        }

        private Task<string> McElieceSimulation(string input)
        {
            // Simulate McEliece algorithm (for demonstration purposes, using a simple transformation)
            return Task.Run(() => "McEliece:" + Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(input).Reverse().ToArray()));
        }

        private void StartCryptoProcess(int index, string method)
        {
            cancellationTokenSources[index] = new CancellationTokenSource();
            var token = cancellationTokenSources[index].Token;

            startButtons[index].Enabled = false;
            stopButtons[index].Enabled = true;

            List<string> data = new List<string> {
                "abcd123", "huhwei", "logmein", "testuser", "demoprofile", "user12345", "sampletext", "securekey",
                "encryption", "validuser", "unknown", "username", "clientserver", "storagekey", "accesscode",
                "authuser", "browsedata", "usertoken", "privatedata", "credentials", "admintest", "testlogin",
                "alphadata", "betaversion", "gammawave", "processor", "syntaxerror", "runtimeerror", "logicalbug",
                "compiletime", "executetime", "development", "programcode", "stacktrace", "heapmemory", "binarydata",
                "usertask", "schedule", "eventloop", "callback", "asynchronous", "synchronous", "threading", "concurrency",
                "parallelism", "multithreading", "dataflow", "performance", "optimization", "efficiency", "speedup",
                "scalability", "throughput", "workload", "load balance", "resource management", "scheduling",
                "dependency", "process synchronization", "race condition", "deadlock", "livelock", "starvation",
                "congestion", "throughput", "latency", "response time", "wait time", "execution time", "utilization",
                "benchmarking", "profiling", "bottleneck analysis", "capacity planning", "computational", "algorithmic",
                "heuristic", "analytical", "predictive model", "simulation", "queueing theory", "work stealing",
                "task parallelism", "data parallelism", "pipelining", "reduction", "partitioning", "merging", "batch processing",
                "stream processing", "vectorization", "parallel algorithm", "concurrent computing", "distributed computing",
                "cloud computing", "grid computing", "high-performance computing", "scientific computing", "supercomputing",
                "bioinformatics", "image processing", "signal processing", "numerical methods", "finite element analysis",
                "machine learning", "deep learning", "neural networks", "natural language processing", "data mining",
                "database operations", "transaction processing", "real-time processing", "embedded systems", "robotics",
                "automation", "cyber-physical systems", "internet of things", "blockchain", "cryptography", "security analysis",
                "fault tolerance", "redundancy", "recovery", "backup", "restore", "business continuity", "risk management",
                "compliance", "audit", "data integrity", "data privacy", "encryption", "decryption", "hashing", "digital signature"
                };

            Task.Run(async () => {
                foreach (var item in data)
                {
                    if (token.IsCancellationRequested)
                    {
                        Invoke(new Action(() => {
                            outputBoxes[index].Text += Environment.NewLine + "Process was cancelled." + Environment.NewLine;
                        }));
                        break;
                    }

                    string result = null;
                    try
                    {
                        switch (method)
                        {
                            case "FEAL":
                                result = await FealSimulation(item);
                                break;
                            case "MD-2":
                                result = await Md2Simulation(item);
                                break;
                            case "McEliece":
                                result = await McElieceSimulation(item);
                                break;
                            default:
                                result = "Unknown method";
                                break;
                        }

                        if (token.IsCancellationRequested)
                        {
                            Invoke(new Action(() => {
                                outputBoxes[index].Text += Environment.NewLine + "Process was cancelled before updating result." + Environment.NewLine;
                            }));
                            break;
                        }

                        // Update the UI with the result
                        Invoke(new Action(() => {
                            outputBoxes[index].Text += result + Environment.NewLine;
                        }));
                    }
                    catch (Exception ex)
                    {
                        Invoke(new Action(() => {
                            outputBoxes[index].Text += $"Error: {ex.Message}" + Environment.NewLine;
                        }));
                    }

                    await Task.Delay(500); // Simulate some processing time to see the UI updates
                }

                // Enable the Start button and disable the Stop button when the task completes or is cancelled
                Invoke(new Action(() => {
                    startButtons[index].Enabled = true;
                    stopButtons[index].Enabled = false;
                }));
            }).ContinueWith(t => {
                if (t.Exception != null)
                {
                    foreach (var e in t.Exception.InnerExceptions)
                    {
                        Invoke(new Action(() => {
                            outputBoxes[index].Text += $"Task Error: {e.Message}" + Environment.NewLine;
                        }));
                    }
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}