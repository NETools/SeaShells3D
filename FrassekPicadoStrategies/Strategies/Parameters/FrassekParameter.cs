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
    public class FrassekParameters : INotifyPropertyChanged, IReflection
    {
        private double _D, _m, _w, _r, _d, _b, _c, _l, _L, _q, _Q;
        public double D { get => _D; set { if (_D == value) return; _D = value; OnChanged(_D); } }
        public double M { get => _m; set { if (_m == value) return; _m = value; OnChanged(_m); } }
        public double W { get => _w; set { if (_w == value) return; _w = value; OnChanged(_w); } }
        public double R { get => _r; set { if (_r == value) return; _r = value; OnChanged(_r); } }
        public double D1 { get => _d; set { if (_d == value) return; _d = value; OnChanged(_d); } }
        public double B { get => _b; set { if (_b == value) return; _b = value; OnChanged(_b); } }
        public double C { get => _c; set { if (_c == value) return; _c = value; OnChanged(_c); } }
        public double L { get => _l; set { if (_l == value) return; _l = value; OnChanged(_l); } }
        public double L1 { get => _L; set { if (_L == value) return; _L = value; OnChanged(_L); } }
        public double Q { get => _q; set { if (_q == value) return; _q = value; OnChanged(_q); } }
        public double Q1 { get => _Q; set { if (_Q == value) return; _Q = value; OnChanged(_Q); } }

        public event PropertyChangedEventHandler PropertyChanged;

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
