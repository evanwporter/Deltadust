using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Deltadust.Core {
    public class Camera
    {
        private readonly Viewport _viewport;
        private Vector2 _position;

        public Camera(Viewport viewport)
        {
            _viewport = viewport;
            Zoom = 1.0f;
        }

        public Vector2 Position
        {
            get => _position;
            set
            {
                _position = value;
                ClampPosition();
            }
        }

        public float Zoom { get; set; }

        public Matrix GetViewMatrix()
        {
            return Matrix.CreateTranslation(new Vector3(-_position, 0.0f)) *
                Matrix.CreateScale(Zoom, Zoom, 1.0f);
        }

        private void ClampPosition()
        {
            var cameraMax = new Vector2(_viewport.Width, _viewport.Height) / Zoom;
            _position = Vector2.Clamp(_position, Vector2.Zero, cameraMax);
        }
    }
}
