using FrassekPicadoStrategies.Strategies.Parameters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using static System.Math;

namespace FrassekPicadoStrategies.Strategies
{
    public class Picado : IShellStrategy
    {
        public event PropertyChangedEventHandler PropertyChanged;
            
        private PicadoParameters _parameters = new PicadoParameters();

        public dynamic Parameters => _parameters;
        public Picado()
        {
            _parameters.PropertyChanged += ParameterChanged;
        }

        private void ParameterChanged(object value, PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(value, new PropertyChangedEventArgs(e.PropertyName));
        }

        public double GetX(double theta, double s)
        {
            return _parameters.D * (_parameters.A * Sin(_parameters.Beta) * Cos(theta) + Cos(s + _parameters.Phi) * Cos(theta + _parameters.Omega) * r_e(s, theta)
             - Sin(_parameters.Mu) * Sin(s + _parameters.Phi) * Sin(theta + _parameters.Omega) * r_e(s, theta)) * Exp(theta * cot(_parameters.Alpha));
        }
        public double GetY(double theta, double s)
        {
            return (_parameters.A * Sin(_parameters.Beta) * Sin(theta) + Cos(s + _parameters.Phi) * Sin(theta + _parameters.Omega) * r_e(s, theta) + Sin(_parameters.Mu) * Sin(s + _parameters.Phi) * Cos(theta + _parameters.Omega) * r_e(s, theta)) * Exp(theta * cot(_parameters.Alpha));
        }

        public double GetZ(double theta, double s)
        {
            return (-_parameters.A * Cos(_parameters.Beta) + Cos(_parameters.Mu) * Sin(s + _parameters.Phi) * r_e(s, theta)) * Exp(theta * cot(_parameters.Alpha));
        }

        private double cot(double v)
        {
            return Cos(v) / Sin(v);
        }
        double r_e(double s, double theta)
        {
            double _a = Cos(s) / _parameters.MajorAxis;
            double _b = Sin(s) / _parameters.MinorAxis;

            return 1.0 / Sqrt(_a * _a + _b * _b) + r_n(s, theta);
        }
        double r_n(double s, double theta)
        {
            double a = (2 * (s - _parameters.P)) / _parameters.W1;
            double b = (2 * l(theta)) / _parameters.W2;

            return _parameters.L * Exp(-(a * a + b * b));
        }
        double l(double theta)
        {
            return (2.0 * Math.PI / _parameters.N) * ((_parameters.N * theta / (2.0 * Math.PI)) - (int)(_parameters.N * theta / (2.0 * Math.PI)));
        }

        public override string ToString()
        {
            return "Picado";
        }
    }
}