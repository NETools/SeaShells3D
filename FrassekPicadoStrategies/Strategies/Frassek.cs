using FrassekPicadoStrategies.Strategies.Parameters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using static System.Math;

namespace FrassekPicadoStrategies.Strategies
{
    public class Frassek : IShellStrategy
    {
        private FrassekParameters _parameters = new FrassekParameters();
        public dynamic Parameters => _parameters;

        public event PropertyChangedEventHandler PropertyChanged;

        public Frassek()
        {
            _parameters.PropertyChanged += ParameterChanged;
        }

        private void ParameterChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(sender, new PropertyChangedEventArgs(e.PropertyName));
        }

        public double GetX(double theta, double s)
        {
            return _parameters.D * _parameters.M * Exp(theta * _parameters.W) * Cos(theta) * (_parameters.R * Cos(s) + _parameters.D1 + _parameters.Q * Sin(_parameters.Q1 * s));
        }
        public double GetY(double theta, double s)
        {
            return _parameters.M * Exp(theta * _parameters.W) * Sin(theta) * (_parameters.R * Cos(s) + _parameters.D1 + _parameters.Q * Sin(_parameters.Q1 * s));
        }
        public double GetZ(double theta, double s)
        {
            return -_parameters.M * Exp(theta * _parameters.W) * (_parameters.R * _parameters.B * Sin(s) + _parameters.C + _parameters.L * Sin(_parameters.L1 * theta));
        }

        public override string ToString()
        {
            return "Frassek";
        }

    }
}
