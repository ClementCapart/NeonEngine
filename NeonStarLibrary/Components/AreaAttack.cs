using Microsoft.Xna.Framework;
using NeonEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary.Components.Enemies
{
    public class AreaAttack : EnemyAttack
    {
        #region Properties
        private float _maximumAngleDifference = 360.0f;

        public float MaximumAngleDifference
        {
            get { return _maximumAngleDifference; }
            set { _maximumAngleDifference = value; }
        }
        #endregion

        public AreaAttack(Entity entity)
            :base(entity)
        {
            Name = "AreaAttack";
        }

        public override void PreUpdate(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (EntityToAttack != null)
            {
                switch(EnemyComponent.State)
                {
                    case EnemyState.Chase:
                    case EnemyState.FinishChase:
                    case EnemyState.Wait:
                    case EnemyState.WaitNode:
                        if (CheckIfEntityIsInRange())
                        {
                            EnemyComponent.State = EnemyState.Attacking;
                            if (CanTurn)
                            {
                                if (EntityToAttack.transform.Position.X <= entity.transform.Position.X)
                                    EnemyComponent.CurrentSide = Side.Left;
                                else
                                    EnemyComponent.CurrentSide = Side.Right;
                            }
                        }
                        break;

                    case EnemyState.Idle:
                    case EnemyState.Patrol:
                        if (CheckIfEntityIsInRange())
                        {
                            EnemyComponent.State = EnemyState.WaitThreat;
                            if (CanTurn)
                            {
                                if (EntityToAttack.transform.Position.X <= entity.transform.Position.X)
                                    EnemyComponent.CurrentSide = Side.Left;
                                else
                                    EnemyComponent.CurrentSide = Side.Right;
                            }
                            
                        }
                        break;

                    case EnemyState.WaitThreat:
                        if (CheckIfEntityIsInRange())
                        {
                            if (EnemyComponent.WaitThreatTimer >= EnemyComponent.WaitThreatDuration)
                            {
                                EnemyComponent.State = EnemyState.Attacking;
                            }
                            if (CanTurn)
                            {
                                if (EntityToAttack.transform.Position.X <= entity.transform.Position.X)
                                    EnemyComponent.CurrentSide = Side.Left;
                                else
                                    EnemyComponent.CurrentSide = Side.Right;
                            }
                        }
                        else
                        {
                            if (EnemyComponent.Chase != null) EnemyComponent.State = EnemyState.Idle;
                        }
                        break;
                }
            }
            base.PreUpdate(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            if (CurrentAttack != null)
            {
                CurrentAttack.Update(gameTime);
                if (CurrentAttack.CooldownFinished)
                {
                    CurrentAttack = null;
                }
            }
            else
            {
                switch (EnemyComponent.State)
                {
                    case EnemyState.Attacking:
                        ChooseAttack();
                        break;
                }
            }
            base.Update(gameTime);
        }

        public override void PostUpdate(GameTime gameTime)
        {
            if (CurrentAttack == null && EnemyComponent.State == EnemyState.Attacking)
                EnemyComponent.State = EnemyState.Wait;

            if (EnemyComponent.State != EnemyState.Attacking && CurrentAttack != null)
            {
                CurrentAttack.CancelAttack();
                CurrentAttack = null;
            }
            base.PostUpdate(gameTime);
        }

        public bool CheckIfEntityIsInRange()
        {
            float distanceSquared = Vector2.DistanceSquared(EntityToAttack.transform.Position, entity.transform.Position);
            if (distanceSquared <= _attacks.Keys.Last() * _attacks.Keys.Last())
            {
                float angle = (Neon.Utils.AngleBetween(entity.transform.Position, EntityToAttack.transform.Position) - entity.transform.rotation);
                if (angle < 0)
                    angle = angle + (float)Math.PI * 2;
                while (angle > Math.PI * 2)
                    angle = angle - (float)Math.PI * 2;
                float lowerAngle = (-_maximumAngleDifference) % 360;
                lowerAngle = lowerAngle < 0 ? lowerAngle + 360 : lowerAngle;
                lowerAngle = MathHelper.ToRadians(lowerAngle);
                if (lowerAngle <= angle && Math.PI * 2 >= angle)
                    return true;
            }

            return false;
        }

        public void ChooseAttack()
        {
            foreach (KeyValuePair<float, List<string>> kvp in _attacks)
            {
                if (Vector2.DistanceSquared(entity.transform.Position, EntityToAttack.transform.Position) <= kvp.Key * kvp.Key)
                {
                    string selectedAttack = kvp.Value[Neon.Utils.CommonRandom.Next(kvp.Value.Count)];

                    if (selectedAttack == "Chase")
                    {
                        EnemyComponent.State = EnemyState.Chase;
                        return;
                    }
                    else
                    {
                        CurrentAttack = null;

                        bool inLocalCooldown = false;
                        foreach (Attack a in LocalAttacksInCooldown)
                        {
                            if (a.Name == selectedAttack)
                                inLocalCooldown = true;
                        }

                        if (!inLocalCooldown)
                            CurrentAttack = AttacksManager.GetAttack(selectedAttack, EnemyComponent.CurrentSide, entity, EntityToAttack, true);

                        return;
                    }
                }
            }
        }


    }


}
