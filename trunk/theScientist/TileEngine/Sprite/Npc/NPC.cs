using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace TileEngine.Sprite.Npc
{
    public class NPC : CharacterSprite
    {
        public String NPCName { get; set; }
        protected String Location { get; set; }
        protected Vector2 startingPosition;
        public Script script;
        public Dictionary<string, Script> scriptDict;
        protected ContentManager content;

        public Vector2 StartingPosition
        {
            get { return startingPosition; }
            set { startingPosition = value; }
        }

        public NPC(Texture2D texture, Script script)
            : base(texture)
        {
            scriptDict = new Dictionary<string, Script>();
            this.script = script;
            if (script != null)
                this.scriptDict.Add("default", script);
        }

        protected void UpdateSpriteAnimation(Vector2 motion)
        {

            float motionAngle = (float)Math.Atan2(motion.Y, motion.X);

            if (motionAngle >= -MathHelper.PiOver4 && motionAngle <= MathHelper.PiOver4)
            {
                CurrentAnimationName = "WalkRight"; //Right
                //motion = new Vector2(1f, 0f);
            }
            else if (motionAngle >= MathHelper.PiOver4 && motionAngle <= 3f * MathHelper.PiOver4)
            {
                CurrentAnimationName = "WalkDown"; //Down
                //motion = new Vector2(0f, 1f);
            }
            else if (motionAngle <= -MathHelper.PiOver4 && motionAngle >= -3f * MathHelper.PiOver4)
            {
                CurrentAnimationName = "WalkUp"; // Up
                //motion = new Vector2(0f, -1f);
            }
            else
            {
                CurrentAnimationName = "WalkLeft"; //Left
                //motion = new Vector2(-1f, 0f);
            }
        }

        public void RemoveHandler(string captionName)
        {
            foreach (var script in scriptDict)
            {
                foreach (var con in script.Value.conversations)
                {
                restart:
                    foreach (var handler in con.Value.Handlers)
                    {
                        if (handler.Caption == captionName)
                        {
                            con.Value.Handlers.Remove(handler);
                            goto restart;
                        }
                    }
                }
            }
        }

        public void ChangeScript(string scriptName)
        {
            if (scriptDict.ContainsKey(scriptName))
                this.script = scriptDict[scriptName];
        }
    }
}
