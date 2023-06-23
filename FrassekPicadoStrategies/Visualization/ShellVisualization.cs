using FrassekPicadoStrategies.Algorithms;
using FrassekPicadoStrategies.Strategies;
using FrassekPicadoStrategies.Strategies.Parameters.Runtime;
using FrassekPicadoStrategies.Visualization.Concurrency;
using FrassekPicadoStrategies.Visualization.Export;
using FrassekPicadoStrategies.Visualization.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FrassekPicadoStrategies.Visualization
{

    /// <summary>
    /// Diese Klasse benutzen wir, um die Muschel zu zeichnen. Wir benutzen Strategy-Pattern.
    /// </summary>
    public class ShellVisualization : IReflection, INotifyPropertyChanged
    {
        private static object _vertexBufferSync = new object();

        private VertexBuffer _vertexBuffer;
        private IndexBuffer _indexBuffer;

        private bool _indicesInitialized;

        private VertexPositionNormalTexture[] _verticesArray;
        private List<int> _indices = new List<int>();

        private BasicEffect _basicEffect;

        private IShellStrategy _currentStrategy;

        private double _maximumHelicoSamples = 100;
        private double _maximumEllipseSamples = 100;

        private double _thetaMin;
        private double _revolutions;

        private Floor _floor;

        private ConcurrentOperation _concurrentOperation = new ConcurrentOperation();

        private static RasterizerState _rasterizerState = new RasterizerState()
        {
            CullMode = CullMode.None,
            FillMode = FillMode.Solid
        };

        public GraphicsDevice GraphicsDevice { get; private set; }
        public GraphViewerCameraAdapter Camera { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public IShellStrategy ShellStrategy => _currentStrategy;

        public double MaximumHelicoSamples
        {
            get => _maximumHelicoSamples;
            set
            {
                _maximumHelicoSamples = value;
                GenerateVertices();
                GenerateIndices();

                CalculateNormals();

                GenerateVertexBuffer();
                GenerateIndexBuffer();

                PropertyChangedNotify(_maximumHelicoSamples);
            }
        }

        public double MaximumEllipseSamples // Wenn wir die Anzahl der Ellipsen-Samples (also wie detailiert die Ellipse sein soll), müssen wir sowohl IndexBuffer, als auch VertexBuffer aktualsiieren
        {
            get => _maximumEllipseSamples;
            set
            {
                _maximumEllipseSamples = value;
                GenerateVertices();
                GenerateIndices();

                CalculateNormals();

                GenerateVertexBuffer();
                GenerateIndexBuffer();
                PropertyChangedNotify(_maximumEllipseSamples);
            }
        }

        public double ThetaMin
        {
            get => _thetaMin;
            set
            {
                _thetaMin = value;
                GenerateVertices();
                GenerateIndices();

                CalculateNormals();

                GenerateVertexBuffer();
                GenerateIndexBuffer();
                PropertyChangedNotify(_thetaMin);
            }
        }
        public double Revolutions
        {
            get => _revolutions;
            set
            {
                _revolutions = value;
                GenerateVertices();
                GenerateIndices();

                CalculateNormals();

                GenerateVertexBuffer();
                GenerateIndexBuffer();
                PropertyChangedNotify(_revolutions);
            }
        }


        private float _rotationX;
        private Matrix _matRotationX;
        public float RotationX
        {
            get => _rotationX;
            set
            {
                _rotationX = value;
                _matRotationX = Matrix.CreateRotationX(MathHelper.ToRadians(_rotationX));
            }
        }

        private float _rotationY;
        private Matrix _matRotationY;
        public float RotationY
        {
            get => _rotationY;
            set
            {
                _rotationY = value;
                _matRotationY = Matrix.CreateRotationY(MathHelper.ToRadians(_rotationY));
            }
        }

        private float _rotationZ;
        private Matrix _matRotationZ;
        public float RotationZ
        {
            get => _rotationZ;
            set
            {
                _rotationZ = value;
                _matRotationZ = Matrix.CreateRotationZ(MathHelper.ToRadians(_rotationZ));
            }
        }

        private Vector3 _translation;
        private Matrix _matTranslation;

        public Vector3 Translation
        {
            get => _translation;
            set
            {
                _translation = value;
                _matTranslation = Matrix.CreateTranslation(_translation);
            }
        }

        public bool ShowFloor { get; set; }

        public Action ExportCommand => () =>
        {
            using(var sfd = new SaveFileDialog())
            {
                sfd.ShowDialog();
                if (string.IsNullOrEmpty(sfd.FileName)) return;
                File.WriteAllText(sfd.FileName, ObjPersistence.ConvertMeshToObj(
                    _vertexBuffer, 
                    _indexBuffer, 
                    _vertexBuffer.VertexDeclaration.VertexStride));

                MessageBox.Show("Model exportiert.", "Erfolgreich", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        };

        public Action SaveSettingsCommand => () =>
        {
            using (var sfd = new SaveFileDialog())
            {
                sfd.InitialDirectory = Path.Combine(Path.GetDirectoryName(Application.StartupPath), ShellStrategy.ToString());
                sfd.ShowDialog();
                if (string.IsNullOrEmpty(sfd.FileName)) return;

                while (Path.GetDirectoryName(sfd.FileName).ToLower() != sfd.InitialDirectory.ToLower())
                {
                    MessageBox.Show("Bitte speichern Sie die Einstellung in dem für diese Konfiguration vorgesehenen Ordner ab.", "Falscher Ordner", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    sfd.ShowDialog();
                }

                File.WriteAllText(sfd.FileName, SettingsSaver.GenerateIniString(this));

                MessageBox.Show("Einstellung gespeichert.", "Erfolgreich", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        };

        public ShellVisualization(GraphicsDevice graphicsDevice, GraphViewerCameraAdapter camera)
        {
            GraphicsDevice = graphicsDevice;
            Camera = camera;

            _basicEffect = new BasicEffect(GraphicsDevice);

            _matRotationX = _matRotationY = _matRotationZ = _matTranslation = Matrix.Identity;

            _floor = new Floor(GraphicsDevice, Camera, 1000, 1000, 5);
            _floor.LoadBuffers();
        }

        public void SetShellStrategy(IShellStrategy shellStrategy)
        {
            if (_currentStrategy != null)
            {
                _currentStrategy.PropertyChanged -= OnCurrentStrategyPropertyChanged;
            }
            _currentStrategy = shellStrategy;
            _currentStrategy.PropertyChanged += OnCurrentStrategyPropertyChanged;

            _concurrentOperation.Add(() =>
            {
                GenerateVertices();
                CalculateNormals();
                GenerateVertexBuffer();
            });
        }

        private void OnCurrentStrategyPropertyChanged(object value, System.ComponentModel.PropertyChangedEventArgs e)
        {
            PropertyChangedNotify(value,$"{ShellStrategy.ToString()}{e.PropertyName}");
            _concurrentOperation.Add(() =>
            {
                GenerateVertices();
                CalculateNormals();
                GenerateVertexBuffer();
            });
        }

        private void PropertyChangedNotify(object value, [CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(value, new PropertyChangedEventArgs(propertyName));
        }

        private void GenerateVertices()
        {
            List<VertexPositionNormalTexture> vertices = new List<VertexPositionNormalTexture>(1_000_000);
            var totalLengthHelico = Math.PI * 2.0 * Revolutions;
            var totalLengthEllipse = Math.PI * 2.0;

            var deltaTheta = totalLengthHelico / MaximumHelicoSamples;
            var deltaS = totalLengthEllipse / MaximumEllipseSamples;

            for (double theta = ThetaMin; theta < totalLengthHelico; theta += deltaTheta)
            {
                int ellipseIndex = 0;
                for (double s = 0; ellipseIndex < MaximumEllipseSamples && s < totalLengthEllipse; s += deltaS, ellipseIndex++)
                {
                    var x = ShellStrategy.GetX(theta, s);
                    var y = ShellStrategy.GetY(theta, s);
                    var z = ShellStrategy.GetZ(theta, s);
                    vertices.Add(new VertexPositionNormalTexture(new Vector3((float)x, (float)z, (float)y), Vector3.Zero, Vector2.Zero));
                }
            }

            _verticesArray = vertices.ToArray();
        }

        private void CalculateNormals()
        {
            for (int i = 0; i < _indices.Count; i += 6)
            {
                var a = _verticesArray[_indices[i]].Position;
                var b = _verticesArray[_indices[i + 1]].Position;
                var c = _verticesArray[_indices[i + 2]].Position;

                var d0 = a - b;
                var d1 = a - c;
                var n = Vector3.Cross(d0, d1);

                _verticesArray[_indices[i]].Normal -= n;
                _verticesArray[_indices[i + 1]].Normal -= n;
                _verticesArray[_indices[i + 2]].Normal -= n;
                _verticesArray[_indices[i + 3]].Normal -= n;
            }

            for (int i = 0; i < _verticesArray.Length; i++)
            {
                if (_verticesArray[i].Normal.LengthSquared() != 0)
                    _verticesArray[i].Normal.Normalize();
            }
        }

        private void GenerateIndices()
        {
            _indices = SurfaceGenerator.GenerateShellSurfaceTopology((int)MaximumEllipseSamples, _verticesArray.Length);
        }

        private void GenerateVertexBuffer()
        {
            if (_verticesArray.Length == 0)
                return;

            lock (_vertexBufferSync)
            {
                _vertexBuffer?.Dispose();
                _vertexBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionNormalTexture), _verticesArray.Length, BufferUsage.None);
                _vertexBuffer.SetData(_verticesArray);
            }
        }

        private void GenerateIndexBuffer()
        {
            if(_indices.Count == 0) return;

            _indexBuffer?.Dispose();
            _indexBuffer = new IndexBuffer(GraphicsDevice, IndexElementSize.ThirtyTwoBits, _indices.Count, BufferUsage.None);
            _indexBuffer.SetData<int>(_indices.ToArray());
        }

        public void Draw()
        {
            lock (_vertexBufferSync)
            {
                if (_vertexBuffer == null || _indexBuffer == null) return;

                GraphicsDevice.RasterizerState = _rasterizerState;

                GraphicsDevice.Indices = _indexBuffer;
                GraphicsDevice.SetVertexBuffer(_vertexBuffer);

                _basicEffect.View = Camera.ViewMatrix;
                _basicEffect.Projection = Camera.ProjectionMatrix;
                _basicEffect.World = _matRotationX * _matRotationY * _matRotationZ * _matTranslation * Matrix.CreateTranslation(-Camera.RotatedCameraPosition);

                _basicEffect.TextureEnabled = false;
                _basicEffect.EnableDefaultLighting();
                _basicEffect.DirectionalLight0.Enabled = true;
                _basicEffect.DirectionalLight0.SpecularColor = new Vector3(0.1f, 0.1f, 0.1f);
                _basicEffect.CurrentTechnique.Passes[0].Apply();

                GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, _indexBuffer.IndexCount / 2);

                if (ShowFloor)
                    _floor.Draw();
            }
        }

        public dynamic GetValue(string name)
        {
            return this.GetType().GetProperty(name).GetValue(this);
        }

        public void SetValue(string name, dynamic value)
        {
            this.GetType().GetProperty(name).SetValue(this, value);
        }
    }
}
