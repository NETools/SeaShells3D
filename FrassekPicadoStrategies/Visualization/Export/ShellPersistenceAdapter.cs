using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrassekPicadoStrategies.Visualization.Export
{
    public class ObjPersistence
    {
        public static string ConvertMeshToObj(VertexBuffer vertexBuffer, IndexBuffer indexBuffer, int stride)
        {
            StringBuilder formatBuilder = new StringBuilder();

            int numVertices = vertexBuffer.VertexCount;

            int step = stride / sizeof(float);
            float[] data = new float[numVertices * step];
            vertexBuffer.GetData(data);

            for (int i = 0; i < data.Length; i += step)
            {
                formatBuilder.AppendLine($"v {data[i + 0]} {data[i + 1]} {data[i + 2]}".Replace(",", "."));
            }

            formatBuilder.AppendLine();


            for (int i = 0; i < data.Length; i += step)
            {
                formatBuilder.AppendLine($"vn {data[i + 3]} {data[i + 4]} {data[i + 5]}".Replace(",", "."));
            }

            formatBuilder.AppendLine();

            int numIndices = indexBuffer.IndexCount;

            int[] indices = new int[numIndices];
            indexBuffer.GetData(indices);

            for (int i = 0; i < indices.Length; i += 6)
            {
                formatBuilder.AppendLine($"f {indices[i + 0] + 1}//{indices[i + 0] + 1} {indices[i + 1] + 1}//{indices[i + 1] + 1} {indices[i + 2] + 1}//{indices[i + 2] + 1} {indices[i + 3] + 1}//{indices[i + 3] + 1}");
            }

            return formatBuilder.ToString();
        }
    }
}
