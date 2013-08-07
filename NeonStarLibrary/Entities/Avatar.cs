using FarseerPhysics.Dynamics;
using NeonEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using NeonStarLibrary.Entities;

namespace NeonStarLibrary
{
    public delegate void ChangedBody(Body body, EventArgs e);

    public class Avatar : Entity
    {
        public event ChangedBody ChangedItsBody;
        Dash dash;
        public Fight fight;

        public SpriteSheet GunAnim;
        public SpriteSheet FireRingAnim;


        public SpritesheetManager Spritesheets;

        float DistanceShaderParameters = 0.001f;
        bool ShaderInverse;
        bool IsTakingDamages;

        public bool NoControl = false;

        List<Rigidbody> ignoredGeometry = new List<Rigidbody>();

        public bool Armored = false;
        public SpriteSheet ShieldFront;
        public SpriteSheet ShieldBack;
        bool ShieldAdded;

        public SideDirection currentAttackSide;

        private float lifePoints = 7;
        public float LifePoints
        {
            get { return lifePoints; }
            set { lifePoints = value; }
        }

        public ElementType MainElement
        {
            get { return elementsManager.MainElement; }
            set { elementsManager.MainElement = value; }
        }

        public ElementType SecondElement
        {
            get { return elementsManager.SecondElement; }
            set { elementsManager.SecondElement = value; }
        }

        
        public ElementsManager elementsManager;

        public bool isCrouched = false;

        public Avatar(NeonEngine.World currentWorld, Vector2 Position)
            :base(currentWorld)        
        {
            this.transform.Position = Position;
            hitbox = AddComponent(new Hitbox(this));
            hitbox.Width = 40;
            hitbox.Height = 136;
            hitbox.Init();
            fight = AddComponent(new Fight(hitbox, new Rectangle(35, 0, 40, 60), this));
            
            rigidbody = AddComponent(new Rigidbody(this));
            rigidbody.BodyType = BodyType.Dynamic;
            rigidbody.Hitbox = hitbox;
            rigidbody.Sensors = true;
            rigidbody.Init();
            rigidbody.body.FixedRotation = true;
            rigidbody.Friction = 10f;
            rigidbody.body.Mass = 0.5f;
            dash = (Dash)AddComponent(new Dash(200, rigidbody));

            AddComponent(new LifeBar(this));
            elementsManager = (ElementsManager)AddComponent(new ElementsManager(this, new Vector2(110, 100)));

            Spritesheets = new SpritesheetManager(this);
            AddComponent(Spritesheets);
            Spritesheets.DrawLayer = 0.5f;

            Dictionary<string, SpriteSheet> spritesheetList = new Dictionary<string, SpriteSheet>();
            spritesheetList.Add("Idle", new SpriteSheet(AssetManager.GetSpriteSheet("LiOnIdle"), 0.5f, this));
            spritesheetList.Add("Run", new SpriteSheet(AssetManager.GetSpriteSheet("LiOnWalk"), 0.5f, this));
            spritesheetList.Add("Jump", new SpriteSheet(AssetManager.GetSpriteSheet("LiOnJump"), 0.5f, this));
            spritesheetList.Add("Shot", new SpriteSheet(AssetManager.GetSpriteSheet("LiOnGun"), 0.5f, this));
            spritesheetList.Add("Kick01", new SpriteSheet(AssetManager.GetSpriteSheet("LiOnKick1"), 0.5f, this));
            spritesheetList.Add("Kick02", new SpriteSheet(AssetManager.GetSpriteSheet("LiOnKick2"), 0.5f, this));
            spritesheetList.Add("FireRing", new SpriteSheet(AssetManager.GetSpriteSheet("LiOnFireRing"), 0.5f, this));

            Spritesheets.Spritesheets = spritesheetList;
           

            rigidbody.body.SleepingAllowed = false;

            ShieldFront = new SpriteSheet(AssetManager.GetSpriteSheet("ShieldEffectFront"), 0.4f, this);
            ShieldFront.Init();
            ShieldBack = new SpriteSheet(AssetManager.GetSpriteSheet("ShieldEffectBack"), 0.6f, this);
            ShieldBack.Init();


            
           

            //AddComponent(new HitboxRenderer(fight));
        }

