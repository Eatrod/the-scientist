using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

// TODO: replace this with the type you want to write out.
using TWrite = System.String;

namespace TileContent
{
    /// <summary>
    /// This class will be instantiated by the XNA Framework Content Pipeline
    /// to write the specified data type into binary .xnb format.
    ///
    /// This should be part of a Content Pipeline Extension Library project.
    /// </summary>
    [ContentTypeWriter]
    public class ScriptWriter : ContentTypeWriter<ScriptContent>
    {
        protected override void Write(ContentWriter output, ScriptContent value)
        {
            output.Write(value.Conversation.Count);
            foreach (ConversationContent c in value.Conversation)
            {
                output.WriteObject(c);
            }
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            // TODO: change this to the name of your ContentTypeReader
            // class which will be used to load this data.
            return "TileEngine.SpriteReader, TileEngine";
        }
    }

    [ContentTypeWriter]
    public class ConversationWriter : ContentTypeWriter<ConversationContent>
    {
        protected override void Write(ContentWriter output, ConversationContent value)
        {
            output.Write(value.name);
            output.Write(value.Text);

            output.Write(value.Handlers.Count);
            foreach (ConversationHandlerContent c in value.Handlers)
            {
                output.WriteObject(c);
            }
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            // TODO: change this to the name of your ContentTypeReader
            // class which will be used to load this data.
            return "TileEngine.ConversationReader, TileEngine";
        }
    }

    [ContentTypeWriter]
    public class ConversationHandlerWriter : ContentTypeWriter<ConversationHandlerContent>
    {
        protected override void Write(ContentWriter output, ConversationHandlerContent value)
        {
            output.Write(value.caption);
            output.Write(value.action);
            output.WriteObject(value.actionParameters);
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            // TODO: change this to the name of your ContentTypeReader
            // class which will be used to load this data.
            return "MyNamespace.MyContentReader, MyGameAssembly";
        }
    }
}
