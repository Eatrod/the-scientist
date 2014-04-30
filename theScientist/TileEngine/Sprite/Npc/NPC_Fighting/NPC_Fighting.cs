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
        private bool dead;
        protected int Hitpoints { get; set; }
        protected int AttackDamage { get; set; }
        protected List<NPC_Fighting> NpcEnemies { get; set; }
        protected int BattleStance { get; set; }
        private float elapsedRespawn;
        private float delayRespawn;
        public float DelayRespawn
        {
            get { return delayRespawn; }
            set { delayRespawn = value; }
        }
        public float ElapsedRespawn
        {
            get { return elapsedRespawn; }
            set { elapsedRespawn = value; }
        }
        public bool Dead
        {
            get { return dead; }
            set { dead = value; }
        }
        //private List<Tiles> PatrolRoute { get; set; } 

        #endregion

        #region Konstruktor
        public NPC_Fighting(Texture2D texture, Script script) : base(texture,script)
        {

        }
        #endregion

        #region Update fält


        #endregion

    }
}
#endregion