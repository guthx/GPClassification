using System;
using BraneCloud.Evolution.EC;
using BraneCloud.Evolution.EC.GP;
using BraneCloud.Evolution.EC.Configuration;
using System.Reflection;
using System.Globalization;
using BraneCloud.Evolution.EC.Simple;
using GPClassification.Data;
using GPClassification.Problem;
using System.Diagnostics;
using System.Threading;
using System.Linq;
using BraneCloud.Evolution.EC.GP.Koza;
using System.Collections.Generic;

namespace GPClassification
{
    class Program
    {

        public enum ClassifierType { SINGLE_CLASSIFIER, MULTI_CLASSIFIER };
        static void Main()
        {
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
            /* var threads = new Thread[3];
             for(var i=0; i<3; i++)
             {
                 threads[i] = new Thread(new ThreadStart(StartGP));
                 threads[i].
                 threads[i].Start();
             }*/
            /*
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
            var test = new DataLoader(@"Datasets\gesty_pogrupowane.txt", 11, 1491, 1, 2, '\t');
          
            ECActivator.AddSourceAssemblies(new[] { Assembly.GetAssembly(typeof(IEvolutionState)), Assembly.GetAssembly(typeof(ClassificationProblem)) });
            IEvolutionState state = Evolve.Initialize(Evolve.LoadParameterDatabase(new[] { "-file", @"Params\App\Iris\koza.params" }), 2);
            state.Run(EvolutionState.C_STARTED_FRESH);
            var best = ((SimpleStatistics)((SimpleEvolutionState)state).Statistics).BestOfRun;
            var data = new ClassificationData();
            state.Output.AddLog(@"F:\Logs\test.gv");
            ((GPIndividual)best[0]).Trees[0].PrintStyle = GPTree.PRINT_STYLE_DOT;
            ((GPIndividual)best[0]).Trees[0].PrintTreeForHumans(state, state.Output.NumLogs - 1);
            Process graphViz = new Process();
            graphViz.StartInfo.FileName = @"F:\Instalki\graphviz-2.38\release\bin\dot.exe";
            graphViz.StartInfo.Arguments = @"-Tps F:\Logs\test.gv -o F:\Logs\test.ps";
            graphViz.Start();
            graphViz.WaitForExit();
            Process ghostscript = new Process();
            ghostscript.StartInfo.FileName = @"F:\Program Files\gs\gs9.50\bin\gswin64.exe";
            ghostscript.StartInfo.Arguments = @"-dNOPAUSE -g1920x1080 F:\Logs\test.ps";
          //  ghostscript.Start();
          //  ghostscript.WaitForExit();
          */
            //StartSCProblem();
            // StartGP();
            // StartSCProblem();
            //CheckTreeFromFile();
            
             
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Ktory model klasyfikatora chcesz utworzyc?");
                Console.WriteLine("1. Single Classificator");
                Console.WriteLine("2. Multi Classificator");
                Console.WriteLine("3. Sprawdz klasyfikator jako kod");
                var key = Console.ReadKey();
                while (key.KeyChar != '1' && key.KeyChar != '2' && key.KeyChar != '3')
                    key = Console.ReadKey();
                if (key.KeyChar == '1')
                    StartGP();
                if (key.KeyChar == '2')
                    StartSCProblem();
                if (key.KeyChar == '3')
                    CheckCodeClassifier();

                Console.WriteLine("Done!");
                Console.ReadKey();
            }
            
            
          //  Console.WriteLine(p);
            //  TestCode();
           // Console.WriteLine("\nDone!");
           // Console.ReadLine();


        }

        private static void CheckCodeClassifier()
        {
            var dataSet = new DataLoader(@"Datasets\gesty_pogrupowane.txt", 11, 12664, 1704, 2, '\t');
            var accuracy = CodeClassifier.CheckClassifier(dataSet);
            Console.WriteLine();
            Console.WriteLine(accuracy);
        }

