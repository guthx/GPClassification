using System;
using System.Collections.Generic;
using System.Text;
using BraneCloud.Evolution.EC.Configuration;
using BraneCloud.Evolution.EC.GP;
namespace GPClassification.Data
{
    [ECConfiguration("ec.app.GPClassification.ClassificationData")]
    public class ClassificationData : GPData
    {
        public double doubleVal;
        public bool boolVal;
        public string stringVal = "";
        public int index;
        public override void CopyTo(GPData gpd)
        {
            ((ClassificationData)gpd).doubleVal = doubleVal;
            ((ClassificationData)gpd).boolVal = boolVal;
            ((ClassificationData)gpd).stringVal = stringVal;
            ((ClassificationData)gpd).index = index;
        }
    }
}
