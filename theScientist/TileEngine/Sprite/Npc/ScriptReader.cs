using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace TileEngine.Sprite.Npc
{
    public class ScriptReader : ContentTypeReader<Script>
    {
        protected override Script Read(ContentReader input, Script existingInstance)
        {
            int conversationCount = input.ReadInt32();
            Conversation[] conversations = new Conversation[conversationCount];
            for (int i = 0; i < conversationCount; i++)
            {
                conversations[i] = input.ReadObject<Conversation>();
            }
            return new Script(conversations);
        }
    }

    public class ConversationReader : ContentTypeReader<Conversation>
    {
        protected override Conversation Read(ContentReader input, Conversation existingInstance)
        {
            string name = input.ReadString();
            string text = input.ReadString();

            int handlerCount = input.ReadInt32();
            ConversationHandler[] handlers = new ConversationHandler[handlerCount];
            for(int i = 0; i < handlerCount; i++)
            {
                handlers[i] = input.ReadObject<ConversationHandler>();
            }
            return new Conversation(name, text, handlers);
        }
    }

    public class ConversationHandlerReader : ContentTypeReader<ConversationHandler>
    {
        protected override ConversationHandler Read(ContentReader input, ConversationHandler existingInstance)
        {
            string caption = input.ReadString();
            string actionName = input.ReadString();
            object[] parameters = input.ReadObject<object[]>();

            return new ConversationHandler(caption, actionName, parameters);
        }
    }
}