        private static void TestCode()
        {
            ECActivator.AddSourceAssemblies(new[] { Assembly.GetAssembly(typeof(IEvolutionState)), Assembly.GetAssembly(typeof(ClassificationProblem)) });
            var parameters = Evolve.LoadParameterDatabase(new[] { "-file", @"Params\App\Iris\koza.params" });
            IEvolutionState state = Evolve.Initialize(parameters, 0);
            state.Setup(state, new Parameter(new string[] { "a" }));
            //state.Run(EvolutionState.C_STARTED_FRESH);
         //   var individual = (GPIndividual)((SimpleStatistics)state.Statistics).BestOfRun[0];
            var tree = new GPTree();
            var reader = new System.IO.StreamReader("single_cl_ecj_graph.txt");
            tree.ReadTree(state, reader);
            var writer = new System.IO.StreamWriter("testcode.txt");
            var code = TreeReader.PrintCodeFromTree(tree);
            writer.Write(code);
            writer.Close();
        }
        private static void StartGP()
        {
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
            var test = new DataLoader(@"Datasets\gesty_pogrupowane.txt", 11, 1491, 1, 2, '\t');

            ECActivator.AddSourceAssemblies(new[] { Assembly.GetAssembly(typeof(IEvolutionState)), Assembly.GetAssembly(typeof(ClassificationProblem)) });
            IEvolutionState state = Evolve.Initialize(Evolve.LoadParameterDatabase(new[] { "-file", @"Params\App\Iris\koza.params" }), 2);
            state.Run(EvolutionState.C_STARTED_FRESH);
            var best = ((SimpleStatistics)((SimpleEvolutionState)state).Statistics).BestOfRun[0];
            var date = DateTime.Now.Ticks.ToString();
            var directoryName = "stats/" + date;
            var directory = System.IO.Directory.CreateDirectory(directoryName);
            var stats = new List<string>();
            stats.Add("Ilość pokoleń: " + state.NumGenerations);
            var accuracy = (double)(1552 - ((KozaFitness)best.Fitness).StandardizedFitness) / 1552;     
            stats.Add("Rozmiar drzewa: " + ((GPIndividual)best).Size.ToString());
            stats.Add("Głębokość drzewa: " + ((GPIndividual)best).Trees[0].Child.Depth.ToString());
            stats.Add("Poprawność klasyfikacji dla danych uczących: " + accuracy.ToString());

            System.IO.File.WriteAllLines(directoryName + "/single_cl_stats.txt", stats.ToArray());

            var writer = new System.IO.StreamWriter(directoryName + "/classifier_code.txt");
            var code = TreeReader.PrintCodeFromTree(((GPIndividual)best).Trees[0]);
            writer.Write(code);
            writer.Close();

            int humanGraph = state.Output.AddLog(directoryName + "/single_cl_human_graph.txt");
            int ecjGraph = state.Output.AddLog(directoryName + "/single_cl_ecj_graph.txt");

            ((GPIndividual)best).Trees[0].PrintStyle = GPTree.PRINT_STYLE_DOT;
            ((GPIndividual)best).Trees[0].PrintTreeForHumans(state, humanGraph);
            ((GPIndividual)best).Trees[0].PrintTree(state, ecjGraph);

            CheckClassifierOnTestData(new[] { best }, state, (int)ClassifierType.SINGLE_CLASSIFIER, directoryName);
            //var data = new ClassificationData();
            /*
            state.Output.AddLog(@"F:\Logs\test.gv");
            ((GPIndividual)best[0]).Trees[0].PrintStyle = GPTree.PRINT_STYLE_DOT;
            ((GPIndividual)best[0]).Trees[0].PrintTreeForHumans(state, state.Output.NumLogs - 1);
            ((GPIndividual)best[0]).Trees[0].PrintTree(state, state.Output.NumLogs - 1);
            */
        }

