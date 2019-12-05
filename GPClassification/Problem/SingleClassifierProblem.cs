using BraneCloud.Evolution.EC;
using BraneCloud.Evolution.EC.Configuration;
using BraneCloud.Evolution.EC.GP;
using BraneCloud.Evolution.EC.GP.Koza;
using BraneCloud.Evolution.EC.Simple;
using GPClassification.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace GPClassification.Problem
{
    [ECConfiguration("ec.app.GPClassification.SingleClassifierProblem")]
    public class SingleClassifierProblem : GPProblem, ISimpleProblem
    {
        public ClassificationData Input;
        public DataLoader Dataset;
        public string Label;
        public double[] CurrentParam;
        public string CurrentLabel;
        public int BestFitness = int.MaxValue;

        public override void Setup(IEvolutionState state, IParameter paramBase)
        {
            Dataset = new DataLoader(@"Datasets\gesty_pogrupowane.txt", 11, 1552, 1, 2, '\t');
            CurrentParam = new double[Dataset.AttributeCount];
            Label = SingleClassificationClass.Current;
            base.Setup(state, paramBase);
            Input = (ClassificationData)state.Parameters.GetInstanceForParameterEq(
                paramBase.Push(P_DATA), null, typeof(ClassificationData));
            Input.Setup(state, paramBase.Push(P_DATA));
        }

        public override object Clone()
        {
            // don't bother copying the inputs and outputs; they're read-only :-)
            // don't bother copying the currentValue; it's transitory
            // but we need to copy our regression data
            var myobj = (SingleClassifierProblem)(base.Clone());

            myobj.Input = (ClassificationData)(Input.Clone());
            return myobj;
        }

        public void Evaluate(IEvolutionState state, Individual ind, int subpop, int threadnum)
        {
            if (!ind.Evaluated)  // don't bother reevaluating
            {
                int TP = 0, TN = 0, FP = 0, FN = 0;
                var hits = 0;
                for (var y = 0; y < Dataset.InstanceCount; y++)
                {
                    CurrentLabel = Dataset.Labels[y];
                    for (var i = 0; i < Dataset.AttributeCount; i++)
                    {
                        CurrentParam[i] = Dataset.Data[y, i];
                    }
                    ((GPIndividual)ind).Trees[0].Child.Eval(state, threadnum, Input, Stack, ((GPIndividual)ind), this);
                    var isClassified = Input.boolVal;

                    if (CurrentLabel == Label)
                    {
                        if (isClassified == true)
                            TP++;
                        else
                            FN++;
                    } else
                    {
                        if (isClassified == true)
                            FP++;
                        else
                            TN++;
                    }

                }
                /*
                double sens = (double)TP / (TP + FN);
                double spec = (double)TN / (TN + FP);

                var fitness = 1 - (spec * sens);
                */
                var fitness = 8 * FP + 8 * FN + 2 * ind.Size;
                var f = ((KozaFitness)ind.Fitness);
                f.SetStandardizedFitness(state, (float)fitness);
                f.Hits = hits;
                ind.Evaluated = true;

            }
        }
    }
}
