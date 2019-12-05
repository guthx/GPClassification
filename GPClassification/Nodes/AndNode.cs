using System;
using System.Collections.Generic;
using System.Text;
using BraneCloud.Evolution.EC;
using BraneCloud.Evolution.EC.Configuration;
using BraneCloud.Evolution.EC.GP;
using GPClassification.Data;
namespace GPClassification.Nodes
{
    [ECConfiguration("ec.app.GPClassification.nodes.AndNode")]
    public class AndNode : GPNode
    {
        public override void Eval(IEvolutionState state, int thread, GPData input, ADFStack stack, GPIndividual individual, IProblem problem)
        {
            ClassificationData result = (ClassificationData)input;
            Children[0].Eval(state, thread, input, stack, individual, problem);
            var value1 = result.boolVal;
            Children[1].Eval(state, thread, input, stack, individual, problem);
            var value2 = result.boolVal;
            if (value1 && value2)
            {
                result.boolVal = true;
            }
            else
            {
                result.boolVal = false;
            }
        }

        public override string ToString()
        {
            return "AND";
        }
    }
}