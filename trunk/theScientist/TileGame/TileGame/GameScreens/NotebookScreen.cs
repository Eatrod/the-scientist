using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using XtheSmithLibrary;
using XtheSmithLibrary.Controls;
using TileGame;
using TileEngine;

namespace TileGame.GameScreens
{
    public class NotebookScreen : BaseGameState
    {
        #region Field region

        //Message message;
        int textMessage = 0;
        int pageIndex = 0;
        int lastPageIndex = 0;
        int leftPageIndex = 0;
        int rightPageIndex = 0;
        Texture2D backgroundImage;
        
        Dictionary<int, Message> messageDict = new Dictionary<int, Message>();
        List<int> ListOfUnlockedKeys;
        Label leftText;
        Label rightText;
        Label leftPagenumberText;
        Label rightPagenumberText;
        Rectangle textRect;
        #endregion

        #region Property Region
        #endregion

        #region Constructor Region
        public NotebookScreen(Game game, GameStateManager manager)
            : base(game, manager)
        {

        }
        #endregion

        #region XNA Method Region
        public override void Initialize()
        {
            ListOfUnlockedKeys = new List<int>();
            textRect = new Rectangle(0, 0, 400, 600);
            base.Initialize();
        }
        protected override void LoadContent()
        {
            ContentManager Content = GameRef.Content;
            backgroundImage = Content.Load<Texture2D>(@"Backgrounds\book");
            base.LoadContent();

            leftPagenumberText = new Label();
            leftPagenumberText.Position = new Vector2(100, 600);
            leftPagenumberText.Text = pageIndex.ToString();
            leftPagenumberText.Color = Color.DarkBlue;
            ControlManager.Add(leftPagenumberText);

            rightPagenumberText = new Label();
            rightPagenumberText.Position = new Vector2(900, 600);
            rightPagenumberText.Text = pageIndex.ToString();
            rightPagenumberText.Color = Color.DarkBlue;
            ControlManager.Add(rightPagenumberText);

            leftText = new Label();
            leftText.Position = new Vector2(100, 100);
            leftText.Text = "";
            leftText.Color = Color.DarkBlue;
            ControlManager.Add(leftText);

            rightText = new Label();
            rightText.Position = new Vector2(GraphicsDevice.Viewport.Width/ 2 + 67, 100);
            rightText.Text = "";
            rightText.Color = Color.DarkBlue;
            ControlManager.Add(rightText);

            InsertTextToMessageDictionary(0, "");
            InsertTextToMessageDictionary(1, "Hint: Find Asterix and talk to him!");
            InsertTextToMessageDictionary(2, "Task: Asterix told you to find potato The Belladonna. Check out the abandoned fields in the north west.");
            InsertTextToMessageDictionary(3, "Task: You have now the Belladonna potato and should move on to next town.");
            InsertTextToMessageDictionary(4, "Completed: You managed to solve Johns riddle.");
            InsertTextToMessageDictionary(5, "Hint: You need an official permit to leave the town");
            InsertTextToMessageDictionary(6, "Completed: You have aquired a valid permit");
            InsertTextToMessageDictionary(7, "Hint: Now you have the axe, you can use it to figth or chop");

            messageDict[0].Unlocked = true;
            messageDict[1].Unlocked = true;
        }
        public override void Update(GameTime gameTime)
        {
            ControlManager.Update(gameTime, PlayerIndex.One);
            base.Update(gameTime);

            if (StoryProgress.ProgressLine["asterixTalkedTo"])
                messageDict[2].Unlocked = true;
            if (StoryProgress.ProgressLine["belladonnaHave"])
                messageDict[3].Unlocked = true;
            if (StoryProgress.ProgressLine["contestAgainstJohnFinished"])
                messageDict[4].Unlocked = true;
            if (StoryProgress.ProgressLine["permitHave"])
                messageDict[6].Unlocked = true;
            if (StoryProgress.ProgressLine["Axe"])
                messageDict[7].Unlocked = true;

            GetKeysThatAreNotLocked();

            if (pageIndex < (ListOfUnlockedKeys.Count))
            {
                if(pageIndex + 1 < ListOfUnlockedKeys.Count)
                    leftPageIndex = ListOfUnlockedKeys[pageIndex + 1];
                else
                    leftPageIndex = 0;
                if (messageDict.ContainsKey(leftPageIndex))
                {
                    leftText.Color = GetTextColor(leftText.Text.Split(':')[0]);
                    leftText.Text = messageDict[leftPageIndex].Text;
                    if (pageIndex == 0)
                        leftPagenumberText.Text = "1";
                    else
                    {
                        leftPagenumberText.Text = (pageIndex+1).ToString();
                    }
                }

                if (pageIndex + 2 < ListOfUnlockedKeys.Count)
                    rightPageIndex = ListOfUnlockedKeys[pageIndex + 2];
                else
                    rightPageIndex = 0;
                if (messageDict.ContainsKey(rightPageIndex))
                {
                    rightText.Color = GetTextColor(rightText.Text.Split(':')[0]);
                    rightText.Text = messageDict[rightPageIndex].Text;
                    if (pageIndex == 0)
                        rightPagenumberText.Text = "2";
                    else
                    {
                        rightPagenumberText.Text = (pageIndex+2).ToString();
                    }
                }
            }
            if (InputHandler.KeyReleased(Keys.N))
                StateManager.PopState();
            if(InputHandler.KeyReleased(Keys.Left))
            {
                pageIndex -= 2;
                if (pageIndex < 0)
                    pageIndex = 0;
            }
            if(InputHandler.KeyReleased(Keys.Right))
            {
                lastPageIndex = pageIndex;
                pageIndex += 2;
                if (pageIndex > ListOfUnlockedKeys.Count - 2)
                    pageIndex = lastPageIndex;
            }
        }
        public override void Draw(GameTime gameTime)
        {
            GameRef.spriteBatch.Begin();
            base.Draw(gameTime);

            GameRef.spriteBatch.Draw(
                backgroundImage,
                GameRef.ScreenRectangle,
                Color.White);

            ControlManager.Draw(GameRef.spriteBatch);

            GameRef.spriteBatch.End();
        }
        #endregion

