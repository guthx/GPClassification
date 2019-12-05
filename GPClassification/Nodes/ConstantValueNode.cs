using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using BraneCloud.Evolution.EC;
using BraneCloud.Evolution.EC.Configuration;
using BraneCloud.Evolution.EC.GP;
using BraneCloud.Evolution.EC.Util;
using GPClassification.Data;
using GPClassification.Problem;

namespace GPClassification.Nodes
{
    [ECConfiguration("ec.app.GPClassification.nodes.ConstantValueNode")]
    public class ConstantValueNode : ERC
    {
        public double value;

        public override string ToStringForHumans()
        {
            return value.ToString();
        }
        public override string Encode()
        {
            return Code.Encode(value);
        }
        public override bool Decode(DecodeReturn dret)
        {
            int pos = dret.Pos;
            String data = dret.Data;
            Code.Decode(dret);
            if(dret.Type != DecodeReturn.T_DOUBLE)
            {
                dret.Data = data;
                dret.Pos = pos;
                return false;
            }
            value = dret.D;
            return true;
        }

        public override void Eval(IEvolutionState state, int thread, GPData input, ADFStack stack, GPIndividual individual, IProblem problem)
        {
            ((ClassificationData)input).doubleVal = value;
        }

        public override bool NodeEquals(GPNode node)
        {
            return (node.GetType() == this.GetType() && ((ConstantValueNode)node).value == value);
        }

        public override void MutateERC(IEvolutionState state, int thread)
        {
            base.MutateERC(state, thread);
            var problem = (ClassificationProblem)state.Evaluator.p_problem;
            var dataset = problem.Dataset;
            var mutationMax = (dataset.MaxValue - dataset.MinValue)/10;
            var mutationValue = -mutationMax + state.Random[thread].NextDouble() * mutationMax * 2;
            value += mutationValue;
        }



        public override void ResetNode(IEvolutionState state, int thread)
        {
            var problem = (ClassificationProblem)state.Evaluator.p_problem;
            var dataset = problem.Dataset;
            value = dataset.MinValue + (state.Random[thread].NextDouble() * (dataset.MaxValue - dataset.MinValue));
        }
    }
}
