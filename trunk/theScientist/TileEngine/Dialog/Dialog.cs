using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TileEngine.Sprite.Npc;
using TileEngine.Sprite.Npc.NPC_Story;

namespace TileEngine.Dialog
{
    public class Dialog
    {
        public Conversation conversation = null;
        public NPC_Story Npc;

        public Rectangle area = new Rectangle(0,0,500,500);

        private SpriteFont spriteFont;
        private SpriteBatch spriteBatch;
        private ContentManager content;
        private Texture2D background;

        private int currentHandler = 0;
        private KeyboardState lastState;

        public void NextText(NPC_Story npc, Conversation conversation)
        {
            this.Npc = npc;
            this.conversation = conversation;
            conversation.Handlers[currentHandler].Invoke(Npc);
        }

        public void Update()
        {
            KeyboardState newState = Keyboard.GetState();

            if (conversation == null || Npc == null)
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
    }
}
