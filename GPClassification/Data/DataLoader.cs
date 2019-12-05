using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace GPClassification.Data
{
    public class DataLoader
    {
        public double[,] Data;
        public string[] Labels;
        public int AttributeCount;
        public int InstanceCount;
        public double MaxValue;
        public double MinValue;
        public double[] MaxValues;
        public double[] MinValues;
        public int ClassCount;
        public List<string> Classes;

        public DataLoader(string fileName, int attributeCount, int instanceCount)
        {
            AttributeCount = attributeCount;
            InstanceCount = instanceCount;
            var reader = new StreamReader(@fileName);
            //  var lineCount = File.ReadLines(@fileName).Count();
            Data = new double[InstanceCount, AttributeCount];
            Labels = new string[InstanceCount];
            int i = 0;
            while (!reader.EndOfStream && i < InstanceCount)
            {
                var line = reader.ReadLine();
                var values = line.Split(',');
                int j;
                for (j = 0; j < AttributeCount; j++)
                {
                    Data[i, j] = double.Parse(values[j], CultureInfo.InvariantCulture);
                }
                Labels[i] = (values[values.Length - 1]);
                i++;
            }
        }
        public DataLoader(string fileName, int attributeCount, int instanceCount, int firstRow = 0, int firstCol = 0, char separator = ',')
        {
            AttributeCount = attributeCount;
            InstanceCount = instanceCount;
            MaxValue = double.MinValue;
            MinValue = double.MaxValue;
            Classes = new List<string>();
            var reader = new StreamReader(@fileName);
            for (var x = 0; x < firstRow; x++)
            {
                reader.ReadLine();
            }
            //  var lineCount = File.ReadLines(@fileName).Count();
            Data = new double[InstanceCount, AttributeCount];
            Labels = new string[InstanceCount];
            int i = 0;
            while (!reader.EndOfStream && i < InstanceCount)
            {
                var line = reader.ReadLine();
                var values = line.Split(separator);
                int j;
                for (j = 0; j < AttributeCount; j++)
                {
                    var value = double.Parse(values[j + firstCol], CultureInfo.InvariantCulture);
                    Data[i, j] = value;
                    if (MaxValue < Data[i, j])
                        MaxValue = Data[i, j];
                    if (MinValue > Data[i, j])
                        MinValue = Data[i, j];
                }
                Labels[i] = (values[values.Length - 1]);
                if (!Classes.Contains(Labels[i]))
                {
                    Classes.Add(Labels[i]);
                }
                i++;
            }
            ClassCount = Classes.Count();
            // Normalize(0, 100);
            FindMinMax();
        }

        private void FindMinMax()
        {
            MaxValues = new double[AttributeCount];
            MinValues = new double[AttributeCount];
            for(var i=0; i<AttributeCount; i++)
            {
                MaxValues[i] = double.MinValue;
                MinValues[i] = double.MaxValue;
            }
            for(var i=0; i<InstanceCount; i++)
                for(var j=0; j<AttributeCount; j++)
                {
                    var val = Data[i, j];
                    if(true /*val != -1*/)
                    {
                        if (val < MinValues[j])
                            MinValues[j] = val;
                        if (val > MaxValues[j])
                            MaxValues[j] = val;
                    }
                }
        }

        private void Normalize(int min, int max)
        {
            var minValues = new double[AttributeCount];
            var maxValues = new double[AttributeCount];
            var meanValues = new double[AttributeCount];
            var sums = new double[AttributeCount];
            var correctInstances = new double[AttributeCount];
            for (var i = 0; i < AttributeCount; i++)
            {
                minValues[i] = double.MaxValue;
                maxValues[i] = double.MinValue;
            }
            for (var i = 0; i < InstanceCount; i++)
            {
                for (var j = 0; j < AttributeCount; j++)
                {
                    var value = Data[i, j];
                    if (Data[i, j] != -1)
                    {
                        if (value < minValues[j])
                            minValues[j] = value;
                        if (value > maxValues[j])
                            maxValues[j] = value;
                    }
                    
                }
            }

            for (var i = 0; i < AttributeCount; i++)
            {
                for (var j = 0; j < InstanceCount; j++)
                {
                    if (Data[j, i] != -1)
                    {
                        Data[j, i] = (((Data[j, i] - minValues[i]) / (maxValues[i] - minValues[i])) * (max - min)) + min;
                        sums[i] += Data[j, i];
                        correctInstances[i]++;
                    }
                         
                }
            }
            for(var i = 0; i< AttributeCount; i++)
            {
                meanValues[i] = sums[i] / correctInstances[i];
            }
            for(var i=0; i<AttributeCount; i++)
                for(var j=0; j<InstanceCount; j++)
                {
                    if (Data[j, i] == -1)
                        Data[j, i] = meanValues[i];
                }

            MaxValue = max;
            MinValue = min;
        }
    }
}
