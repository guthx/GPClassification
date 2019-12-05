using System;
using System.Collections.Generic;
using System.Text;
using BraneCloud.Evolution.EC;
using BraneCloud.Evolution.EC.Configuration;
using BraneCloud.Evolution.EC.GP;
using GPClassification.Data;
namespace GPClassification.Nodes
{
    [ECConfiguration("ec.app.GPClassification.nodes.IsCorruptedNode")]
    public class IsCorruptedNode : GPNode
    {
        public override void Eval(IEvolutionState state, int thread, GPData input, ADFStack stack, GPIndividual individual, IProblem problem)
        {
            ClassificationData data = (ClassificationData)input;
            Children[0].Eval(state, thread, input, stack, individual, problem);
            var value1 = data.doubleVal;
            if (value1 == -1)
            {
                data.boolVal = true;
            }
            else
            {
                data.boolVal = false;
            }
        }

        public override string ToString()
        {
            return "isC";
        }
    }
}