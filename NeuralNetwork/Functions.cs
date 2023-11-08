using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NNTestFrame.NeuralNetwork
{
    internal class Functions
    {
        //activation functions
        public static Func<double, double> Identity = (x) => x;
        public static Func<double, double> BinaryStep = (x) => x < 0.5 ? 0 : 1;
        public static Func<double, double> Sigmoid = (x) => 1 / (1 + Math.Exp(-x));
        public static Func<double, double> Hyperbolic = (x) => Math.Tanh(x);
        public static Func<double, double> ReLU = (x) => Math.Max(0d, x);
        public static Func<double, double> GELU = (x) => throw new NotImplementedException();
        public static Func<double, double> Softplus = (x) => Math.Log(1 + Math.Exp(x));
        public static Func<double, double> ELU = (x) => x > 0 ? x : Math.Exp(x) - 1;
        public static Func<double, double> SELU = (x) => 1.0507d * (x >= 0 ? x : 1.67326d * (Math.Exp(x) - 1));
        public static Func<double, double> LeakyReLU = (x) => x > 0 ? x : 0.01 * x;
        public static Func<double, double> ParametricReLU = (x) => throw new NotImplementedException();
        public static Func<double, double> SigmoidShrinkage = (x) => x / (1 + Math.Exp(-x));
        public static Func<double, double> Gaussian = (x) => Math.Exp(-(x * x));

        public static double NumericDerivative(Func<double, double> func, double x, double h = 1e-6d)
        {
            return (func.Invoke(x + h) - func.Invoke(x - h)) / (2 * h);
        }

        //boolean functions
        public static Func<double, double, double> b_zero = (x, y) => { return 0; };
        public static Func<double, double, double> b_and = (x, y) => { return x * y; };
        public static Func<double, double, double> b_ninduct = (x, y) => { return x * (1 - y); };
        public static Func<double, double, double> b_x = (x, y) => { return x; };
        public static Func<double, double, double> b_ndeduct = (x, y) => { return (1 - x) * y; };
        public static Func<double, double, double> b_y = (x, y) => { return y; };
        public static Func<double, double, double> b_xor = (x, y) => { return x + y - 2 * x * y; };
        public static Func<double, double, double> b_or = (x, y) => { return x + y - x * y; };
        public static Func<double, double, double> b_pirs = (x, y) => { return (1 - x) * (1 - y); };
        public static Func<double, double, double> b_nxor = (x, y) => { return 1 - x - y + 2 * x * y; };
        public static Func<double, double, double> b_ny = (x, y) => { return 1 - y; };
        public static Func<double, double, double> b_deduct = (x, y) => { return 1 - y + x * y; };
        public static Func<double, double, double> b_nx = (x, y) => { return 1 - x; };
        public static Func<double, double, double> b_induct = (x, y) => { return 1 - x + x * y; };
        public static Func<double, double, double> b_sheffer = (x, y) => { return 1 - x * y; };
        public static Func<double, double, double> b_one = (x, y) => { return 1; };
    }
}
