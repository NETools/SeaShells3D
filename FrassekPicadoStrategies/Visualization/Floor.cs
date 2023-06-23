using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrassekPicadoStrategies.Visualization
{
    internal class Floor
    {
        private VertexBuffer _vertexBuffer;
        private IndexBuffer _indexBuffer;

        private BasicEffect _basicEffect;

        public GraphicsDevice GraphicsDevice { get; private set; }
        public GraphViewerCameraAdapter CameraAdapter { get; private set; }

        public int Width { get; private set; }
        public int Depth { get; private set; }
        public double Size { get; private set; }



        public Floor(GraphicsDevice graphicsDevice, GraphViewerCameraAdapter cameraAdapter, int width, int depth, double size)
        {
            this.GraphicsDevice = graphicsDevice;
            this.CameraAdapter = cameraAdapter;

            this.Width = width;
            this.Depth = depth;
            this.Size = size;

            _basicEffect = new BasicEffect(GraphicsDevice);
        }

        public void LoadBuffers()
        {
            int countPerWidth = (int)Math.Ceiling((double)Width / Size) + 1;
            int countPerDepth = (int)Math.Ceiling((double)Depth / Size) + 1;

            VertexPositionColor[] vertices = new VertexPositionColor[2 * (countPerWidth + countPerDepth)];

            int index = 0;
            for (int i = 0; i < countPerWidth; i++)
            {
                vertices[index++] = new VertexPositionColor(new Vector3((float)(i * Size), 0, 0), Color.Gray);
                vertices[index++] = new VertexPositionColor(new Vector3((float)(i * Size), 0, Depth), Color.Gray);
            }

            for (int i = 0; i < countPerDepth; i++)
            {
                vertices[index++] = new VertexPositionColor(new Vector3(0, 0, (float)(i * Size)), Color.Gray);
                vertices[index++] = new VertexPositionColor(new Vector3(Width, 0, (float)(i * Size)), Color.Gray);
            }

            int[] indices = new int[vertices.Length];
            for (int i = 0; i < indices.Length; i++)
                indices[i] = i;

            _vertexBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionColor), vertices.Length, BufferUsage.WriteOnly);
            _indexBuffer = new IndexBuffer(GraphicsDevice, IndexElementSize.ThirtyTwoBits, indices.Length, BufferUsage.WriteOnly);

            _vertexBuffer.SetData<VertexPositionColor>(vertices);
            _indexBuffer.SetData<int>(indices);
        }

        public void Draw()
        {
            _basicEffect.View = CameraAdapter.ViewMatrix;
            _basicEffect.Projection = CameraAdapter.ProjectionMatrix;
            _basicEffect.World = Matrix.CreateTranslation(-Width * .5f, 0, -Depth * .5f) * Matrix.CreateTranslation(-CameraAdapter.RotatedCameraPosition);

            _basicEffect.VertexColorEnabled = true;
            _basicEffect.CurrentTechnique.Passes[0].Apply();

            GraphicsDevice.SetVertexBuffer(_vertexBuffer);
            GraphicsDevice.Indices = _indexBuffer;

            GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.LineList, 0, 0, _indexBuffer.IndexCount);
        }
    }
}
