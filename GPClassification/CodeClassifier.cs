using GPClassification.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPClassification
{
    public static class CodeClassifier
    {
        private static string Classify(double[] cParams)
        {
            var P0 = cParams[0];
            var P1 = cParams[1];
            var P2 = cParams[2];
            var P3 = cParams[3];
            var P4 = cParams[4];
            var P5 = cParams[5];
            var P6 = cParams[6];
            var P7 = cParams[7];
            var P8 = cParams[8];
            var P9 = cParams[9];
            var P10 = cParams[10];

            if (P6 < 9)
            {
                if (P0 < 5)
                {
                    if (P3 < 41)
                    {
                        return "kciuk";
                    }
                    else
                    {
                        return "l1";
                    }
                }
                else
                {
                    if (P0 < 13)
                    {
                        return "reka";
                    }
                    else
                    {
                        return "piesc";
                    }
                }
            }
            else
            {
                if (P9 < 26)
                {
                    if (P2 > 49)
                    {
                        if (P7 > 11)
                        {
                            if (P4 > 2)
                            {
                                return "reka";
                            }
                            else
                            {
                                return "l2";
                            }
                        }
                        else
                        {
                            return "l3";
                        }
                    }
                    else
                    {
                        return "kciuk";
                    }
                }
                else
                {
                    if (P2 > 49)
                    {
                        if (P2 < 103)
                        {
                            return "l5";
                        }
                        else
                        {
                            return "l4";
                        }
                    }
                    else
                    {
                        return "l4";
                    }
                }
            }

        }

        public static double CheckClassifier(DataLoader dataset)
        {
            var correct = 0;

            for (var i = 0; i < dataset.InstanceCount; i++)
            {
                var cParams = new double[11];
                for (var j=0; j<11; j++)
                {
                    cParams[j] = dataset.Data[i, j];
                }

                var label = Classify(cParams);
                if (label == dataset.Labels[i])
                    correct++;
            }

            return (double)correct / dataset.InstanceCount;
        }
    }
}