        #region Game State Method Region
        #endregion

        #region Methods Region
        public void InsertTextToMessageDictionary(int key, string text)
        {
            StringBuilder stringBuild = new StringBuilder();
            string formatedText;
            //text = "Nu maste du hitta nyckeln, den ligger dar man minst anar";
            WordWrapper.WrapWord(new StringBuilder(text), stringBuild, leftText.SpriteFont, textRect);
            formatedText = stringBuild.ToString();
            if (!messageDict.ContainsKey(key))
                messageDict.Add(key, new Message(formatedText));
            else
                messageDict[key].Text = formatedText;
        }

        private void GetKeysThatAreNotLocked()
        {
            ListOfUnlockedKeys.Clear();
            foreach(KeyValuePair<int, Message> pair in messageDict)
            {
                if (pair.Value.Unlocked)
                    ListOfUnlockedKeys.Add(pair.Key);
            }
            ListOfUnlockedKeys.Sort();
        }

        private Color GetTextColor(string text)
        {
            if (text == "Hint")
                return Color.ForestGreen;
            else if (text == "Completed")
                return Color.IndianRed;
            else if (text == "Task")
                return Color.DarkBlue;
            else
            {
                return Color.DarkBlue;
            }
        }

        public class WordWrapper 
	    { 
	        public static char[] NewLine = {'\r','\n'}; 
	        public static void WrapWord(StringBuilder original, StringBuilder target, SpriteFont font, Rectangle bounds) 
	        { 
	            int lastWhiteSpace = 0; 
	            Vector2 currentTargetSize; 
	            for (int i = 0; i < original.Length; i++) 
	            {                 
	                char character = original[i]; 
	                if (char.IsWhiteSpace(character)) 
	                { 
	                    lastWhiteSpace = target.Length; 
	                } 
	                target.Append(character); 
	                currentTargetSize = font.MeasureString(target); 
	                if (currentTargetSize.X > bounds.Width) 
	                {                     
	                    target.Insert(lastWhiteSpace, NewLine); 
	                    target.Remove(lastWhiteSpace + NewLine.Length, 1); 
	                } 
	            } 
	        } 
	    }
        #endregion
    }
}
