using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NNTestFrame.NeuralNetwork
{
    internal class ElementaryPerceptron
    {

        Func<double, double> activationFunction;

        double[][][] weights;
        int inputsCount;
        int exitsCount;
        double[][] netResult;
        double[][] lastResult;
        double[][][] lastDelta;

        public double[][] LastResult { get => lastResult; }

        public ElementaryPerceptron(int inputs, int hidenLayers, int[] hidenLayersConfiguration, int exits)
        {
            //check hidenLayersConfiguration.Count == hidenLayers
            activationFunction = Functions.Sigmoid;
            inputsCount = inputs;
            exitsCount = exits;
            weights = new double[hidenLayers + 1][][];
            lastDelta = new double[hidenLayers + 1][][];
            weights[0] = new double[inputs + 1][];
            lastDelta[0] = new double[inputs + 1][];
            for (int i = 1; i < hidenLayers + 1; i++)
            {
                weights[i] = new double[hidenLayersConfiguration[i - 1] + 1][];
                lastDelta[i] = new double[hidenLayersConfiguration[i - 1] + 1][];
                for (int j = 0; j < weights[i - 1].GetLength(0); j++)
                {
                    weights[i - 1][j] = new double[hidenLayersConfiguration[i - 1]];
                    lastDelta[i - 1][j] = new double[hidenLayersConfiguration[i - 1]];
                }
            }

            for (int j = 0; j < weights[hidenLayers].GetLength(0); j++)
            {
                weights[hidenLayers][j] = new double[exits];
                lastDelta[hidenLayers][j] = new double[exits];
            }
        }

        public ElementaryPerceptron(int inputs, int hidenLayers, int[] hidenLayersConfiguration, int exits, Func<double, double> activationFunction) : this(inputs, hidenLayers, hidenLayersConfiguration, exits)
        {
            this.activationFunction = activationFunction;
        }

        public void RandomizeWeights(Random random)
        {
            for (int i = 0; i < weights.GetLength(0); i++)
            {
                for (int j = 0; j < weights[i].GetLength(0); j++)
                {
                    for (int k = 0; k < weights[i][j].GetLength(0); k++)
                    {
                        weights[i][j][k] = random.NextDouble() - 0.5d;
                    }
                }
            }
        }

        public double[] Evaluate(double[] input)
        {
            //input.length == inputCount
            var net = new double[weights.GetLength(0) + 1][];
            var oj = new double[weights.GetLength(0) + 1][];
            net[0] = input.Append(1d).ToArray();
            oj[0] = new double[input.Length + 1];
            for (int j = 0; j < net[0].GetLength(0); j++)
            {
                oj[0][j] = net[0][j];
            }

            for (int i = 1; i < weights.GetLength(0); i++)
            {
                net[i] = new double[weights[i].GetLength(0)];
                oj[i] = new double[weights[i].GetLength(0)];
                for (int j = 0; j < weights[i].GetLength(0); j++)
                {
                    if (weights[i].GetLength(0) - 1 == j)
                    {
                        net[i][j] = 1;
                        oj[i][j] = 1;
                        continue;
                    }

                    net[i][j] = 0;
                    for (int k = 0; k < weights[i - 1].GetLength(0); k++)
                    {
                        net[i][j] += oj[i - 1][k] * weights[i - 1][k][j];
                    }
                    oj[i][j] = activationFunction(net[i][j]);
                }
            }

            net[net.Length - 1] = new double[exitsCount];
            oj[oj.Length - 1] = new double[exitsCount];
            for (int j = 0; j < exitsCount; j++)
            {
                net[net.Length - 1][j] = 0;
                for (int k = 0; k < weights[net.Length - 2].GetLength(0); k++)
                {
                    net[net.Length - 1][j] += oj[net.Length - 2][k] * weights[net.Length - 2][k][j];
                }
                oj[net.Length - 1][j] = activationFunction(net[net.Length - 1][j]);
            }
            netResult = net;
            lastResult = oj;
            return oj[net.Length - 1];
        }

        private double[][] ErrorEvaluate(double[] trueResult)
        {
            var sigm = new double[weights.GetLength(0) + 1][];

            sigm[weights.GetLength(0)] = new double[exitsCount];
            for (int j = 0; j < exitsCount; j++)
            {
                //sigm[sigm.Length - 1][j] = (trueResult[j] - lastResult[sigm.Length - 1][j]) * lastResult[sigm.Length - 1][j] * (1 - lastResult[sigm.Length - 1][j]);
                sigm[sigm.Length - 1][j] = (trueResult[j] - lastResult[sigm.Length - 1][j]) * Functions.NumericDerivative(activationFunction, netResult[sigm.Length - 1][j]);
            }

            for (int i = weights.GetLength(0) - 1; i >= 0; i--)
            {
                sigm[i] = new double[weights[i].GetLength(0)];
                for (int j = 0; j < weights[i].GetLength(0); j++)
                {
                    if (weights[i].GetLength(0) - 1 == j)
                    {
                        sigm[i][j] = 0;
                        continue;
                    }

                    var temp = 0d;
                    for (int k = 0; k < sigm[i + 1].GetLength(0); k++)
                    {
                        if (k == weights[i][j].GetLength(0))
                        {
                            continue;
                        }
                        temp += sigm[i + 1][k] * weights[i][j][k];
                    }
                    //sigm[i][j] = lastResult[i][j] * (1 - lastResult[i][j]) * temp;
                    sigm[i][j] = Functions.NumericDerivative(activationFunction, netResult[i][j]) * temp;
                }
            }
            return sigm;
        }

        public void ErrorCorrection(double[] trueResult, double correctionHardness)
        {
            var sigm = ErrorEvaluate(trueResult);
            for (int i = 0; i < weights.GetLength(0); i++)
            {
                for (int j = 0; j < weights[i].GetLength(0); j++)
                {
                    if (weights[i].GetLength(0) - 1 == j)
                    {
                        continue;
                    }
                    for (int k = 0; k < weights[i][j].GetLength(0); k++)
                    {
                        weights[i][j][k] += correctionHardness * sigm[i + 1][k] * lastResult[i][j];
                    }
                }
            }
        }

        public void ErrorCorrectionAdaptive(double[] trueResult)
        {
            var sigm = ErrorEvaluate(trueResult);

            double correctionHardness = 0.01;
            for (int j = 0; j < exitsCount; j++)
            {
                correctionHardness *= Math.Abs(trueResult[j] - lastResult[sigm.Length - 1][j]) * 10;
            }

            for (int i = 0; i < weights.GetLength(0); i++)
            {
                for (int j = 0; j < weights[i].GetLength(0); j++)
                {
                    if (weights[i].GetLength(0) - 1 == j)
                    {
                        continue;
                    }
                    for (int k = 0; k < weights[i][j].GetLength(0); k++)
                    {
                        weights[i][j][k] += correctionHardness * sigm[i + 1][k] * lastResult[i][j];
                    }
                }
            }
        }

        public void ErrorCorrectionImpulse(double[] trueResult, double correctionHardness, double impulse)
        {
            var sigm = ErrorEvaluate(trueResult);
            for (int i = 0; i < weights.GetLength(0); i++)
            {
                for (int j = 0; j < weights[i].GetLength(0); j++)
                {
                    if (weights[i].GetLength(0) - 1 == j)
                    {
                        continue;
                    }
                    for (int k = 0; k < weights[i][j].GetLength(0); k++)
                    {
                        var temp = correctionHardness * sigm[i + 1][k] * lastResult[i][j];
                        weights[i][j][k] += temp + impulse * lastDelta[i][j][k];
                        lastDelta[i][j][k] = temp;
                    }
                }
            }
        }

        public void ErrorCorrectionAdaptiveImpulse(double[] trueResult, double impulse)
        {
            var sigm = ErrorEvaluate(trueResult);
            double correctionHardness = 0.01;
            for (int j = 0; j < exitsCount; j++)
            {
                correctionHardness *= Math.Abs(trueResult[j] - lastResult[sigm.Length - 1][j]) * 10;
            }
            for (int i = 0; i < weights.GetLength(0); i++)
            {
                for (int j = 0; j < weights[i].GetLength(0); j++)
                {
                    if (weights[i].GetLength(0) - 1 == j)
                    {
                        continue;
                    }
                    for (int k = 0; k < weights[i][j].GetLength(0); k++)
                    {
                        var temp = correctionHardness * sigm[i + 1][k] * lastResult[i][j];
                        weights[i][j][k] += temp + impulse * lastDelta[i][j][k];
                        lastDelta[i][j][k] = temp;
                    }
                }
            }
        }

        public void Train(double[] input, double[] trueResult, double correctionHardness)
        {
            Evaluate(input);
            ErrorCorrection(trueResult, correctionHardness);
            //ErrorCorrectionAdaptive(trueResult);
            //ErrorCorrectionImpulse(trueResult, correctionHardness, 10);
            //ErrorCorrectionAdaptiveImpulse(trueResult, correctionHardness);
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < weights.GetLength(0); i++)
            {
                //stringBuilder.AppendLine("Layer number: " + i);
                for (int j = 0; j < weights[i].GetLength(0); j++)
                {
                    //stringBuilder.AppendLine("Element from: " + j);
                    for (int k = 0; k < weights[i][j].GetLength(0); k++)
                    {
                        //stringBuilder.AppendLine("Element to: " + k);
                        stringBuilder.AppendLine("Element layer, from, to: " + i + ", " + j + ", " + k);
                        stringBuilder.AppendLine("Weight equals: " + weights[i][j][k]);
                    }
                }
            }
            return stringBuilder.ToString();
        }
    }
}
