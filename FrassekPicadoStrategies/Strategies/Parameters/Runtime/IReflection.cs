using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrassekPicadoStrategies.Strategies.Parameters.Runtime
{
    public interface IReflection
    {
        void SetValue(string name, dynamic value);
        dynamic GetValue(string name);
    }
}
