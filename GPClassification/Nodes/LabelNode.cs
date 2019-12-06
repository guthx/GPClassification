using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using BraneCloud.Evolution.EC;
using BraneCloud.Evolution.EC.Configuration;
using BraneCloud.Evolution.EC.GP;
using BraneCloud.Evolution.EC.Util;
using GPClassification.Problem;
using GPClassification.Data;
using System.Linq;

namespace GPClassification.Nodes
{
    [ECConfiguration("ec.app.GPClassification.nodes.LabelNode")]
    public class LabelNode : ERC
    {
        public long labelIndex;
        public string label;
        public override string ToStringForHumans()
        {
            return label;
        }
        public override string ToString()
        {
            return "ERC(label)";
        }
      //  public override string Name => "ERC(label)";
        public override string Encode()
        {
            return Code.Encode(labelIndex);
        }
        public override bool Decode(DecodeReturn dret)
        {
            int pos = dret.Pos;
            String data = dret.Data;
            Code.Decode(dret);
            if (dret.Type != DecodeReturn.T_LONG)
            {
                dret.Data = data;
                dret.Pos = pos;
                return false;
            }
            labelIndex = dret.L;
            return true;
        }

        public override void Eval(IEvolutionState state, int thread, GPData input, ADFStack stack, GPIndividual individual, IProblem problem)
        {
            var cProblem = (ClassificationProblem)problem;
            ((ClassificationData)input).stringVal = cProblem.Dataset.Classes.ElementAt((int)labelIndex);
        }

        public override bool NodeEquals(GPNode node)
        {
            return (node.GetType() == this.GetType() && ((LabelNode)node).labelIndex == labelIndex);
        }


        public override void ResetNode(IEvolutionState state, int thread)
        {
            var problem = (ClassificationProblem)state.Evaluator.p_problem;
            var classCount = problem.Dataset.ClassCount;
            labelIndex = state.Random[thread].NextInt(classCount);
            label = problem.Dataset.Classes.ElementAt((int)labelIndex);
        }
    }
}
