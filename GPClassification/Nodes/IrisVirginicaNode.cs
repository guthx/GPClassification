using System;
using System.Collections.Generic;
using System.Text;
using BraneCloud.Evolution.EC;
using BraneCloud.Evolution.EC.Configuration;
using BraneCloud.Evolution.EC.GP;
using GPClassification.Data;
namespace GPClassification.Nodes
{
    [ECConfiguration("ec.app.GPClassification.nodes.IrisVirginicaNode")]
    public class IrisVirginicaNode : GPNode
    {
        public override void Eval(IEvolutionState state, int thread, GPData input, ADFStack stack, GPIndividual individual, IProblem problem)
        {
            ClassificationData result = (ClassificationData)input;
            result.stringVal = "Iris-virginica";
        }

        public override string ToString()
        {
            return "Iris-virginica";
        }
    }
}
