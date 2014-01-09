using NeonEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NeonStarLibrary
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
        }

        public override void OnTrigger(Entity trigger, Entity triggeringEntity, object[] parameters = null)
        {
            if (File.Exists(@"../Data/Levels/" + GroupName + "/" + LevelName + ".xml"))
            {
                entity.containerWorld.ChangeScreen(new LoadingScreen(Neon.Game, (int)SpawnPointIndex, @"../Data/Levels/" + GroupName + "/" + LevelName));
            } 
            else
            {
                Console.WriteLine("Warning : Level " + LevelName + " in Group " + GroupName + " doesn't exist.");
            }
            
            base.OnTrigger(trigger, triggeringEntity, parameters);
        }
    }
}
