using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using NeonEngine;

namespace NeonStarLibrary
{
    public class ParallaxPlan : Entity
    {
        ScrollingType _scrollingType;
        float _speed;
        public float Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }
        string _graphicTag;
        public string graphicTag
        {
            get { return _graphicTag; }
            set { _graphicTag = value; }
        }
        Graphic backgroundImage;

        public float drawLayer
        {
            get { return backgroundImage.Layer; }
            set { backgroundImage.Layer = value; }
        }

        public ScrollingType scrollingType
        {
            get { return _scrollingType; }
            set { _scrollingType = value; }
        }

        public ParallaxPlan(World containerWorld, Vector2 Position, string graphicTag, ScrollingType scrollingType, float Layer, float Speed)
            :base(containerWorld)
        {
            this.containerWorld = containerWorld;
            this.graphicTag = graphicTag;
            backgroundImage = (Graphic)AddComponent(new Graphic(this));
            backgroundImage.Layer = Layer;
            backgroundImage.GraphicTag = graphicTag;
            this.scrollingType = scrollingType;
            this.Speed = Speed;
            transform.Position = Position;
        }

        public void Update()
        {
            switch (scrollingType)
            {
                case ScrollingType.HorizontalScroll :
                    transform.Position += new Vector2(containerWorld.camera.Position.X * Speed, 0);
                    break;
                case ScrollingType.VerticalScroll:
                    transform.Position += new Vector2(0, containerWorld.camera.Position.Y * Speed);
                    break;
            }
        }
    }
}
