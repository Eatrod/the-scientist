#region Using fält

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TileEngine.Sprite.Npc;
using Microsoft.Xna.Framework.Graphics;

#endregion

#region Klass fält
namespace TileEngine.Sprite.Npc.Npc_Fighting
{
    //TODO: Konstruktor parameter
    class NPC_Fighting_Hostile : NPC_Fighting
    {
        #region Medlemsvariabler

        protected int AlertedStatus { get; set; }
        protected bool ChasingPlayer { get; set; }

        #endregion

        #region Konstruktor

        public NPC_Fighting_Hostile(Texture2D texture, Script script): base(texture, script)
        {
            this.AlertedStatus = 0;
            this.ChasingPlayer = false;
        }

        #endregion

        #region Update fält

        /// <summary>
        /// Uppdaterar NPCn alerted status alltså hur "misstänksam" NPCn för tillfället är.
        /// Ju mer misstänksam desto längre synfält etc på en skala vi får komma på.
        /// </summary>
        public void AlertedUpdate()
        {
            
        }

        /// <summary>
        /// Ska uppdatera hurvida NPCn för tillfället jagar spelaren
        /// </summary>
        public void ChasingPlayerUpdate()
        {
            
        }

        #endregion
    }
}
#endregion
