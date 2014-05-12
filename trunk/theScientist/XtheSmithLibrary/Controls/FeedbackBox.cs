using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Linq;
using System.Text;
using TileEngine;

namespace XtheSmithLibrary.Controls
{
    public class FeedbackBox : PictureBox
    {
        private Dictionary<string, bool> dictionary;
        public bool isShowing = false;
        public bool soundPlayed = false;
        private float elapsedTime = 0.0f;
        private Texture2D texture;
        private Rectangle rectangle;
        private SoundEffect questCompleteSound;
        private ContentManager Content;


        public FeedbackBox(Texture2D texture, Rectangle rectangle, ContentManager Content) : base(texture, rectangle)
        {
            this.rectangle = rectangle;
            this.texture = texture;
            dictionary = new Dictionary<string,bool>();
            this.dictionary.Add("Axe", false);
            this.dictionary.Add("Johns riddle", false);
            this.dictionary.Add("Talked to Asterix", false);
            this.dictionary.Add("Permit", false);
            this.dictionary.Add("Belladonna", false);
            this.Content = Content;
            questCompleteSound = Content.Load<SoundEffect>(@"Sounds/Effects/quest_completed");
        }

        protected override void LoadContent()
        {

            //ContentManager Content = Game.Content;
            questCompleteSound = Content.Load<SoundEffect>(@"Sounds/Effects/quest_completed");
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
            if (StoryProgress.ProgressLine["permitHave"] && dictionary["Permit"] == false && isShowing == false)
            {
                this.text = "You now have the item: ";
                this.type = "Permit";
                isShowing = true;
            }
            if (StoryProgress.ProgressLine["contestAgainstJohnFinished"] && dictionary["Johns riddle"] == false && isShowing == false)
            {
                this.text = "You have completed the task: ";
                this.type = "Johns riddle";
                isShowing = true;
            }
            if (StoryProgress.ProgressLine["asterixTalkedTo"] && dictionary["Talked to Asterix"] == false && isShowing == false)
            {
                this.text = "You have completed the task: ";
                this.type = "Talked to Asterix";
                isShowing = true;
            }
            if (StoryProgress.ProgressLine["belladonnaHave"] && dictionary["Belladonna"] == false && isShowing == false)
            {
                this.text = "You now have the item: ";
                this.type = "Belldonna";
                isShowing = true;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (type == null)
                return;
            if (dictionary[type] == false)
            {
                if (soundPlayed == false)
                {
                    questCompleteSound.Play(0.5f, 0.0f, 0.0f);
                    soundPlayed = true;
                }

                spriteBatch.Draw(texture, rectangle, Color.Black);
                spriteBatch.DrawString(spriteFont, text + type,new Vector2(rectangle.X+20, rectangle.Y+20), Color.LightGreen);
                if (elapsedTime > 5)
                {
                    elapsedTime = 0;
                    dictionary[type] = true;
                    isShowing = false;
                    soundPlayed = false;
                }
            }
        }
    }
}
