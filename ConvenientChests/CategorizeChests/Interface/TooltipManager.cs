using ConvenientChests.CategorizeChests.Interface.Widgets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;

namespace ConvenientChests.CategorizeChests.Interface
{
    class TooltipManager : ITooltipManager
    {
        private Widget _tooltip;

        public void ShowTooltipThisFrame(Widget tooltip)
        {
            _tooltip = tooltip;
        }

        public void Draw(SpriteBatch batch)
        {
            if (_tooltip != null)
            {
                var mousePosition = Game1.getMousePosition();

                _tooltip.Position = new Point(
                    mousePosition.X + 8 * Game1.pixelZoom,
                    mousePosition.Y + 8 * Game1.pixelZoom
                );

                _tooltip.Draw(batch);

                _tooltip = null;
            }
        }
    }
}