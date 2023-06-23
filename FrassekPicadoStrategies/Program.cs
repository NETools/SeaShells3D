using FrassekPicadoStrategies.Strategies;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace FrassekPicadoStrategies
{
    public class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new Game1())
                game.Run();
        }
    }
}
