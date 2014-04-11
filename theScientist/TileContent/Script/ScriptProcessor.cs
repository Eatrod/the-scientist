using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using System.Xml;
using System.Collections;


namespace TileContent
{
 /// <summary>
 /// L�ser in en XML fil och g�r igenom den. Noderna sorteras d�refter beroende p� vilken typ dom �r.
 /// N�r ett "konversationsblock" l�sts in sparas det som ett konversations objekt sen l�ses n�sta block in.
 /// Retunerar ett skript som skickas till ScriptReader i tileEngine via ScriptWriter klassen
 /// </summary>
    [ContentProcessor(DisplayName = "NPC Script Processor")]
    public class ScriptProcessor : ContentProcessor<XmlDocument, ScriptContent>
    {
        public override ScriptContent Process(
            XmlDocument input,
            ContentProcessorContext context)
        {
            ScriptContent script = new ScriptContent();

            XmlNodeList conversations = input.GetElementsByTagName("Conversation");

            foreach (XmlNode node in conversations)
            {
                ConversationContent c = new ConversationContent();
                c.Name = node.Attributes["Name"].Value;
                c.Text = node.FirstChild.InnerText;

                foreach (XmlNode handlerNode in node.LastChild.ChildNodes)
                {
                    ConversationHandlerContent h = new ConversationHandlerContent();
                    h.caption = handlerNode.Attributes["Caption"].Value;

                    string action = handlerNode.Attributes["Action"].Value;

                    if (action.Contains(":"))
                    {
                        string[] actionSplit = action.Split(':');
                        h.action = actionSplit[0];
                        h.actionParameters = (object[])actionSplit[1].Split(',');
                    }
                    else
                    {
                        h.action = action;
                        h.actionParameters = null;
                    }

                    c.Handlers.Add(h);
                    
                }

                script.Conversations.Add(c);

                
            }

            return script;
        }
    }
}