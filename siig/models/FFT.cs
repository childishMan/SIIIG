using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace siig.models
{
    class FFT
    {
        public static List<complex> ForwardFourierTransform(List<complex> Signal)
        {
            if (Signal.Count == 1)
            {
                return Signal;
            }

            var N = Signal.Count;

            var Even = Signal.Where((item, index) => index % 2 == 0).ToList();
            var Odd = Signal.Where((item, index) => index % 2 != 0).ToList();

            var yEven = ForwardFourierTransform(Even);
            var yOdd = ForwardFourierTransform(Odd);


            var Y = new List<complex>();

            for (int i = 0; i < N; i++)
            {
                Y.Add(new complex(0, 0));
            }

            var W = new List<complex>();

            for (int i = 0; i < N; i++)
            {
                double alpha = -2 * Math.PI * i / N;
                W.Add(ComplexConverter.FromRadians(1, alpha));
            }

            for (int i = 0; i < N / 2; i++)
            {


                Y[i] = yEven[i] + W[i] * yOdd[i];
                Y[i + N / 2] = yEven[i] - W[i] * yOdd[i];
            }

            return Y;
        }

        public static List<complex> InverseFourierTransform(List<complex> Signal)
        {

            if (Signal.Count == 1)
            {
                return Signal;
            }

            var N = Signal.Count;

            var Even = Signal.Where((item, index) => index % 2 == 0).ToList();
            var Odd = Signal.Where((item, index) => index % 2 != 0).ToList();

            var yEven = ForwardFourierTransform(Even);
            var yOdd = ForwardFourierTransform(Odd);


            var Y = new List<complex>();

            for (int i = 0; i < N; i++)
            {
                Y.Add(new complex(0, 0));
            }

            var W = new List<complex>();

            for (int i = 0; i < N; i++)
            {
                double alpha = 2 * Math.PI * i / N;
                W.Add(ComplexConverter.FromRadians(1, alpha));
            }

            for (int i = 0; i < N / 2; i++)
            {

                Y[i] = (yEven[i] + W[i] * yOdd[i]) / N;
                Y[i + N / 2] = (yEven[i] - W[i] * yOdd[i]) / N;
            }

            return Y;
        }
    }

    public class complex
    {
        private double real = 0.0;
        private double img = 0.0;

        public double Magnitude
        {
            get
            {
                return Math.Sqrt(Math.Pow(real, 2) + Math.Pow(img, 2));
            }
        }

        public double Phase
        {
            get
            {
                var vall = Math.Atan(img / real)*(180/Math.PI);
                if (Double.IsNaN(vall))
                {
                    vall = 0;
                }
                return vall;
            }
        }

        public complex(double real, double img)
        {
            this.real = real;
            this.img = img;
        }

        public complex(double real)
        {
            this.real = real;
            this.img = 0;
        }

        public static complex operator *(complex a, complex b)
        {
            double rl = (a.img * b.img) * -1 + a.real * b.real;
            double im = (a.real * b.img) + (a.img * b.real);

            return new complex(rl, im);
        }

        public static complex operator *(int a, complex b)
        {
            return new complex(b.real * a, b.img * a);
        }

        public static complex operator +(complex a, complex b)
        {
            return new complex(a.real + b.real, a.img + b.img);
        }

        public static complex operator -(complex a, complex b)
        {
            return new complex(a.real - b.real, a.img - b.img);
        }

        public static complex operator /(complex a, int b)
        {
            return new complex(a.real / b, a.img / b);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"{real:F2}");
            if (img != 0)
            {
                if (img > 0)
                    sb.Append(" + ");
                else if (img < 0)
                    sb.Append(" - ");
                sb.Append($"{Math.Abs(img):F2}");
                sb.Append("i");
            }

            sb.Append($" M:{Magnitude:F2} P:{Phase:F2}");
            return sb.ToString();
        }
    }

    public class ComplexConverter
    {
        public static complex FromRadians(double r, double rad)
        {
            return new complex(r * Math.Cos(rad), r * Math.Sin(rad));
        }

        public static List<complex> FromList(List<double> list)
        {
            var ListOFComplex = new List<complex>();

            foreach (var item in list)
            {
                ListOFComplex.Add(new complex(item, 0));
            }

            return ListOFComplex;
        }
    }
}
