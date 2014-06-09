using NeonEngine;
using NeonEngine.Components.Triggers;
using NeonStarLibrary.Components.Avatar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NeonStarLibrary.Components.Triggers
{
    public class ChangeLevel : Component
    {
#region Properties
        private string _groupName = "";

        public string GroupName
        {
            get { return _groupName; }
            set { _groupName = value; }
        }

        private string _levelName = "";

        public string LevelName
        {
            get { return _levelName; }
            set { _levelName = value; }
        }

        private float _spawnPointIndex = 0;

        public float SpawnPointIndex
        {
            get { return _spawnPointIndex; }
            set { _spawnPointIndex = value; }
        }
#endregion 

        public ChangeLevel(Entity entity)
            :base(entity, "ChangeLevel")
        {
            RequiredComponents = new Type[] { typeof(HitboxTrigger) };
        }

        public override void Init()
        {
            if (entity.spritesheets != null)
                entity.spritesheets.ChangeAnimation("Opening", 0, true, false, false);
            base.Init();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (entity.spritesheets != null && entity.spritesheets.CurrentSpritesheetName == "Opening" && entity.spritesheets.IsFinished())
                entity.spritesheets.ChangeAnimation("Opened");
            base.Update(gameTime);
        }

        public override void OnTrigger(Entity trigger, Entity triggeringEntity, object[] parameters = null)
        {
            if (File.Exists(@"../Data/Levels/" + GroupName + "/" + LevelName + "/" + LevelName + "_Info.xml"))
            {
                if (GroupName == "02CityLevel" &&  entity.GameWorld.LevelGroupName == "01TrainingLevel")
                {
                    LevelArrival.NextLevelToDisplay = "LowerCity";
                    LevelArrival.WaitForNextLevel = true;
                }
                else if (LevelName == "01TrainingEntrance" && entity.GameWorld.LevelName == "00TrainingOpening")
                {
                    LevelArrival.NextLevelToDisplay = "Frontier";
                    LevelArrival.WaitForNextLevel = true;
                }
                entity.GameWorld.ChangeLevel(GroupName, LevelName, (int)SpawnPointIndex);
                if (entity.spritesheets != null)
                    entity.spritesheets.ChangeAnimation("Closing", 0, true, false, false);
                if (entity.GameWorld.Avatar != null && entity.GameWorld.Avatar.spritesheets != null)
                {
                    AvatarCore ac = entity.GameWorld.Avatar.GetComponent<AvatarCore>();
                    if (ac != null)
                    {
                        entity.GameWorld.Avatar.spritesheets.Active = false;
                        ac.State = AvatarState.ChangingLevel;
                    }
                }
            } 
            else
            {
                Console.WriteLine("Warning : Level " + LevelName + " in Group " + GroupName + " doesn't exist.");
            }
            
            base.OnTrigger(trigger, triggeringEntity, parameters);
        }
    }
}
