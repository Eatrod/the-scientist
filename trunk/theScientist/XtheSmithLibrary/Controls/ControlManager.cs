using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;



namespace XtheSmithLibrary.Controls
{
    public class ControlManager : List<Control>
    {
        #region Fields and Properties

        public int selectedControl = 0;
        static SpriteFont spriteFont;
        private ConcurrentQueue<Control> removeQueue = new ConcurrentQueue<Control>();
        private ConcurrentQueue<Control> addQueue = new ConcurrentQueue<Control>();
        object thisLock = new object();
        public static SpriteFont SpriteFont
        {
            get { return spriteFont; }
        }

        #endregion

        #region Event Region

        public event EventHandler FocusChanged;

        #endregion

        #region Constructors

        public ControlManager(SpriteFont spriteFont)
            : base()
        {
            ControlManager.spriteFont = spriteFont;
        }
        public ControlManager(SpriteFont spriteFont, int capacity)
            : base(capacity)
        {
            ControlManager.spriteFont = spriteFont;
        }
        public ControlManager(SpriteFont spriteFont, IEnumerable<Control> collection) :
            base(collection)
        {
            ControlManager.spriteFont = spriteFont;
        }

        #endregion

        #region Methods

        public void Update(GameTime gameTime, PlayerIndex playerIndex)
        {
            if (Count == 0)
                return;
            lock (thisLock)
            {
                foreach (Control c in this)
                {
                    if (c.Enabled)
                        c.Update(gameTime);
                    if (c.HasFocus)
                        c.HandleInput(playerIndex);
                }
                while (removeQueue.Count() > 0)
                {
                    Control c;
                    bool gotItem = removeQueue.TryDequeue(out c);
                    if (gotItem)
                    {
                        this.Remove(c);
                    }
                    
                }
                while (addQueue.Count() > 0)
                {
                    Control c;
                    bool gotItem = addQueue.TryDequeue(out c);
                    if (gotItem)
                    {
                        this.Add(c);
                    }
                    foreach(Control control in this)
                    {
                        control.HasFocus = false;
                    }
                    selectedControl = 0;
                    NextControl();
                    //c.HasFocus = true;
                }
            }
            if (InputHandler.KeyPressed(Keys.Up))
                PreviousControl(); 
            if (InputHandler.KeyPressed(Keys.Down))
                NextControl();
        }
        public void Draw(SpriteBatch spriteBatch)
        {          
            foreach (Control c in this)
            {
                if (c.Visible)
                    c.Draw(spriteBatch);
            }
        }
        public void NextControl()
        {
            if (Count == 0)
                return;
            int currentControl = selectedControl;
            this[selectedControl].HasFocus = false;
            do
            {
                selectedControl++;
                if (selectedControl == Count)
                    selectedControl = 0;
                if (this[selectedControl].TabStop && this[selectedControl].Enabled)
                {
                    if (FocusChanged != null)
                        FocusChanged(this[selectedControl], null);
                    break;
                }
            } while (currentControl != selectedControl);
            this[selectedControl].HasFocus = true;
        }
        public void PreviousControl()
        {
            if (Count == 0)
                return;
            int currentControl = selectedControl;
            this[selectedControl].HasFocus = false;
            do
            {
                selectedControl--;
                if (selectedControl < 0)
                    selectedControl = Count - 1;
                if (this[selectedControl].TabStop && this[selectedControl].Enabled)
                {
                    if (FocusChanged != null)
                        FocusChanged(this[selectedControl], null);
                    break;
                }
            } while (currentControl != selectedControl);
            this[selectedControl].HasFocus = true;
        }

        #endregion

        public void RemoveItem(Control item)
        {
            removeQueue.Enqueue(item);
            //lock (thisLock)
            //{
            //    this.Remove(item);
            //}
        }

        public void AddItem(Control item)
        {
            addQueue.Enqueue(item);
            //lock (thisLock)
            //{
            //    this.Add(item);
            //}
        }
    }
}
