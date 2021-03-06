﻿using System;
using System.Collections.Generic;
using System.Text;
using BraneCloud.Evolution.EC;
using BraneCloud.Evolution.EC.Configuration;
using BraneCloud.Evolution.EC.GP;
using GPClassification.Data;
namespace GPClassification.Nodes
{
    [ECConfiguration("ec.app.GPClassification.nodes.BoolFalseNode")]
    public class BoolFalseNode : GPNode
    {
        public override void Eval(IEvolutionState state, int thread, GPData input, ADFStack stack, GPIndividual individual, IProblem problem)
        {
            ClassificationData result = (ClassificationData)input;
            result.boolVal = false;
        }

        public override string ToString()
        {
            return "FALSE";
        }
    }
}
