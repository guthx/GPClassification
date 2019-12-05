using System;
using System.Collections.Generic;
using System.Text;
using BraneCloud.Evolution.EC;
using BraneCloud.Evolution.EC.Configuration;
using BraneCloud.Evolution.EC.GP;
using GPClassification.Data;
namespace GPClassification.Nodes
{
    [ECConfiguration("ec.app.GPClassification.nodes.IfNodeSingle")]
    public class IfNodeSingle : GPNode
    {
        public override void Eval(IEvolutionState state, int thread, GPData input, ADFStack stack, GPIndividual individual, IProblem problem)
        {
            ClassificationData data = (ClassificationData)input;
            Children[0].Eval(state, thread, input, stack, individual, problem);
            if (data.boolVal)
            {
                Children[1].Eval(state, thread, input, stack, individual, problem);
            }
            else
            {
                Children[2].Eval(state, thread, input, stack, individual, problem);
            }


        }

        public override string ToString()
        {
            return "IF";
        }
    }
}
