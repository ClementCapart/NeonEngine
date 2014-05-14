using Microsoft.Xna.Framework;
using NeonEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary.Components.Enemies
{
    public class GroundAttack : EnemyAttack
    {
        #region Properties
        private bool _cantAttackInAir = true;

        public bool CantAttackInAir
        {
            get { return _cantAttackInAir; }
            set { _cantAttackInAir = value; }
        }

        private bool _checkBothSide = true;

        public bool CheckBothSide
        {
            get { return _checkBothSide; }
            set { _checkBothSide = value; }
        }

        private Vector2 _detectionOffset = Vector2.Zero;

        public Vector2 DetectionOffset
        {
          get { return _detectionOffset; }
          set { _detectionOffset = value; }
        }
        #endregion

        public GroundAttack(Entity entity)
            :base(entity)
        {
            Name = "GroundAttack";
        }

        public override void PreUpdate(Microsoft.Xna.Framework.GameTime gameTime)
        { 
            if(EntityToAttack != null && (entity.rigidbody.isGrounded || !CantAttackInAir))
            {
                switch (EnemyComponent.State)
                {
                    case EnemyState.Chase:
                    case EnemyState.FinishChase:
                    case EnemyState.Wait:
                    case EnemyState.WaitNode:
                        if (_checkBothSide)
                        {
                            Entity[] hitEntities;
                            if (EnemyComponent.Chase != null)
                            {
                                hitEntities = EnemyComponent.UniqueRaycast(_detectionOffset, EnemyComponent.Chase.DetectionDistance, true);
                            }
                            else
                            {
                                hitEntities = EnemyComponent.UniqueRaycast(_detectionOffset, Attacks.Keys.Last(), true);
                            }


                            if (hitEntities[0] == EntityToAttack && Math.Abs(EntityToAttack.transform.Position.X - entity.transform.Position.X) <= Attacks.Keys.Last())
                            {
                                EnemyComponent.State = EnemyState.Attacking;
                                if(CanTurn)
                                    EnemyComponent.CurrentSide = Side.Right;
                            }
                            else if (hitEntities[1] == EntityToAttack && Math.Abs(EntityToAttack.transform.Position.X - entity.transform.Position.X) <= Attacks.Keys.Last())
                            {
                                EnemyComponent.State = EnemyState.Attacking;
                                if (CanTurn)
                                    EnemyComponent.CurrentSide = Side.Left;
                            }
                            else if (entity.hitboxes[0].hitboxRectangle.Intersects(EntityToAttack.hitboxes[0].hitboxRectangle))
                            {
                                EnemyComponent.State = EnemyState.Attacking;
                                if (CanTurn)
                                {
                                    if (entity.transform.Position.X < EntityToAttack.transform.Position.X)
                                        EnemyComponent.CurrentSide = Side.Right;
                                    else
                                        EnemyComponent.CurrentSide = Side.Left;
                                }
                            }
                        }
                        else
                        {
                            Entity[] hitEntities;
                            if (EnemyComponent.Chase != null)
                            {
                                hitEntities = EnemyComponent.UniqueRaycast(_detectionOffset, EnemyComponent.Chase.DetectionDistance, false);
                            }
                            else
                            {
                                hitEntities = EnemyComponent.UniqueRaycast(_detectionOffset, Attacks.Keys.Last(), false);
                            }
                            if (hitEntities[0] == EntityToAttack && Math.Abs(EntityToAttack.transform.Position.X - entity.transform.Position.X) <= Attacks.Keys.Last())
                            {
                                EnemyComponent.State = EnemyState.Attacking;
                            }
                            else if (entity.hitboxes[0].hitboxRectangle.Intersects(EntityToAttack.hitboxes[0].hitboxRectangle))
                            {
                                EnemyComponent.State = EnemyState.Attacking;
                                if (CanTurn)
                                {
                                    if (entity.transform.Position.X <= EntityToAttack.transform.Position.X)
                                        EnemyComponent.CurrentSide = Side.Right;
                                    else
                                        EnemyComponent.CurrentSide = Side.Left;
                                }
                            }
                        }
                        break;

                    case EnemyState.Idle:
                    case EnemyState.Patrol:
                        if (_checkBothSide)
                        {
                            Entity[] hitEntities;
                            if (EnemyComponent.Chase != null)
                            {
                                hitEntities = EnemyComponent.UniqueRaycast(_detectionOffset, EnemyComponent.Chase.DetectionDistance, true);
                            }
                            else
                            {
                                hitEntities = EnemyComponent.UniqueRaycast(_detectionOffset, Attacks.Keys.Last(), true);
                            }

                            if (hitEntities[0] == EntityToAttack && Math.Abs(EntityToAttack.transform.Position.X - entity.transform.Position.X) <= Attacks.Keys.Last())
                            {
                                EnemyComponent.State = EnemyState.WaitThreat;
                                if (CanTurn)
                                    EnemyComponent.CurrentSide = Side.Right;
                            }
                            else if (hitEntities[1] == EntityToAttack && Math.Abs(EntityToAttack.transform.Position.X - entity.transform.Position.X) <= Attacks.Keys.Last())
                            {
                                EnemyComponent.State = EnemyState.WaitThreat;
                                if (CanTurn)
                                    EnemyComponent.CurrentSide = Side.Left;
                            }
                            else if (entity.hitboxes[0].hitboxRectangle.Intersects(EntityToAttack.hitboxes[0].hitboxRectangle))
                            {
                                EnemyComponent.State = EnemyState.WaitThreat;
                                if (CanTurn)
                                {
                                    if (entity.transform.Position.X <= EntityToAttack.transform.Position.X)
                                        EnemyComponent.CurrentSide = Side.Right;
                                    else
                                        EnemyComponent.CurrentSide = Side.Left;
                                }
                            }
                        }
                        else
                        {
                            Entity[] hitEntities;
                            if (EnemyComponent.Chase != null)
                            {
                                hitEntities = EnemyComponent.UniqueRaycast(_detectionOffset, EnemyComponent.Chase.DetectionDistance, false);
                            }
                            else
                            {
                                hitEntities = EnemyComponent.UniqueRaycast(_detectionOffset, Attacks.Keys.Last(), false);
                            }
                            if (hitEntities[0] == EntityToAttack && Math.Abs(EntityToAttack.transform.Position.X - entity.transform.Position.X) <= Attacks.Keys.Last())
                            {
                                EnemyComponent.State = EnemyState.WaitThreat;
                            }
                            else if (entity.hitboxes[0].hitboxRectangle.Intersects(EntityToAttack.hitboxes[0].hitboxRectangle))
                            {
                                EnemyComponent.State = EnemyState.WaitThreat;
                                if (CanTurn)
                                {
                                    if (entity.transform.Position.X <= EntityToAttack.transform.Position.X)
                                        EnemyComponent.CurrentSide = Side.Right;
                                    else
                                        EnemyComponent.CurrentSide = Side.Left;
                                }
                            }
                        }
                        break;

                    case EnemyState.WaitThreat:
                        if (EnemyComponent.WaitThreatTimer > EnemyComponent.WaitThreatDuration)
                        {
                            if (_checkBothSide)
                            {
                                Entity[] hitEntities;
                                if (EnemyComponent.Chase != null)
                                {
                                    hitEntities = EnemyComponent.UniqueRaycast(_detectionOffset, EnemyComponent.Chase.DetectionDistance, true);
                                }
                                else
                                {
                                    hitEntities = EnemyComponent.UniqueRaycast(_detectionOffset, Attacks.Keys.Last(), true);
                                }

                                if (hitEntities[0] == EntityToAttack && Math.Abs(EntityToAttack.transform.Position.X - entity.transform.Position.X) <= Attacks.Keys.Last())
                                {
                                    EnemyComponent.State = EnemyState.Attacking;
                                    if (CanTurn)
                                        EnemyComponent.CurrentSide = Side.Right;
                                }
                                else if (hitEntities[1] == EntityToAttack && Math.Abs(EntityToAttack.transform.Position.X - entity.transform.Position.X) <= Attacks.Keys.Last())
                                {
                                    EnemyComponent.State = EnemyState.Attacking;
                                    if (CanTurn)
                                        EnemyComponent.CurrentSide = Side.Left;
                                }
                                else if (entity.hitboxes[0].hitboxRectangle.Intersects(EntityToAttack.hitboxes[0].hitboxRectangle))
                                {
                                    EnemyComponent.State = EnemyState.Attacking;
                                    if (CanTurn)
                                    {
                                        if (entity.transform.Position.X <= EntityToAttack.transform.Position.X)
                                            EnemyComponent.CurrentSide = Side.Right;
                                        else
                                            EnemyComponent.CurrentSide = Side.Left;
                                    }
                                }
                            }
                            else
                            {
                                Entity[] hitEntities;
                                if (EnemyComponent.Chase != null)
                                {
                                    hitEntities = EnemyComponent.UniqueRaycast(_detectionOffset, EnemyComponent.Chase.DetectionDistance, false);
                                }
                                else
                                {
                                    hitEntities = EnemyComponent.UniqueRaycast(_detectionOffset, Attacks.Keys.Last(), false);
                                }
                                if (hitEntities[0] == EntityToAttack && Math.Abs(EntityToAttack.transform.Position.X - entity.transform.Position.X) <= Attacks.Keys.Last())
                                {
                                    EnemyComponent.State = EnemyState.Attacking;
                                }
                                else if (entity.hitboxes[0].hitboxRectangle.Intersects(EntityToAttack.hitboxes[0].hitboxRectangle))
                                {
                                    EnemyComponent.State = EnemyState.Attacking;
                                    if (CanTurn)
                                    {
                                        if (entity.transform.Position.X <= EntityToAttack.transform.Position.X)
                                            EnemyComponent.CurrentSide = Side.Right;
                                        else
                                            EnemyComponent.CurrentSide = Side.Left;
                                    }
                                }
                            }
                        }
                        else if(_checkBothSide)
                        {
                            Entity[] hitEntities;
                            if (EnemyComponent.Chase != null)
                            {
                                hitEntities = EnemyComponent.UniqueRaycast(_detectionOffset, EnemyComponent.Chase.DetectionDistance, true);
                            }
                            else
                            {
                                hitEntities = EnemyComponent.UniqueRaycast(_detectionOffset, Attacks.Keys.Last(), true);
                            }

                            if (hitEntities[0] == EntityToAttack && Math.Abs(EntityToAttack.transform.Position.X - entity.transform.Position.X) <= Attacks.Keys.Last())
                            {
                                if (CanTurn)
                                    EnemyComponent.CurrentSide = Side.Right;
                            }
                            else if (hitEntities[1] == EntityToAttack && Math.Abs(EntityToAttack.transform.Position.X - entity.transform.Position.X) <= Attacks.Keys.Last())
                            {
                                if (CanTurn)
                                    EnemyComponent.CurrentSide = Side.Left;
                            }
                            else if (entity.hitboxes[0].hitboxRectangle.Intersects(EntityToAttack.hitboxes[0].hitboxRectangle))
                            {
                                if (CanTurn)
                                {
                                    if (entity.transform.Position.X < EntityToAttack.transform.Position.X)
                                        EnemyComponent.CurrentSide = Side.Right;
                                    else
                                        EnemyComponent.CurrentSide = Side.Left;
                                }
                            }
                            else if (EnemyComponent.Chase == null)
                                EnemyComponent.State = EnemyState.Idle;
                        }
                        else
                        {
                            Entity[] hitEntities;
                            if (EnemyComponent.Chase != null)
                            {
                                hitEntities = EnemyComponent.UniqueRaycast(_detectionOffset, EnemyComponent.Chase.DetectionDistance, false);
                            }
                            else
                            {
                                hitEntities = EnemyComponent.UniqueRaycast(_detectionOffset, Attacks.Keys.Last(), false);
                            }
                            if (hitEntities[0] != EntityToAttack || Math.Abs(EntityToAttack.transform.Position.X - entity.transform.Position.X) > _rangeForAttackOne && !entity.hitboxes[0].hitboxRectangle.Intersects(EntityToAttack.hitboxes[0].hitboxRectangle))
                            {
                                if (EnemyComponent.Chase == null)
                                    EnemyComponent.State = EnemyState.Idle;
                            }
                        }
                        break;
                }
            }
            
            base.PreUpdate(gameTime);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
            if (CurrentAttack != null)
            {
                CurrentAttack.Update(gameTime);
                if (CurrentAttack != null && CurrentAttack.CooldownFinished)
                {
                    CurrentAttack = null;
                    EnemyComponent.State = EnemyState.Wait;
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
            
            
        }

        public override void PostUpdate(GameTime gameTime)
        {
            if (EnemyComponent.State != EnemyState.Dying && EnemyComponent.State != EnemyState.Dead)
            {
                if ((CurrentAttack == null && EnemyComponent.State == EnemyState.Attacking) || !entity.rigidbody.isGrounded)
                    EnemyComponent.State = EnemyState.Wait;

                if (EnemyComponent.State != EnemyState.Attacking && CurrentAttack != null)
                {
                    CurrentAttack.CancelAttack();
                    CurrentAttack = null;
                }
            }
            
            base.PostUpdate(gameTime);
        }

        private void ChooseAttack()
        {
            foreach (KeyValuePair<float, Dictionary<string, float>> kvp in Attacks)
            {
                if (Math.Abs(entity.transform.Position.X - EntityToAttack.transform.Position.X) <= kvp.Key)
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
                                for (int j = kvp.Value.Count - 1; j >= 0; j--)
                                    kvp.Value[kvp.Value.ElementAt(j).Key] = normalRatio;
                            }

                            if (kvp.Value.Count > 1)
                            {
                                float chanceReduction = _randomBalanceRate;
                                float chanceeIncrease = _randomBalanceRate / (kvp.Value.Count - 1);

                                for (int j = kvp.Value.Count - 1; j >= 0; j--)
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
