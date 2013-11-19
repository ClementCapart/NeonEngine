using System;
using NeonEngine;
using NeonStarLibrary;
using NeonEngine.Private;
using Microsoft.Xna.Framework;

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

        private Avatar _avatar = null;

        public UseElementScreenScript(Entity entity)
            : base(entity, "UseElementScreenScript")
        {
        }
		
		public override void Init()
		{
            if (_avatarName != "")
                _avatar = Neon.world.GetEntityByName(_avatarName).GetComponent<Avatar>();
			entity.spritesheets.ChangeAnimation("Off");
		}

        public override void Update(GameTime gameTime)
        {
            if (_avatar != null)
                if (_avatar.elementSystem.LeftSlotElement == Element.Fire)
				{
                    entity.graphic.Remove();
                    entity.spritesheets.ChangeAnimation("UseElement");
				}
        }
		
		public override void OnTrigger(Entity trigger, Entity triggeringEntity, object[] parameters = null)
		{
		}
    }
}
