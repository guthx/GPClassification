﻿using System;
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
    [ECConfiguration("ec.app.GPClassification.nodes.ConstantIntValueNode")]
    public class ConstantIntValueNode : ERC
    {
        public double value;
        public double printedValue;
        public int index;
        public override string ToStringForHumans()
        {
            return printedValue.ToString();
        }
        public override string ToString()
        {
            return "ERC(double)";
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
            if (dret.Type != DecodeReturn.T_DOUBLE)
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
            index = ((ClassificationData)input).index;
            var minVal = ((ClassificationProblem)problem).Dataset.MinValues[index];
            var maxVal = ((ClassificationProblem)problem).Dataset.MaxValues[index];
            var val = Math.Round(minVal + (maxVal - minVal) * value);
            printedValue = val;
            //if (val == maxVal)
            //    val = -1;
            ((ClassificationData)input).doubleVal = val;

        }

        public override void MutateERC(IEvolutionState state, int thread)
        {
            var diff = state.Random[thread].NextDouble() * 0.2 - 0.1;
            value = value + diff;
            if (value < 0)
                value = 0;
            if (value > 1)
                value = 1;
        }

        public override bool NodeEquals(GPNode node)
        {
            return (node.GetType() == this.GetType() && ((ConstantValueNode)node).value == value);
        }

        /*
        public override void ResetNode(IEvolutionState state, int thread)
        {
            var problem = (ClassificationProblem)state.Evaluator.p_problem;
            var dataset = problem.Dataset;
            var minVal = (int)dataset.MinValue;
            var maxVal = (int)dataset.MaxValue;
            var intValue = minVal + state.Random[thread].NextInt(maxVal - minVal + 1);
            value = (double)intValue;
        }
        */
        public override void ResetNode(IEvolutionState state, int thread)
        {
            value = state.Random[thread].NextDouble();
        }
    }
}
