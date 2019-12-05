using System;
using System.Collections.Generic;
using System.Text;
using BraneCloud.Evolution.EC;
using BraneCloud.Evolution.EC.Configuration;
using BraneCloud.Evolution.EC.GP;
using GPClassification.Data;
namespace GPClassification.Nodes
{
    [ECConfiguration("ec.app.GPClassification.nodes.NotNode")]
    public class NotNode : GPNode
    {
        public override void Eval(IEvolutionState state, int thread, GPData input, ADFStack stack, GPIndividual individual, IProblem problem)
        {
            ClassificationData result = (ClassificationData)input;
            Children[0].Eval(state, thread, input, stack, individual, problem);
            var value1 = result.boolVal;
            if(value1 == true)
            {
                result.boolVal = false;
            } else
            {
                result.boolVal = true;
            }
        }

        public override string ToString()
        {
            return "NOT";
        }
    }
}