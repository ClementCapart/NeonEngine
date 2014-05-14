using Microsoft.Xna.Framework;
using NeonEngine;
using NeonStarLibrary.Components.Avatar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary
{
    public enum ElementState
    {
        Initialization,
        Charge,
        Effect,
        End,
    }

    public class ElementEffect
    {
        protected GameScreen _world;
        protected Entity _entity;
        protected ElementSystem _elementSystem;
        protected NeonStarInput _input;

        public Element EffectElement;

        protected ElementSlot _elementSlot;

        public ElementState State;

        public ElementEffect(ElementSystem elementSystem, ElementSlot slot, Entity entity, NeonStarInput input, GameScreen world)
        {
            _world = world;
            _entity = entity;
            _elementSystem = elementSystem;
            _input = input;
            _elementSlot = slot;

            InitializeLevelParameters();
        }

        public virtual void InitializeLevelParameters()
        {
        
        }

        public virtual void PreUpdate(GameTime gameTime)
        {
        }

        public virtual void Update(GameTime gameTime)
        {
        }

        public virtual void PostUpdate(GameTime gameTime)
        {
        }

        public virtual void End()
        {
            State = ElementState.End;
        }
    }
}
