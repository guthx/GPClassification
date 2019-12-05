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
namespace GPClassification.Nodes
{
    [ECConfiguration("ec.app.GPClassification.nodes.ParameterNode")]
    public class ParameterNode : ERC
    {
        public long paramIndex;

        public override string ToStringForHumans()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("P");
            stringBuilder.Append(paramIndex.ToString());
            return stringBuilder.ToString();
        }
        public override string Encode()
        {
            return Code.Encode(paramIndex);
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
            paramIndex = dret.L;
            return true;
        }

        public override void Eval(IEvolutionState state, int thread, GPData input, ADFStack stack, GPIndividual individual, IProblem problem)
        {
            var cProblem = (ClassificationProblem)problem;
            ((ClassificationData)input).doubleVal = cProblem.CurrentParam[paramIndex];
            ((ClassificationData)input).index = (int)paramIndex;
        }

        public override bool NodeEquals(GPNode node)
        {
            return (node.GetType() == this.GetType() && ((ParameterNode)node).paramIndex == paramIndex);
        }


        public override void ResetNode(IEvolutionState state, int thread)
        {
            var problem = (ClassificationProblem)state.Evaluator.p_problem;
            var attributeCount = problem.Dataset.AttributeCount;
            paramIndex = state.Random[thread].NextInt(attributeCount);
        }
    }
}
