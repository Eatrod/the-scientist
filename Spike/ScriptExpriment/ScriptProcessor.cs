using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
 /// Läser in en XML fil och går igenom den. Noderna sorteras därefter beroende på vilken typ dom är.
 /// När ett "konversationsblock" lästs in sparas det som ett konversations objekt sen läses nästa block in.
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

                    string[] methods = action.Split(';');
                    string[] aclass = action.Split('!');
                    Type type = Type.GetType("TileContent.ScriptProcessor, TileContent");

                    foreach (var m in methods)
                    {
                        ConversationHandlerActionContent a = new ConversationHandlerActionContent();

                        if (m.Contains(":"))
                        {
                            string[] actionSplit = m.Split('!');
                            actionSplit = m.Split(':');
                            a.MethodName = actionSplit[1];
                            a.Parameters = (object[]) actionSplit[1].Split(',');
                            a.type = type;
                            if (type == null)
                                a.MethodName = "Skript helvete";
                        }
                        else
                        {
                            a.MethodName = m;
                            a.Parameters = null;
                            a.type = type;
                        }

                        h.Actions.Add(a);
                    }

                    c.Handlers.Add(h);
                    
                }

                script.Conversations.Add(c);

                
            }

            return script;
        }
    }
}