        private static void CheckTreeFromFile()
        {
            ECActivator.AddSourceAssemblies(new[] { Assembly.GetAssembly(typeof(IEvolutionState)), Assembly.GetAssembly(typeof(ClassificationProblem)) });
            var parameters = Evolve.LoadParameterDatabase(new[] { "-file", @"Params\App\Iris\koza.params" });
            IEvolutionState state = Evolve.Initialize(parameters, 0);
            state.Setup(state, new Parameter(new string[] {"a" } ));
            //state.Run(EvolutionState.C_STARTED_FRESH);
            var individual = (GPIndividual)((SimpleStatistics)state.Statistics).BestOfRun[0];
            var tree = new GPTree();
            var reader = new System.IO.StreamReader("single_cl_ecj_graph.txt");
            tree.ReadTree(state, reader);
          //  individual.Setup(state, new Parameter(new string[] { "numtrees = 1", "1" }));
            individual.Trees[0] = tree;
        
            
            

           // CheckClassifierOnTestData(new[] { individual }, state, (int)ClassifierType.SINGLE_CLASSIFIER, directoryName);
        }

        private static void StartSCProblem()
        {
            var bestClassifiers = new Individual[8];
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
            ECActivator.AddSourceAssemblies(new[] { Assembly.GetAssembly(typeof(IEvolutionState)), Assembly.GetAssembly(typeof(SingleClassifierProblem)) });
            var parameters = Evolve.LoadParameterDatabase(new[] { "-file", @"Params\App\Iris\single.params"});
            IEvolutionState state = Evolve.Initialize(parameters, 0);
            var date = DateTime.Now.Ticks.ToString();
            var directoryName = "stats/" + date;
            var directory = System.IO.Directory.CreateDirectory(directoryName);
            var writer = new System.IO.StreamWriter(directoryName + "/multi_cl_stats.txt", false);
          //  IEvolutionState state = Evolve.Initialize(Evolve.LoadParameterDatabase(new[] { "-file", @"Params\App\Iris\single.params" }), 0);

            for(var i = 0; i < 8; i++)
            {
               // parameters = Evolve.LoadParameterDatabase(new[] { "-file", @"Params\App\Iris\single.params", "-stat.file", @"out1.stat" });
                if (i != 0)
                state = Evolve.Initialize(parameters, 0);
                SingleClassificationClass.Current = SingleClassificationClass.Labels[i];
                // state.Output.FilePrefix = i.ToString();
                state.Run(EvolutionState.C_STARTED_FRESH);
                var best = ((SimpleStatistics)((SimpleEvolutionState)state).Statistics).BestOfRun[0];

                int humanGraph = state.Output.AddLog(directoryName + "/multi_cl_human_graph_" + i.ToString() + ".txt" );
                int ecjGraph = state.Output.AddLog(directoryName + "/multi_cl_ecj_graph_" + i.ToString() + ".txt");
                ((GPIndividual)best).Trees[0].PrintStyle = GPTree.PRINT_STYLE_DOT;
                ((GPIndividual)best).Trees[0].PrintTreeForHumans(state, humanGraph);
                ((GPIndividual)best).Trees[0].PrintTree(state, ecjGraph);

                state.Output.Close();
                // System.IO.File.Delete("out.stat");
                bestClassifiers[i] = best;

                var accuracy = (double)(1552 - ((KozaFitness)bestClassifiers[i].Fitness).StandardizedFitness) / 1552;
                writer.WriteLine("Klasyfikator dla gestu " + SingleClassificationClass.Labels[i]);
                writer.WriteLine("Ilość generacji: " + state.NumGenerations.ToString());
                writer.WriteLine("Rozmiar drzewa: " + ((GPIndividual)bestClassifiers[i]).Size.ToString());
                writer.WriteLine("Poprawność klasyfikacji: " + accuracy.ToString());
                writer.WriteLine();
            //    ((GPIndividual)bestClassifiers[0]).Trees[0].Child.Eval
            //((GPIndividual)bestClassifiers[0]).Trees[0].
            //  ((SingleClassifierProblem)state.Evaluator.p_problem)

            }

            writer.Close();

            CheckClassifierOnTestData(bestClassifiers, state, (int)ClassifierType.MULTI_CLASSIFIER, directoryName);

        }

