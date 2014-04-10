using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TileEngine.Sprite.Npc;

namespace TileEngine
{
    public class Dialog : DrawableGameComponent
    {
        //All kod för att göra dialog ruta med karaktärs ansiktena här
        public Conversation conversation = null;
        public NPC_Story Npc;

        public Rectangle area = new Rectangle(0,0,300,200);

        private SpriteFont spriteFont;
        private SpriteBatch spriteBatch;
        private ContentManager content;
        private Texture2D background;

        private int currentHandler = 0;
        private KeyboardState lastState;

        public Dialog(Game game, ContentManager content) : base(game)
        {
            this.content = content;
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            spriteFont = content.Load<SpriteFont>("Fonts/ControlFont");

            background = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            background.SetData<Color>(new Color[] { Color.White });
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState newState = Keyboard.GetState();

            if (conversation != null || Npc != null)
                return;

            if (newState.IsKeyDown(Keys.Up) && lastState.IsKeyUp(Keys.Up))
            {
                currentHandler--;
                if(currentHandler < 0)
                    currentHandler = conversation.Handlers.Count - 1;
            }

            if (newState.IsKeyDown(Keys.Down) && lastState.IsKeyUp(Keys.Down))
            {
                currentHandler = (currentHandler - 1) % conversation.Handlers.Count;
            }

            if (newState.IsKeyDown(Keys.Space) && lastState.IsKeyUp(Keys.Space))
            {
                conversation.Handlers[currentHandler].Invoke(Npc);
            }

            lastState = newState;
        }

        public override void Draw(GameTime gameTime)
        {
            if (conversation == null)
                return;

            Rectangle dest = new Rectangle(GraphicsDevice.Viewport.Width / 2 - area.Width / 2, 
                                           GraphicsDevice.Viewport.Height / 2 - area.Height / 2,
                                           area.Width,
                                           area.Height);
            spriteBatch.Begin();
            spriteBatch.Draw(background,dest, new Color(0,0,0,100));
            spriteBatch.End();

            spriteBatch.Begin();
            spriteBatch.DrawString(spriteFont,conversation.Text, new Vector2(dest.X, dest.Y), Color.White );
            spriteBatch.End();
        }
    }
}
