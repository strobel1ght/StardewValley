using Microsoft.Xna.Framework.Graphics;

namespace ConvenientChests.CategorizeChests.Interface.Widgets
{
    /// <summary>
    /// A simple non-interactive sprite.
    /// </summary>
    class Stamp : Widget
    {
        private readonly TextureRegion _textureRegion;

        public Stamp(TextureRegion textureRegion)
        {
            _textureRegion = textureRegion;
            Width = _textureRegion.Width;
            Height = _textureRegion.Height;
        }

        public override void Draw(SpriteBatch batch)
        {
            batch.Draw(_textureRegion.Texture, _textureRegion.Region, GlobalPosition.X, GlobalPosition.Y,
                _textureRegion.Width, _textureRegion.Height);
        }
    }
}