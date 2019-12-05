using BraneCloud.Evolution.EC.GP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPClassification.Data
{
    public class LabeledClassifier
    {
        public GPIndividual Classifier { get; set; }
        public string Label { get; set; }
    }
}