        public override void Update(GameTime gameTime)
        {
            if (!Armored)
            {
                if (ShieldAdded)
                {
                    ShieldFront.Remove();
                    ShieldBack.Remove();
                    ShieldAdded = false;
                }
            }
            else if (!ShieldAdded)
            {
                AddComponent(ShieldFront);
                AddComponent(ShieldBack);
                ShieldAdded = true;
            }
            
            foreach (Rigidbody rg in ignoredGeometry)
                this.rigidbody.body.RestoreCollisionWith(rg.body);

            ignoredGeometry.Clear();

            if (!NoControl)
            {
                if (!fight.isHitting)
                {
                    if (Neon.Input.Check(Buttons.LeftThumbstickLeft) && rigidbody.isGrounded && rigidbody.body.LinearVelocity.X > -4)
                    {
                        rigidbody.body.LinearVelocity += new Vector2(-0.5f, 0);
                        if (Spritesheets.CurrentSpritesheetName != "Run")
                            Spritesheets.ChangeAnimation("Run");
                        currentAttackSide = SideDirection.Left;
                        Spritesheets.CurrentSpritesheet.spriteEffects = SpriteEffects.FlipHorizontally;
                    }
                    else if (Neon.Input.Check(Buttons.LeftThumbstickRight) && rigidbody.isGrounded && rigidbody.body.LinearVelocity.X < 4)
                    {
                        rigidbody.body.LinearVelocity += new Vector2(0.5f, 0);
                        if (Spritesheets.CurrentSpritesheetName != "Run" && (Spritesheets.CurrentSpritesheetName != "Jump" || rigidbody.body.LinearVelocity.Y > -0.1f))
                            Spritesheets.ChangeAnimation("Run");
                        currentAttackSide = SideDirection.Right;
                        Spritesheets.CurrentSpritesheet.spriteEffects = SpriteEffects.None;
                    }
                    else
                    {
                        if (rigidbody.isGrounded && (rigidbody.body.LinearVelocity.X < 0.1f && rigidbody.body.LinearVelocity.X > -0.1f) && (Spritesheets.CurrentSpritesheetName != "FireRing" || (Spritesheets.CurrentSpritesheetName == "FireRing" && Spritesheets.CurrentSpritesheet.currentFrame == Spritesheets.CurrentSpritesheet.spriteSheetInfo.FrameCount - 1))
                        && (Spritesheets.CurrentSpritesheetName != "Shot" || (Spritesheets.CurrentSpritesheetName == "Shot" && Spritesheets.CurrentSpritesheet.currentFrame == Spritesheets.CurrentSpritesheet.spriteSheetInfo.FrameCount - 1)) && Spritesheets.CurrentSpritesheetName != "Idle")
                            Spritesheets.ChangeAnimation("Idle");
                    }

                    if (Neon.Input.Check(Buttons.LeftThumbstickLeft) && !rigidbody.isGrounded && rigidbody.body.LinearVelocity.X > -3)
                    {
                        rigidbody.body.LinearVelocity += new Vector2(-0.3f, 0);
                        currentAttackSide = SideDirection.Left;
                        Spritesheets.CurrentSpritesheet.spriteEffects = SpriteEffects.FlipHorizontally;
                    }
                    else if (Neon.Input.Check(Buttons.LeftThumbstickRight) && !rigidbody.isGrounded && rigidbody.body.LinearVelocity.X < 3)
                    {
                        rigidbody.body.LinearVelocity += new Vector2(0.3f, 0);
                        currentAttackSide = SideDirection.Right;
                        Spritesheets.CurrentSpritesheet.spriteEffects = SpriteEffects.None;
                    }
                    if (Neon.Input.Check(Buttons.A))
                    {
                        if (Neon.Input.Check(Buttons.LeftThumbstickDown))
                            GoDown();
                        else
                            if (Neon.Input.Pressed(Buttons.A) && rigidbody.isGrounded && !isCrouched)
                                rigidbody.body.LinearVelocity += new Vector2(0, -6f);
                    }
                         

                    if (!rigidbody.isGrounded && rigidbody.body.LinearVelocity.X > 3.5f)
                        rigidbody.body.LinearVelocity -= new Vector2(0.3f, 0);
                    else if (!rigidbody.isGrounded && rigidbody.body.LinearVelocity.X < -3.5f)
                        rigidbody.body.LinearVelocity += new Vector2(0.3f, 0);
                           

                    if (Neon.Input.Pressed(Buttons.X) && rigidbody.isGrounded)
                    {
                        Spritesheets.ChangeAnimation((new Random().Next(0, 2) == 0 ? "Kick01" : "Kick02"));
                        //fight.StartHit(currentAttackSide);
                    }

                   if(elementsManager != null)
                        if (Neon.Input.Check(Buttons.B))
                            elementsManager.UseElement();
  
                }
            }

            base.Update(gameTime);
            
            if (rigidbody.isGrounded)
                rigidbody.body.LinearDamping = 0.05f;
            else
                rigidbody.body.LinearDamping = 0.05f;

            if(Neon.Input.Pressed(Keys.Space))
                TakeDamage(5);

            if (IsTakingDamages)
            {
                if (!ShaderInverse)
                {
                    DistanceShaderParameters += 0.010f;
                    containerWorld.ScreenEffect.Parameters["Distance"].SetValue(DistanceShaderParameters);
                    if (DistanceShaderParameters > 0.03)
                        ShaderInverse = true;
                }
                else if (DistanceShaderParameters > 0f)
                {
                    DistanceShaderParameters -= 0.007f;
                    containerWorld.ScreenEffect.Parameters["Distance"].SetValue(DistanceShaderParameters);
                }
                else
                {
                    ShaderInverse = false;
                    DistanceShaderParameters = 0.001f;
                    containerWorld.ScreenEffect = null;
                    IsTakingDamages = false;
                }
            }

            if(!rigidbody.isGrounded)
                if (rigidbody.body.LinearVelocity.Y < -0.1f)
                {
                    if (Spritesheets.CurrentSpritesheetName != "Jump")
                        Spritesheets.ChangeAnimation("Jump", false, true);
                    Spritesheets.CurrentSpritesheet.SetFrame(0);
                }
                else if (rigidbody.body.LinearVelocity.Y > 0.1f)
                {
                    if (Spritesheets.CurrentSpritesheetName != "Jump")
                        Spritesheets.ChangeAnimation("Jump", false, false, false, 1);
                    Spritesheets.CurrentSpritesheet.SetFrame(0);
                }

            foreach (Rigidbody rg in ignoredGeometry)
                this.rigidbody.body.IgnoreCollisionWith(rg.body);
        }

