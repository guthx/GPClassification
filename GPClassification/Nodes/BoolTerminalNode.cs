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
    [ECConfiguration("ec.app.GPClassification.nodes.BoolTerminalNode")]
    public class BoolTerminalNode : ERC
    {
        public bool value;

        public override string ToStringForHumans()
        {
            return value.ToString();
        }

        public override string ToString()
        {
            return "ERC(bool)";
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
            if (dret.Type != DecodeReturn.T_BOOLEAN)
            {
                dret.Data = data;
                dret.Pos = pos;
                return false;
            }
            value = dret.B.GetValueOrDefault();
            return true;
        }

        public override void Eval(IEvolutionState state, int thread, GPData input, ADFStack stack, GPIndividual individual, IProblem problem)
        {
            ((ClassificationData)input).boolVal = value;
        }

        public override bool NodeEquals(GPNode node)
        {
            return (node.GetType() == this.GetType() && ((BoolTerminalNode)node).value == value);
        }


        public override void ResetNode(IEvolutionState state, int thread)
        {
            value = state.Random[thread].NextBoolean();
        }
    }
}
