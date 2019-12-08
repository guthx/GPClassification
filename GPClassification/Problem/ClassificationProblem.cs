using System;
using System.Collections.Generic;
using System.Text;
using BraneCloud.Evolution.EC;
using BraneCloud.Evolution.EC.GP;
using BraneCloud.Evolution.EC.GP.Koza;
using BraneCloud.Evolution.Archetype;
using BraneCloud.Evolution.EC.Configuration;
using BraneCloud.Evolution.EC.Simple;
using GPClassification.Data;

namespace GPClassification.Problem
{
    [ECConfiguration("ec.app.GPClassification.ClassificationProblem")]
    public class ClassificationProblem : GPProblem, ISimpleProblem
    {
        public ClassificationData Input;
        public DataLoader Dataset;
        public double[] CurrentParam;
        public int BestFitness = int.MaxValue;
        public int StagnationCount = 0;

        public override void Setup(IEvolutionState state, IParameter paramBase)
        {
            Dataset = new DataLoader(@"Datasets\gesty_pogrupowane.txt", 11, 1552, 1, 2, '\t');
            CurrentParam = new double[Dataset.AttributeCount];
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
            var myobj = (ClassificationProblem)(base.Clone());

            myobj.Input = (ClassificationData)(Input.Clone());
            return myobj;
        }

        public void Evaluate(IEvolutionState state, Individual ind, int subpop, int threadnum)
        {
            if (!ind.Evaluated)  // don't bother reevaluating
            {
                int failedClassify = 0;
                var hits = 0;
                for (var y = 0; y < Dataset.InstanceCount; y++)
                {
                    for(var i=0; i<Dataset.AttributeCount; i++)
                    {
                        CurrentParam[i] = Dataset.Data[y, i];
                    }
                    ((GPIndividual)ind).Trees[0].Child.Eval(state, threadnum, Input, Stack, ((GPIndividual)ind), this);
                    var classifiedLabel = Input.stringVal;
                    // It's possible to get NaN because cos(infinity) and
                    // sin(infinity) are undefined (hence cos(exp(3000)) zings ya!)
                    // So since NaN is NOT =,<,>,etc. any other number, including
                    // NaN, we're CAREFULLY wording our cutoff to include NaN.
                    // Interesting that this has never been reported before to
                    // my knowledge.
                    if (classifiedLabel != Dataset.Labels[y])
                    {
                        failedClassify++;
                    }
                    
                    

                }
                if (failedClassify < 5)
                {
                    hits++;
                }
                var penalty = ind.Size > 50 ? (ind.Size - 50)*2 : 0;
                failedClassify *= 8;
                failedClassify += (int)penalty;
                // the fitness better be KozaFitness!
                var f = ((KozaFitness)ind.Fitness);
                f.SetStandardizedFitness(state, (float)failedClassify);
                f.Hits = hits;
                ind.Evaluated = true;
                /*
                if (failedClassify < BestFitness)
                {
                    BestFitness = failedClassify;
                    StagnationCount = 0;
                } else
                {
                    StagnationCount++;
                    if (StagnationCount > 30)
                    {
                        state.Evaluator.RunComplete(state);
                        state.Finish();
                    }
                }*/
            }
        }
    }
}