        private void GoDown()
        {
            Rigidbody rg = rigidbody.beacon.CheckGround();

            if (rg != null)
                if (rg.OneWayPlatform)
                    this.ignoredGeometry.Add(rg);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public void TakeDamage(int damage)
        {
            containerWorld.ScreenEffect = AssetManager.GetEffect("ChromaticAberration");
            containerWorld.ScreenEffect.Parameters["Distance"].SetValue(DistanceShaderParameters);

            IsTakingDamages = true;
            if (!Armored)
                LifePoints -= damage;
            else
                Armored = false;
        }

        public void Crouch()
        {
            if (!isCrouched)
            {
                isCrouched = true;
                Vector2 CurrentVelocity = rigidbody.body.LinearVelocity;
                hitbox.Height /= 2;
                hitbox.Y += hitbox.Height;
                rigidbody.GenerateNewBody(hitbox);
                rigidbody.Friction = 10f;
                rigidbody.body.FixedRotation = true;
                rigidbody.body.SleepingAllowed = false;
                rigidbody.body.LinearVelocity = CurrentVelocity;
                if (ChangedItsBody != null)
                    ChangedItsBody(rigidbody.body, null);
            }

        }

        public void Stand()
        {
            if (isCrouched)
            {
                isCrouched = false;
                Vector2 CurrentVelocity = rigidbody.body.LinearVelocity;
                hitbox.Y -= hitbox.Height;
                hitbox.Height *= 2;
                rigidbody.GenerateNewBody(hitbox);
                rigidbody.Friction = 10f;
                rigidbody.body.FixedRotation = true;
                rigidbody.body.SleepingAllowed = false;
                rigidbody.body.LinearVelocity = CurrentVelocity;
                if(ChangedItsBody != null)
                    ChangedItsBody(rigidbody.body, null);
            }
        }
    }
}
