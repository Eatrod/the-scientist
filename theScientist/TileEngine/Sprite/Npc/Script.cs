using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections.ObjectModel;



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
        MethodInfo action;
        object[] actionParameters;

        public string Caption
        {
            get { return caption; }
        }

        public ConversationHandler(string caption, string methodName, object[] parameters)
        {
            this.caption = caption;
            action = typeof(NPC).GetMethod(methodName);

            if(parameters != null)
                actionParameters = (object[])parameters.Clone();
            
        }

        public void Invoke(NPC npc)
        {
            action.Invoke(npc, actionParameters);
        }
    }

}
