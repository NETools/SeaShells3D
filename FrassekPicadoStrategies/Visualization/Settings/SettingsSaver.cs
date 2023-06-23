using FrassekPicadoStrategies.Strategies.Parameters.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FrassekPicadoStrategies.Visualization.Settings
{
    internal class SettingsSaver
    {
        private static HashSet<string> _legalShellVisualizationProps = new HashSet<string>()
        {
            "ThetaMin",
            "Revolutions"
        };

        private static Dictionary<string, Func<double, double>> _conversionRequiredShellVisualizationProps = new Dictionary<string, Func<double, double>>()
        {
            { "Alpha", ToDeg },
            { "Beta", ToDeg },
            { "Mu", ToDeg },
            { "Phi", ToDeg },
            { "Omega", ToDeg },

        };

        private static double ToDeg(double value)
        {
            return value * 180.0 / Math.PI;
        }

        public static string GenerateIniString(ShellVisualization shellVisualization)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("[Shell-Parameters]");

            IReflection param = shellVisualization.ShellStrategy.Parameters;

            foreach (var prop in param.GetType().GetProperties())
            {
                if (_conversionRequiredShellVisualizationProps.ContainsKey(prop.Name))
                    sb.AppendLine($"{prop.Name} = {_conversionRequiredShellVisualizationProps[prop.Name](param.GetValue(prop.Name))}");
                else
                    sb.AppendLine($"{prop.Name} = {param.GetValue(prop.Name)}");
            }

            sb.AppendLine("[Renderer-Parameters]");

            foreach (var prop in shellVisualization.GetType().GetProperties())
                if (_legalShellVisualizationProps.Contains(prop.Name))
                    sb.AppendLine($"{prop.Name} = {shellVisualization.GetValue(prop.Name)}");

            return sb.ToString();
        }
    }
}
