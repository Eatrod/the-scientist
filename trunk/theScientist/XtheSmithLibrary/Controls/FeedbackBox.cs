using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Linq;
using System.Text;
using TileEngine;

namespace XtheSmithLibrary.Controls
{
    public class FeedbackBox : PictureBox
    {
        private Dictionary<string, bool> dictionary;
        private string type;
        private string currentKey;
        public bool isShowing = false;
        private float elapsedTime = 0.0f;

        public FeedbackBox()
        {
            dictionary = new Dictionary<string,bool>();
            this.dictionary.Add("Axe", false);
            this.dictionary.Add("John", false);
            this.dictionary.Add("Talked to Asterix", false);
        }

        public override void Update(GameTime gameTime)
        {
            if (isShowing)
                elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            //TODO: Hitta ett sätt att implementera den här lösningen (förutsätter att this.dictionary=StoryProgress.ProgressLine)
            /*foreach (var key in StoryProgress.ProgressLine)
            {
                if (dictionary.ContainsKey(key.Key))
                {
                    if (dictionary[key.Key] == false && isShowing == false)
                    {
                        this.text = "You now have the item: TESTEST";
                        this.currentKey = key.Key;
                        isShowing = true;
                    }
                }

            }*/
            if (StoryProgress.ProgressLine["Axe"] && dictionary["Axe"] == false && isShowing == false)
            {
                this.text = "You now have the item: ";
                this.type = "Axe";
                isShowing = true;
            }
            if (StoryProgress.ProgressLine["contestAgainstJohnFinished"] && dictionary["John"] == false && isShowing == false)
            {
                this.text = "You have completed the task: ";
                this.type = "John";
                isShowing = true;
            }
            if (StoryProgress.ProgressLine["asterixTalkedTo"] && dictionary["Talked to Asterix"] == false && isShowing == false)
            {
                this.text = "You have completed the task: ";
                this.type = "Talked to Asterix";
                isShowing = true;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (type == null)
                return;
            if (dictionary[type] == false)
            {
                spriteBatch.DrawString(spriteFont, text + type,new Vector2(500, 500), Color.White);
                if (elapsedTime > 5)
                {
                    elapsedTime = 0;
                    dictionary[type] = true;
                    isShowing = false;
                }
            }
        }
    }
}
