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
            if (EntityToAttack != null && EnemyComponent != null)
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
                            if (EnemyComponent.Chase == null) EnemyComponent.State = EnemyState.Idle;
                        }
                        break;
                }
            }
            base.PreUpdate(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (CurrentAttack != null)
            {
                CurrentAttack.Update(gameTime);
                
                if (CurrentAttack != null && CurrentAttack.CooldownFinished)
                {
                    CurrentAttack = null;
                }
            }
            else if(EnemyComponent != null)
            {
                switch (EnemyComponent.State)
                {
                    case EnemyState.Attacking:
                        ChooseAttack();
                        break;
                }
            }
            
        }

        public override void PostUpdate(GameTime gameTime)
        {
            if (CurrentAttack == null && EnemyComponent != null && EnemyComponent.State == EnemyState.Attacking)
                EnemyComponent.State = EnemyState.Wait;

            if (EnemyComponent != null && EnemyComponent.State != EnemyState.Attacking && CurrentAttack != null)
            {
                CurrentAttack.CancelAttack();
                CurrentAttack = null;
            }
            base.PostUpdate(gameTime);
        }

        public bool CheckIfEntityIsInRange()
        {
            float distanceSquared = Vector2.DistanceSquared(EntityToAttack.transform.Position, entity.transform.Position);
            if (distanceSquared <= Attacks.Keys.Last() * Attacks.Keys.Last())
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
            foreach (KeyValuePair<float, Dictionary<string, float>> kvp in Attacks)
            {
                if (Vector2.DistanceSquared(entity.transform.Position, EntityToAttack.transform.Position) <= kvp.Key * kvp.Key)
                {
                    string selectedAttack = "";
                    double random = Neon.Utils.GetRandomNumber(0.0f, 100.0f);

                    int i = 0;
                    float excludedPart = 0.0f;

                    while (i < kvp.Value.Count)
                    {
                        if (random > excludedPart && random < excludedPart + kvp.Value.ElementAt(i).Value)
                        {
                            selectedAttack = kvp.Value.ElementAt(i).Key;
                            
                            if (selectedAttack != _lastAttackLaunched)
                            {
                                float normalRatio = 100.0f / kvp.Value.Count;
                                for(int j = kvp.Value.Count - 1; j >= 0; j --)
                                    kvp.Value[kvp.Value.ElementAt(j).Key] = normalRatio;
                            }
                            
                            if (kvp.Value.Count > 1)
                            {
                                float chanceReduction = _randomBalanceRate;
                                float chanceeIncrease = _randomBalanceRate / (kvp.Value.Count - 1);

                                for(int j = kvp.Value.Count - 1; j >= 0; j --)
                                {
                                    KeyValuePair<string, float> kvp2 = kvp.Value.ElementAt(j);
                                    if (kvp2.Key == selectedAttack)
                                        kvp.Value[kvp2.Key] = Math.Max(0, kvp2.Value - chanceReduction);
                                    else
                                        kvp.Value[kvp2.Key] = Math.Min(100.0f / (kvp.Value.Count - 1), kvp2.Value + chanceeIncrease);
                                }
                            }

                            _lastAttackLaunched = selectedAttack;
                            break;
                        }
                        else
                        {                          
                            excludedPart += kvp.Value.ElementAt(i).Value;
                            i++;
                        }
                    }

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
