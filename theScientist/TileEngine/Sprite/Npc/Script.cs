using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

namespace TileEngine.Sprite.Npc
{
    public class Script
    {
        Dictionary<string, Conversation> conversations =
            new Dictionary<string, Conversation>();

        public Conversation this[string name]
        { 
            get{return conversations[name];}
        }

        public Script(params Conversation[] newConversations)
        {
            foreach (Conversation c in newConversations)
                conversations.Add(c.Name, c);
        }
    }

    public class Conversation
    {
        string name;
        string text;
        public Collection<ConversationHandler> Handlers = new Collection<ConversationHandler>();

        public string Name
        {
            get { return name; } 
        
        }

        public string Text
        {
            get { return text; }
        }

        public Conversation(string name, string text, params ConversationHandler[] newHandlers)
        {
            this.name = name;
            this.text = text;
            foreach (ConversationHandler ch in newHandlers)
                Handlers.Add(ch);
        }
    }

    public class ConversationHandler
    {
        string caption;
        private ConversationHandlerAction[] actions;

        public string Caption
        {
            get { return caption; }
        }

        public ConversationHandler(string caption, params ConversationHandlerAction[] actions)
        {
            this.caption = caption;
            this.actions = actions;
            
        }

        public void Invoke(NPC_Story.NPC_Story npc)
        {
            foreach (var action in actions)
            {
                action.Invoke(npc);
            }
        }
    }

    public class ConversationHandlerAction
    {
        private MethodInfo method;
        private object[] parameters;

        public ConversationHandlerAction(string methodName, object[] parameters)
        {
            method = typeof (NPC_Story.NPC_Story).GetMethod(methodName);
            this.parameters = parameters;
        }

        public void Invoke(NPC_Story.NPC_Story npc)
        {
            method.Invoke(npc, parameters);
        }
    }

}