        private static void CheckClassifierOnTestData(Individual[] classifiers, IEvolutionState state, int classifierType, string directoryName)
        {
            if (classifierType == (int)ClassifierType.MULTI_CLASSIFIER)
            {
                var labeledClassifiers = new LabeledClassifier[classifiers.Length];
                for (var i = 0; i < classifiers.Length; i++)
                {
                    var labeledClassifier = new LabeledClassifier
                    {
                        Classifier = (GPIndividual)classifiers[i],
                        Label = SingleClassificationClass.Labels[i]
                    };
                    labeledClassifiers[i] = labeledClassifier;
                }

                labeledClassifiers = labeledClassifiers.OrderByDescending(c => ((KozaFitness)c.Classifier.Fitness).AdjustedFitness).ToArray();

                var input = ((SingleClassifierProblem)state.Evaluator.p_problem).Input;
                var stack = ((SingleClassifierProblem)state.Evaluator.p_problem).Stack;
                var problem = (SingleClassifierProblem)state.Evaluator.p_problem;

                

                int correct = 0, incorrect = 0;

                var dataSet = new DataLoader(@"Datasets\gesty_pogrupowane.txt", 11, 12664, 1704, 2, '\t');
                for (var j = 0; j < dataSet.InstanceCount; j++)
                {
                    for (var x = 0; x < dataSet.AttributeCount; x++)
                    {
                        problem.CurrentParam[x] = dataSet.Data[j, x];
                    }
                    for (var i = 0; i < labeledClassifiers.Length; i++)
                    {
                        var individual = labeledClassifiers[i].Classifier;

                        individual.Trees[0].Child.Eval(state, 0, input, stack, individual, problem);
                        if (input.boolVal == true)
                        {
                            if (labeledClassifiers[i].Label == dataSet.Labels[j])
                            {
                                correct++;
                                break;
                            }
                            else
                            {
                                incorrect++;
                                break;
                            }
                        }
                        if (i == labeledClassifiers.Length - 1)
                        {
                            if (labeledClassifiers[i].Label == dataSet.Labels[j])
                            {
                                correct++;
                                break;
                            }
                            else
                            {
                                incorrect++;
                                break;
                            }
                        }
                    }
                }


                double classifierAccuracy = (double)correct / (correct + incorrect);

                var writer = new System.IO.StreamWriter(directoryName + "/multi_cl_stats.txt", true);
                writer.WriteLine("Całkowita poprawność klasyfikacji dla danych testowych: " + classifierAccuracy.ToString());
                writer.Close();

                Console.WriteLine("Dokładność multi-klasyfikatora dla danych testowych: {0}", classifierAccuracy);
            }
            else if (classifierType == (int)ClassifierType.SINGLE_CLASSIFIER)
            {
                var individual = (GPIndividual)classifiers[0];
                var input = ((ClassificationProblem)state.Evaluator.p_problem).Input;
                var stack = ((ClassificationProblem)state.Evaluator.p_problem).Stack;
                var problem = (ClassificationProblem)state.Evaluator.p_problem;

                int correct = 0, incorrect = 0;

                var dataSet = new DataLoader(@"Datasets\gesty_pogrupowane.txt", 11, 12664, 1704, 2, '\t');
                for (var j = 0; j < dataSet.InstanceCount; j++)
                {
                    for (var x = 0; x < dataSet.AttributeCount; x++)
                    {
                        problem.CurrentParam[x] = dataSet.Data[j, x];
                    }
                    individual.Trees[0].Child.Eval(state, 0, input, stack, individual, problem);
                    if (input.stringVal == dataSet.Labels[j])
                        correct++;
                    else
                        incorrect++;

                }
                double classifierAccuracy = (double)correct / (correct + incorrect);

                var writer = new System.IO.StreamWriter(directoryName + "/single_cl_stats.txt", true);
                writer.WriteLine("Dokładność klasyfikatora dla danych testowych: " + classifierAccuracy);
                writer.Close();

                Console.WriteLine("Dokładność pojedynczego klasyfikatora dla danych testowych: {0}", classifierAccuracy);
            }
        } 

    }
}
