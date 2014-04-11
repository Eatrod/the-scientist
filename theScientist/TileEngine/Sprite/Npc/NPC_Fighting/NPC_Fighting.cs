#region Using fält

using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

#endregion

#region Klass fält
namespace TileEngine.Sprite.Npc.NPC_Fighting
{
    public class NPC_Fighting : NPC
    {
        #region Medlemsvariabler

        //TODO: Komma på hur synfältet ska defineras (Cirkelsektor är en place holder idé)
        //private Math.circlesector VisionRange{ get; set;}
        protected int Hitpoints { get; set; }
        protected int AttackDamage { get; set; }
        protected List<NPC_Fighting> NpcEnemies { get; set; }
        protected int BattleStance { get; set; }
        //private List<Tiles> PatrolRoute { get; set; } 

        #endregion

        #region Konstruktor
        public NPC_Fighting(Texture2D texture, Script script) : base(texture,script)
        {

        }
        #endregion

        #region Update fält

        /// <summary>
        /// Uppdaterar NPCns synfält i takt med att den rör sig
        /// </summary>
        public void VisionRangeUpdate()
        {
            
        }

        /// <summary>
        /// Ska lägga till (eller ta bort) fiender i NPCns lista över vilka den ska slåss mot
        /// </summary>
        public void EnemyListUpdate()
        {

        }

        /// <summary>
        /// Uppdaterar NPCns patrullrutt igenom att ta bort eller lägga till nya tiles.
        /// </summary>
        public void PatrolRouteUpdate()
        {
            
        }

        #endregion

        /// <summary>
        /// Alla objekt som NPCn ser?
        /// </summary>
        public void VisionObjectInVision()
        {
            
        }
    }
}
#endregion