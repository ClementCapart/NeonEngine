using NeonEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary
{
    public enum DashSide
    {
        Left, Right
    }

    public class Dash : Component
    {
        private float dashTarget;
        private bool isDashing;
        private DashSide currentSide;

        private Rigidbody body;

        float DashDistance;

        public Dash(float DashDistance, Rigidbody body)
            :base(body.entity, "Dash")
        {
            this.body = body;
            this.DashDistance = DashDistance;
        }

        public override void Update(GameTime gameTime)
        {
            if (Neon.Input.Pressed(Buttons.LeftTrigger))
                StartDash(DashSide.Left);
            else if (Neon.Input.Pressed(Buttons.RightTrigger))
                StartDash(DashSide.Right);

            CheckDash();
            base.Update(gameTime);
        }

        public void StartDash(DashSide dashSide)
        {
            if (!isDashing)
            {
                currentSide = dashSide;
                isDashing = true;
                if (dashSide == DashSide.Left)
                {
                    dashTarget = entity.transform.Position.X - DashDistance;
                    body.body.LinearVelocity = new Vector2(-30f, 0);
                }
                else if (dashSide == DashSide.Right)
                {
                    dashTarget = entity.transform.Position.X + DashDistance;
                    body.body.LinearVelocity = new Vector2(30f, 0);
                }
            }
        }

        private void CheckDash()
        {
            if (isDashing)
                if ((currentSide == DashSide.Left && entity.transform.Position.X <= dashTarget) || (currentSide == DashSide.Right && entity.transform.Position.X >= dashTarget))
                {
                    body.body.LinearVelocity = Vector2.Zero;
                    isDashing = false;
                }
                else if (body.body.LinearVelocity.X < 0.5f && body.body.LinearVelocity.X > -0.5f)
                    isDashing = false;
        }
    }
}
