using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;

namespace ConvenientChests.CategorizeChests.Interface.Widgets {
    public class ScrollBar : Widget {
        private ScrollBarRunner _runner;

        private int _scrollPosition;

        public int ScrollPosition {
            get => _scrollPosition;
            set {
                _scrollPosition = value;
                UpdateScroller();
            }
        }


        private int _scrollMax;

        public int ScrollMax {
            get => _scrollMax;
            set {
                _scrollMax = value;
                UpdateScroller();
            }
        }

        public int Step { get; set; } = 1;

        public bool Visible { get; set; } = true;

        private SpriteButton ScrollUpButton   { get; }
        private SpriteButton ScrollDownButton { get; }
        private Rectangle    _scrollBackground;

        public ScrollBar() {
            ScrollUpButton   = new SpriteButton(Sprites.UpArrow);
            ScrollDownButton = new SpriteButton(Sprites.DownArrow);
            _runner           = new ScrollBarRunner {Width = 24};

            AddChild(ScrollUpButton);
            AddChild(ScrollDownButton);
            AddChild(_runner);

            PositionElements();

            ScrollUpButton.OnPress   += () => Scroll(-1);
            ScrollDownButton.OnPress += () => Scroll(+1);

            ModEntry.StaticHelper.Events.Input.ButtonReleased  += InputOnButtonReleased;
            ModEntry.StaticHelper.Events.GameLoop.UpdateTicked += GameLoopOnUpdateTicked;
        }

        protected override void OnDimensionsChanged() {
            if (Width != 64) {
                Width = 64;
                return;
            }

            base.OnDimensionsChanged();
            PositionElements();
        }

        private void PositionElements() {
            if (ScrollDownButton == null)
                return;

            ScrollUpButton.X   = 0;
            ScrollDownButton.X = 0;
            ScrollDownButton.Y = Height - ScrollDownButton.Height;

            _scrollBackground.X      = 20;
            _scrollBackground.Y      = ScrollUpButton.Height                                    - 4;
            _scrollBackground.Height = Height - ScrollUpButton.Height - ScrollDownButton.Height + 8;
            _scrollBackground.Width  = _runner.Width;
            _runner.X                = _scrollBackground.X;


            _scrollBackground.Location = Globalize(_scrollBackground.Location);
            UpdateScroller();
        }

        private void UpdateScroller() {
            if (Step == 0)
                return;

            _runner.Height = (int) (_scrollBackground.Height * (Step / (float) ScrollMax));
            _runner.Y      = 60 + (int) ((_scrollBackground.Height - _runner.Height) * Math.Min(1, (ScrollPosition / (float) (ScrollMax - Step))));
        }


        public override void Draw(SpriteBatch batch) {
            if (!Visible)
                return;

            // draw background
            IClickableMenu.drawTextureBox(batch, Game1.mouseCursors,
                                          new Rectangle(403, 383, 6, 6),
                                          _scrollBackground.X, _scrollBackground.Y, _scrollBackground.Width, _scrollBackground.Height,
                                          Color.White, 4f, false);

            base.Draw(batch);
        }

        private void Scroll(int direction) {
            if (ScrollMax == 0)
                return;

            ScrollPosition = Math.Max(0, Math.Min(ScrollMax, ScrollPosition + direction * Step));
            OnScroll?.Invoke(this, new ScrollBarEventArgs(ScrollPosition, direction));
            UpdateScroller();
        }


        protected bool Scrolling;

        public override bool ReceiveLeftClick(Point point) {
            if (base.ReceiveLeftClick(point))
                return true;

            var localPoint = new Point(point.X - _runner.Position.X, point.Y - _runner.Position.Y);
            if (_runner.LocalBounds.Contains(localPoint))
                Scrolling = true;


            return true;
        }

        /// <summary>
        /// Update ScrollRunner position and dispatch scrolling events
        /// </summary>
        private void GameLoopOnUpdateTicked(object sender, UpdateTickedEventArgs e) {
            if (!Scrolling)
                return;

            var mouseY   = Game1.getMouseY();
            var progress = Math.Min(Math.Max(0f, mouseY - _scrollBackground.Y) / (Height), 1);
            ScrollPosition = (int) (progress * ScrollMax);

            OnScroll?.Invoke(this, new ScrollBarEventArgs(ScrollPosition, mouseY < GlobalBounds.Y ? -1 : 1));
        }

        /// <summary>
        /// Cancel scrolling on button release
        /// </summary>
        private void InputOnButtonReleased(object sender, ButtonReleasedEventArgs e) {
            if (Scrolling && (e.Button == SButton.MouseLeft || e.Button.IsUseToolButton()))
                Scrolling = false;
        }


        public override bool ReceiveScrollWheelAction(int amount) => base.ReceiveScrollWheelAction(amount);

        public event EventHandler<ScrollBarEventArgs> OnScroll;

        public class ScrollBarEventArgs : EventArgs {
            public ScrollBarEventArgs(int position, int direction) {
                Position  = position;
                Direction = direction;
            }

            public int Position  { get; set; }
            public int Direction { get; set; }
        }

        protected class ScrollBarRunner : Widget {
            private static readonly TextureRegion TextureTop = new TextureRegion(Game1.mouseCursors, new Rectangle(435, 463, 6, 3), true);
            private static readonly TextureRegion TextureMid = new TextureRegion(Game1.mouseCursors, new Rectangle(435, 466, 6, 4), true);
            private static readonly TextureRegion TextureBot = new TextureRegion(Game1.mouseCursors, new Rectangle(435, 470, 6, 3), true);

            public bool Visible { get; set; } = true;

            public override void Draw(SpriteBatch batch) {
                if (!Visible)
                    return;

                base.Draw(batch);

                var rect = GlobalBounds;
                batch.Draw(TextureMid, rect.X, rect.Y,                          rect.Width,       rect.Height);
                batch.Draw(TextureTop, rect.X, rect.Y,                          TextureTop.Width, TextureTop.Height);
                batch.Draw(TextureBot, rect.X, rect.Bottom - TextureBot.Height, TextureBot.Width, TextureBot.Height);
            }
        }
    }
}