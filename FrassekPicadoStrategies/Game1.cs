// - 1 -
using FrassekPicadoStrategies.Strategies;
using FrassekPicadoStrategies.Strategies.Parameters;
using FrassekPicadoStrategies.Strategies.Parameters.Runtime;
using FrassekPicadoStrategies.View;
using FrassekPicadoStrategies.Visualization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FrassekPicadoStrategies
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;

        private ShellVisualization _shellVisualization;
        private GraphViewerCameraAdapter _camera;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.GraphicsProfile = GraphicsProfile.HiDef;
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 800;
            _graphics.PreferMultiSampling = true;
            _graphics.SynchronizeWithVerticalRetrace = true;

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        private ParameterSettings _parameterSettings;

        protected override void LoadContent()
        {
            // 2.
            _camera = new GraphViewerCameraAdapter(GraphicsDevice, 0.001f, 1, GraphicsDevice.Viewport.AspectRatio);
            _shellVisualization = new ShellVisualization(GraphicsDevice, _camera);


            _parameterSettings = new ParameterSettings(_shellVisualization);
            _parameterSettings.Show();
        }

        private int _prevMouseScrollValue;
        protected override void Update(GameTime gameTime)
        {

            if (!this.IsActive) return;

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            // 3.

            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                _camera.BeginRotation();
                _camera.Rotate(gameTime.ElapsedGameTime.TotalMilliseconds * 0.5f);
            }
            if (Mouse.GetState().LeftButton == ButtonState.Released)
            {
                _camera.EndRotation();
            }

            if (Mouse.GetState().RightButton == ButtonState.Pressed)
            {
                _camera.BeginTranslation();
                _camera.Translate(gameTime.ElapsedGameTime.TotalMilliseconds * 0.05);
            }
            if (Mouse.GetState().RightButton == ButtonState.Released)
            {
                _camera.EndTranslation();
            }

            if (Mouse.GetState().ScrollWheelValue > _prevMouseScrollValue)
            {
                _camera.Scroll((float)(1 * gameTime.ElapsedGameTime.TotalMilliseconds));
            }

            if (Mouse.GetState().ScrollWheelValue < _prevMouseScrollValue)
            {
                _camera.Scroll((float)(-1 * gameTime.ElapsedGameTime.TotalMilliseconds));
            }

            _prevMouseScrollValue = Mouse.GetState().ScrollWheelValue;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(30, 30, 34));

            _shellVisualization.Draw();

            base.Draw(gameTime);
        }



    }
}