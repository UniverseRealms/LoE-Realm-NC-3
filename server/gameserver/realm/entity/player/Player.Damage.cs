﻿#region

using LoESoft.GameServer.networking.outgoing;
using System;

#endregion

namespace LoESoft.GameServer.realm.entity.player
{
    partial class Player
    {
        public void ForceHit(int dmg, Entity chr, bool NoDef)
        {
            if (chr != null)
                Damage(dmg, chr, NoDef);
        }

        public void Damage(int dmg, Entity chr, bool NoDef, bool manaDrain = false)
        {
            if (manaDrain)
            {
                try
                {
                    if (HasConditionEffect(ConditionEffectIndex.Paused))
                        return;

                    MP -= dmg;

                    UpdateCount++;

                    Owner.BroadcastMessage(new NOTIFICATION
                    {
                        ObjectId = Id,
                        Text = "{\"key\":\"blank\",\"tokens\":{\"data\":\"-" + dmg + " MP\"}}",
                        Color = new ARGB(0x9B30FF)
                    }, null);

                    SaveToCharacter();
                }
                catch (Exception) { }
            }
            else
            {
                try
                {
                    if (HasConditionEffect(ConditionEffectIndex.Paused) ||
                        HasConditionEffect(ConditionEffectIndex.Stasis) ||
                        HasConditionEffect(ConditionEffectIndex.Invincible) ||
                        HasConditionEffect(ConditionEffectIndex.Invulnerable))
                        return;

                    dmg = (int)StatsManager.GetDefenseDamage(dmg, NoDef);
                    HP -= dmg;

                    Owner.BroadcastMessage(new DAMAGE
                    {
                        TargetId = Id,
                        Effects = 0,
                        Damage = (ushort)dmg,
                        Killed = HP <= 0,
                        BulletId = 0,
                        ObjectId = chr.Id
                    }, this);

                    UpdateCount++;

                    Client.Character.HP = HP;

                    if (HP <= 0)
                        Death(chr.ObjectDesc.DisplayId, chr.ObjectDesc);
                }
                catch (Exception) { }
            }
        }

        public override bool HitByProjectile(Projectile projectile, RealmTime time)
        {
            if (projectile.ProjectileOwner is Player ||
                HasConditionEffect(ConditionEffectIndex.Paused) ||
                HasConditionEffect(ConditionEffectIndex.Stasis) ||
                HasConditionEffect(ConditionEffectIndex.Invincible))
                return false;

            return base.HitByProjectile(projectile, time);
        }
    }
}