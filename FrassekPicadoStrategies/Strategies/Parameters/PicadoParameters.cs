using FrassekPicadoStrategies.Strategies.Parameters.Runtime;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FrassekPicadoStrategies.Strategies.Parameters
{
    public class PicadoParameters : INotifyPropertyChanged, IReflection
    {
        private double _D, _alpha, _beta, _A, _minorAxis, _majorAxis, _phi, _mu, _omega, _N, _L, _P, _W1, _W2;

        public event PropertyChangedEventHandler PropertyChanged;
        public double D { get => _D; set { if (_D == value) return; _D = value; OnChanged(_D); } }
        public double Alpha { get => _alpha; set { if (_alpha == value) return;  _alpha = value * Math.PI / 180.0; OnChanged(value); } }
        public double Beta { get => _beta; set { if (_beta == value) return; _beta = value * Math.PI / 180.0; OnChanged(value); } }
        public double A { get => _A; set { if (_A == value) return; _A = value; OnChanged(_A); } }
        public double MinorAxis { get => _minorAxis; set { if (_minorAxis == value) return; _minorAxis = value; OnChanged(_minorAxis); } }
        public double MajorAxis { get => _majorAxis; set { if (_majorAxis == value) return; _majorAxis = value; OnChanged(_majorAxis); } }
        public double Phi { get => _phi; set { if (_phi == value) return; _phi = value * Math.PI / 180.0; OnChanged(value); } }
        public double Mu { get => _mu; set { if (_mu == value) return; _mu = value * Math.PI / 180.0; OnChanged(value); } }
        public double Omega { get => _omega; set { if (_omega == value) return; _omega = value * Math.PI / 180.0 ; OnChanged(value); } }
        public double N { get => _N; set { if (_N == value) return; _N = value; OnChanged(_N); } }
        public double L { get => _L; set { if (_L == value) return; _L = value; OnChanged(_L); } }
        public double P { get => _P; set { if (_P == value) return; _P = value; OnChanged(_P); } }
        public double W1 { get => _W1; set { if (_W1 == value) return; _W1 = value; OnChanged(_W1); } }
        public double W2 { get => _W2; set { if (_W2 == value) return; _W2 = value; OnChanged(_W2); } }

        public dynamic GetValue(string name)
        {
            return this.GetType().GetProperty(name).GetValue(this);
        }

        public void SetValue(string name, dynamic value)
        {
            this.GetType().GetProperty(name).SetValue(this, value);
        }
        private void OnChanged(object value, [CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(value, new PropertyChangedEventArgs(propertyName));
        }
    }
}
