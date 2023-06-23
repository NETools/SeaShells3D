using FrassekPicadoStrategies.Strategies.Parameters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrassekPicadoStrategies.Strategies
{
    public interface IShellStrategy : INotifyPropertyChanged
    {
        public dynamic Parameters { get; }
        double GetX(double theta, double s);
        double GetY(double theta, double s);
        double GetZ(double theta, double s);
    }
}
