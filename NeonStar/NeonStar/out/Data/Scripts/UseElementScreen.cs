using System;
using NeonEngine;
using NeonStarLibrary;
using NeonEngine.Private;
using Microsoft.Xna.Framework;
using NeonStarLibrary.Components.Avatar;

namespace NeonScripts
{
    public class UseElementScreenScript : ScriptComponent
    {
        private string _avatarName = "";

        public string AvatarName
        {
            get { return _avatarName; }
            set { _avatarName = value; }
        }

        private AvatarCore _avatar = null;
		
		private int _scriptStep = 0;

        public UseElementScreenScript(Entity entity)
            : base(entity, "UseElementScreenScript")
        {
        }
		
		public override void Init()
		{
            if(_avatarName != "")
			{
				Entity avatar = Neon.World.GetEntityByName(_avatarName);
				if(avatar != null)
					_avatar = avatar.GetComponent<AvatarCore>();
			}
			entity.spritesheets.ChangeAnimation("Off");
		}

        public override void Update(GameTime gameTime)
        {
            if (_avatar != null)
                if (_avatar.ElementSystem.LeftSlotElement == Element.Fire && _scriptStep == 0)
				{
					_scriptStep++;
                    entity.spritesheets.ChangeAnimation("Noise", 0, true, false, false,0);
				}
				if (_scriptStep == 1 && entity.spritesheets.IsFinished())
				{
					_scriptStep++;
					entity.spritesheets.ChangeAnimation("UseElement");
				}
        }
		
		public override void OnTrigger(Entity trigger, Entity triggeringEntity, object[] parameters = null)
		{
		}
    }
}
