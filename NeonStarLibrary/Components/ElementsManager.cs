using NeonEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using NeonStarLibrary.Entities;

namespace NeonStarLibrary
{
    public enum ElementType
    {
        None, Fire, Steel, FireRing, Armor, Gun, GunLv2
    }

    public class ElementsManager : DrawableComponent
    {

        public Vector2 DrawPosition;

        public float ShotTimer = 0.0f;
        public float ShotDelay = 0.5f;

        public static Dictionary<ElementType[], ElementType> ElementsDictionary = new Dictionary<ElementType[], ElementType>();

        public ElementType MainElement = ElementType.None;
        public ElementType SecondElement = ElementType.None;

        public float FireRingRadius = 160f;

        public ElementsManager(Entity entity, Vector2 DrawPosition)
            : base(DrawLayer.HUD, entity, "ElementsManager")
        {
            this.DrawPosition = DrawPosition;
            ElementsDictionary.Add(new ElementType[2] { ElementType.Fire, ElementType.Fire }, ElementType.FireRing);
            ElementsDictionary.Add(new ElementType[2] { ElementType.Fire, ElementType.Steel }, ElementType.Gun);
            ElementsDictionary.Add(new ElementType[2] { ElementType.Fire, ElementType.Gun }, ElementType.GunLv2);
            ElementsDictionary.Add(new ElementType[2] { ElementType.Steel, ElementType.Steel }, ElementType.Armor);
            ElementsDictionary.Add(new ElementType[2] { ElementType.Steel, ElementType.Gun }, ElementType.Gun);
        }

        public override void Update(GameTime gameTime)
        {
            if (Neon.Input.Pressed(Buttons.LeftShoulder))
                this.SwitchElements();

            if (Neon.Input.Pressed(Buttons.RightShoulder))
                this.MixElements();

            if (Neon.Input.Pressed(Buttons.Y))
                this.DropElement();
            base.Update(gameTime);
        }

        public bool StockElement(ElementType elem)
        {
            if (MainElement == ElementType.None)
            {
                MainElement = elem;
                //entity.containerWorld.AddEntity(new Feedback(elem.ToString() + "Small", entity.Position - new Vector2(0, (entity as Avatar).hitbox.Height / 2), DrawLayer.Foreground6, SideDirection.Left, 1f, containerWorld, true));
                return true;
            }
            else if (SecondElement == ElementType.None)
            {             
                SecondElement = elem;
                //entity.containerWorld.AddEntity(new Feedback(elem.ToString() + "Small", entity.Position - new Vector2(0, (entity as Avatar).hitbox.Height / 2), DrawLayer.Foreground6, SideDirection.Left, 1f, containerWorld, true));
                return true;
            }
            
            return false;
        }

        public void DropElement()
        {
            MainElement = ElementType.None;
        }

        public void SwitchElements()
        {
            ElementType element = MainElement;
            MainElement = SecondElement;
            SecondElement = element;
        }

        public ElementType MixElements()
        {
            ElementType ElementToReturn = ElementType.None;
            if (MainElement != ElementType.None && SecondElement != ElementType.None)
            {
                foreach (KeyValuePair<ElementType[], ElementType> kvp in ElementsDictionary)
                {
                    if (MainElement == kvp.Key[0])
                    {
                        if (SecondElement == kvp.Key[1])
                        {
                            ElementToReturn = kvp.Value;
                            break;
                        }
                    }
                    else if (SecondElement == kvp.Key[0])
                        if (MainElement == kvp.Key[1])
                        {
                            ElementToReturn = kvp.Value;
                            break;
                        }
                }
                if (ElementToReturn != ElementType.None)
                {
                    SecondElement = ElementType.None;
                    MainElement = ElementType.None;
                    this.StockElement(ElementToReturn);
                }
            }
            return ElementToReturn;
        }

        public void UseElement()
        {
            switch(MainElement)
            {
                case ElementType.Armor:
                    (this.entity as Avatar).Armored = true;
                    MainElement = ElementType.None;
                    break;
                case ElementType.FireRing:
                    
                    if (!(entity as Avatar).isCrouched && (entity as Avatar).rigidbody.isGrounded)
                    {
                        MainElement = ElementType.None;
                        (entity as Avatar).Spritesheets.ChangeAnimation("FireRing", true, true);
                        for (int i = entity.containerWorld.entities.Count - 1; i >= 0; i--)
                        {
                            if (entity.containerWorld.entities[i] is Enemy)
                            {
                                Enemy e = (entity.containerWorld.entities[i] as Enemy);
                                if (Vector2.Distance(e.transform.Position, (this.entity as Avatar).transform.Position) < FireRingRadius)
                                {
                                    e.rigidbody.body.ApplyLinearImpulse(Vector2.Normalize(e.transform.Position - (this.entity as Avatar).transform.Position) * 8f);
                                    e.TakeDamage(50f, ElementType.Fire);
                                }
                            }
                        }
                    }
                    break;
                case ElementType.Gun:
                case ElementType.GunLv2:
                    if(!(entity as Avatar).isCrouched && (entity as Avatar).rigidbody.isGrounded && (entity as Avatar).rigidbody.body.LinearVelocity.X < 0.1f && (entity as Avatar).rigidbody.body.LinearVelocity.X > -0.1f)
                    {
                        if ((entity as Avatar).Spritesheets.CurrentSpritesheetName != "Shot")
                        {
                            (entity as Avatar).Spritesheets.ChangeAnimation("Shot", true, true);
                        }
                        else if ((entity as Avatar).Spritesheets.CurrentSpritesheet.currentFrame == 2)
                        {
                            if((entity as Avatar).currentAttackSide == SideDirection.Right)
                                entity.containerWorld.entities.Add(new Bullet(entity.transform.Position + new Vector2(75, -23), "GunShot", new Vector2(1, 0), entity.containerWorld));
                            else
                                entity.containerWorld.entities.Add(new Bullet(entity.transform.Position + new Vector2(-75, -23), "GunShot", new Vector2(-1, 0), entity.containerWorld));
                        }   
                    }
                    break;
            }
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            spriteBatch.Draw(AssetManager.GetTexture("ElementsManagerDressing"), DrawPosition - new Vector2(AssetManager.GetTexture("ElementsManagerDressing").Width / 2, AssetManager.GetTexture("ElementsManagerDressing").Height / 2), Color.White);
            if(this.MainElement != ElementType.None)
                spriteBatch.Draw(AssetManager.GetTexture(this.MainElement.ToString()), this.DrawPosition + new Vector2(3, -18), Color.White);
            if(this.SecondElement != ElementType.None)
                spriteBatch.Draw(AssetManager.GetTexture(this.SecondElement.ToString()), this.DrawPosition + new Vector2(-44, -18), Color.White);
        }


    }
}
