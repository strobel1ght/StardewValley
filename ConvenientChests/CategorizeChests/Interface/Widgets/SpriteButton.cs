using Microsoft.Xna.Framework.Graphics;

namespace ConvenientChests.CategorizeChests.Interface.Widgets {
    /// <summary>
    /// A button that uses a single TextureRegion to display itself.
    /// </summary>
    class SpriteButton : Button {
        private readonly TextureRegion _textureRegion;

        public bool Visible { get; set; } = true;

        public SpriteButton(TextureRegion textureRegion) {
            _textureRegion = textureRegion;
            Width         = _textureRegion.Width;
            Height        = _textureRegion.Height;
        }

        public override void Draw(SpriteBatch batch) {
            if (!Visible)
                return;

            batch.Draw(_textureRegion.Texture, _textureRegion.Region, GlobalPosition.X, GlobalPosition.Y,
                       _textureRegion.Width, _textureRegion.Height);
        }
    }
}