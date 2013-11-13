﻿using Microsoft.Xna.Framework;
using NeonEngine;
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
        protected int _elementLevel = 1;

        protected float _cooldownDuration = 0.0f;

        protected ElementState _state;

        public ElementEffect(ElementSystem elementSystem, int elementLevel, Entity entity, NeonStarInput input, GameScreen world)
        {
            _world = world;
            _entity = entity;
            _elementSystem = elementSystem;
            _input = input;
            _elementLevel = elementLevel;
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
            _state = ElementState.End;
        }
    }
}
