using NeonEngine;
using NeonEngine.Components.Graphics2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary.Components.Scripts
{
    public class FollowOpacity : Component
    {
        private string _entityName = "";

        public string EntityName
        {
          get { return _entityName; }
          set { _entityName = value; }
        }

        private List<DrawableComponent> _drawableComponents;
        private Graphic _graphicToFollow;

        public bool Active = true;

        public FollowOpacity(Entity entity)
            :base(entity, "FollowOpacity")
        {
        }     

        public override void Init()
        {
            Entity e = entity.GameWorld.GetEntityByName(_entityName);
            if(e != null)
                _graphicToFollow = e.GetComponent<Graphic>();

            _drawableComponents = entity.GetComponentsByInheritance<DrawableComponent>();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (Active)
            {
                if (_graphicToFollow != null)
                {
                    if (_drawableComponents != null)
                    {
                        foreach (DrawableComponent dc in _drawableComponents)
                        {
                            dc.Opacity = _graphicToFollow.Opacity;
                        }
                    }
                }
            }

            
 	        
            base.Update(gameTime);
        }
    }
}
