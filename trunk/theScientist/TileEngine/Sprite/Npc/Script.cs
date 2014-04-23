using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace TileEngine.Sprite.Npc
{
    public class Script
    {
        Dictionary<string, Conversation> conversations =
            new Dictionary<string, Conversation>();

        public Conversation this[string name]
        {
            get
            {
                if (name == "random")
                {
                    Random rand = new Random((int)DateTime.Now.Ticks);
                    return conversations.ElementAt(rand.Next(0, conversations.Count)).Value;
                }
                else
                {
                    return conversations[name];
                }
            }
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

        public void Invoke(NPC_Story.NPC_Story npc, PlayerCharacter player, StoryProgress story)
        {
            foreach (var action in actions)
            {
                if (action.GetInvoker() == "NPC")
                    action.InvokeNPC(npc);
                else if (action.GetInvoker() == "Player")
                    action.InvokePlayer(player);
                else if (action.GetInvoker() == "Story" && story != null)
                    action.InvokeStory(story);

            }
        }
    }

    public class ConversationHandlerAction
    {
        private MethodInfo method;
        private object[] parameters;
        private string invoker { get; set; }

        public string GetInvoker()
        { 
            return this.invoker;
        }

        public ConversationHandlerAction(string methodName, object[] parameters)
        {
            if (typeof (NPC_Story.NPC_Story).GetMethod(methodName) != null)
            {
                method = typeof (NPC_Story.NPC_Story).GetMethod(methodName);
                this.invoker = "NPC";
            }
            else if (typeof (PlayerCharacter).GetMethod(methodName) != null)
            {
                method = typeof (PlayerCharacter).GetMethod(methodName);
                this.invoker = "Player";
            }
            else if (typeof(StoryProgress).GetMethod(methodName) != null)
            {
                method = typeof (StoryProgress).GetMethod(methodName);
                this.invoker = "Story";
            }

            this.parameters = parameters;
        }

        public void InvokeNPC(NPC_Story.NPC_Story npc)
        {
            method.Invoke(npc, parameters);
        }

        public void InvokePlayer(PlayerCharacter player)
        {
            method.Invoke(player, parameters);
        }

        public void InvokeStory(StoryProgress progress)
        {
            method.Invoke(progress, parameters);
        }

    }

}
