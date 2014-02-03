using NeonEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary.Components.Private
{
    public enum CollectibleState
    {
        Available,
        Used,
        Disabled
    }

    public class Collectible : Component
    {
        #region Properties
       
        #endregion

        public string LevelName;
        public string GroupName;
        public string EntityName;

        public CollectibleState State = CollectibleState.Available;

        public Collectible(Entity entity, string name)
            :base(entity, name)
        {
        }

        public override void Init()
        {
            LevelName = entity.GameWorld.LevelName;
            GroupName = entity.GameWorld.LevelGroupName;
            EntityName = entity.Name;

            base.Init();
        }


    }
}
