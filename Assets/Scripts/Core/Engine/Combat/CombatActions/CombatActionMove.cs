﻿using System;
using System.Collections.Generic;
using Character.Base;
using Character.Class;
using Combat;
using Core.Domain.Character.Moves;
using UnityEngine;
using static Character.Base.CharacterDefinition;
using static Core.Domain.Character.Moves.CharacterMove;
using Random = UnityEngine.Random;

namespace Core.Engine.Combat.CombatActions
{
    public class CombatActionMove : ICombatAction
    {
        public static event Action<string> OnMoveHistory;

        private List<BaseCharacter> _targets;

        public void Execute(BaseCharacter user, CharacterMove move, List<BaseCharacter> targets)
        {
            _targets = targets;

            switch (move.Category)
            {
                case MoveCategory.PHYSICAL:
                    DoPhysicalMove(user, move, targets);
                    break;
                case MoveCategory.MAGICAL:
                    DoMagicalMove(user, move, targets);
                    break;
                case MoveCategory.BUFF:
                    DoStatusMove(user, move, targets);
                    break;
                case MoveCategory.DEBUFF:
                    DoStatusMove(user, move, targets);
                    break;
                case MoveCategory.HEAL:
                    DoHealMove(user, move, targets);
                    break;
            }
        }

        private void DoHealMove(BaseCharacter user, CharacterMove move, List<BaseCharacter> targets)
        {
            var cachedBaseRotation = user.transform.rotation;

            var miss = Random.Range(0, 100) > move.Accuracy;
            if (miss)
            {
                foreach (var target in targets)
                {
                    var vfx = move.SpawnVfxOnTarget ? move.VfxPrefab : null;
                    target.CharacterStats.ReceiveHeal(0, vfx);
                    if (!move.SpawnVfxOnTarget)
                        user.SpawnVfx(move.VfxPrefab);

                    if (target == user)
                        user.RotateTo(cachedBaseRotation);
                    else
                        user.RotateTo(target.transform);

                    OnMoveHistory?.Invoke($"{user.Name} missed {move.MoveName} on {target.Name}.");
                }
                return;
            }

            foreach (var target in targets)
            {
                if (target == user)
                    user.RotateTo(cachedBaseRotation);
                else
                    user.RotateTo(target.transform);

                var modifier = TypeModifier(move.Type, target.Type, out string effectiveComplement);
                var heal = ((1 - target.Intelligence / 100f) + move.Power) * modifier * (1 - target.Intelligence / 100f);

                Debug.Log($"Heal: ((1 - {target.Intelligence} / 100) + {move.Power}) * {modifier} * (1 - {target.Intelligence} / 100) = {heal}");

                var vfx = move.SpawnVfxOnTarget ? move.VfxPrefab : null;
                target.CharacterStats.ReceiveHeal(Mathf.FloorToInt(heal), vfx);
                if (!move.SpawnVfxOnTarget)
                    user.SpawnVfx(move.VfxPrefab);

                OnMoveHistory?.Invoke($"{user.Name} used {move.MoveName} on {target.Name}. {effectiveComplement}");

                var statusTarget = move.Category == MoveCategory.DEBUFF ? targets[0] : user;
                ImproveStats(statusTarget, move);
            }
        }

        private void DoPhysicalMove(BaseCharacter user, CharacterMove move, List<BaseCharacter> targets)
        {
            var cachedBaseRotation = user.transform.rotation;

            var miss = Random.Range(0, 100) > move.Accuracy;
            if (miss)
            {
                foreach (var target in targets)
                {
                    var vfx = move.SpawnVfxOnTarget ? move.VfxPrefab : null;
                    target.CharacterStats.ReceiveDamage(0, vfx);
                    if (!move.SpawnVfxOnTarget)
                        user.SpawnVfx(move.VfxPrefab);

                    if (target == user)
                        user.RotateTo(cachedBaseRotation);
                    else
                        user.RotateTo(target.transform);

                    OnMoveHistory?.Invoke($"{user.Name} missed {move.MoveName} on {target.Name}.");
                }
                return;
            }

            foreach (var target in targets)
            {
                if (target == user)
                    user.RotateTo(cachedBaseRotation);
                else
                    user.RotateTo(target.transform);

                var modifier = TypeModifier(move.Type, target.Type, out string effectiveComplement);
                var damage = ((1 - target.Attack / 100f) + move.Power) * modifier * (1 - target.Defense / 100f);

                Debug.Log($"Damage: ((1 - {target.Attack} / 100) + {move.Power}) * {modifier} * (1 - {target.Defense} / 100) = {damage}");

                var vfx = move.SpawnVfxOnTarget ? move.VfxPrefab : null;
                target.CharacterStats.ReceiveDamage(Mathf.FloorToInt(damage), vfx);
                if (!move.SpawnVfxOnTarget)
                    user.SpawnVfx(move.VfxPrefab);

                OnMoveHistory?.Invoke($"{user.Name} used {move.MoveName} on {target.Name}. {effectiveComplement}");

                var statusTarget = move.Category == MoveCategory.DEBUFF ? targets[0] : user;
                ImproveStats(statusTarget, move);
            }
        }

        private void DoMagicalMove(BaseCharacter user, CharacterMove move, List<BaseCharacter> targets)
        {
            var cachedBaseRotation = user.transform.rotation;

            var miss = Random.Range(0, 100) > move.Accuracy;
            if (miss)
            {
                foreach (var target in targets)
                {
                    var vfx = move.SpawnVfxOnTarget ? move.VfxPrefab : null;
                    target.CharacterStats.ReceiveDamage(0, vfx);
                    if (!move.SpawnVfxOnTarget)
                        user.SpawnVfx(move.VfxPrefab);

                    if (target == user)
                        user.RotateTo(cachedBaseRotation);
                    else
                        user.RotateTo(target.transform);

                    OnMoveHistory?.Invoke($"{user.Name} missed {move.MoveName} on {target.Name}.");
                }
                return;
            }

            foreach (var target in targets)
            {
                if (target == user)
                    user.RotateTo(cachedBaseRotation);
                else
                    user.RotateTo(target.transform);

                var modifier = TypeModifier(move.Type, target.Type, out string effectiveComplement);
                var damage = ((1 - target.Intelligence / 100f) + move.Power) * modifier * (1 - target.Intelligence / 100f);

                Debug.Log($"Damage: ((1 - {target.Intelligence} / 100) + {move.Power}) * {modifier} * (1 - {target.Intelligence} / 100) = {damage}");

                var vfx = move.SpawnVfxOnTarget ? move.VfxPrefab : null;
                target.CharacterStats.ReceiveDamage(Mathf.FloorToInt(damage), vfx);
                if (!move.SpawnVfxOnTarget)
                    user.SpawnVfx(move.VfxPrefab);

                OnMoveHistory?.Invoke($"{user.Name} used {move.MoveName} on {target.Name}. {effectiveComplement}");

                var statusTarget = move.Category == MoveCategory.DEBUFF ? targets[0] : user;
                ImproveStats(statusTarget, move);
            }
        }

        private void DoStatusMove(BaseCharacter user, CharacterMove move, List<BaseCharacter> targets)
        {
            var miss = Random.Range(0, 100) > move.Accuracy;
            if (miss) return;

            var statusTarget = move.Category == MoveCategory.DEBUFF ? targets[0] : user;
            ImproveStats(statusTarget, move);
        }

        private void ImproveStats(BaseCharacter user, CharacterMove move)
        {
            if (move.StatsToImprove == null || move.StatsToImprove.Count == 0)
                return;

            string improvedLog = "";
            foreach (var stat in move.StatsToImprove)
            {
                string modifier = stat.ValueToImprove > 0 ? "+" : "-";
                improvedLog += $"{stat.StatToImprove} {modifier}{stat.ValueToImprove} \n";

                switch (stat.StatToImprove)
                {
                    case StatsCanImprove.Attack:
                        user.CharacterStats.ImproveAttack(stat.ValueToImprove, stat.VfxPrefab);
                        break;
                    case StatsCanImprove.Defense:
                        user.CharacterStats.ImproveDefense(stat.ValueToImprove, stat.VfxPrefab);
                        break;
                    case StatsCanImprove.Speed:
                        user.CharacterStats.ImproveSpeed(stat.ValueToImprove, stat.VfxPrefab);
                        break;
                    case StatsCanImprove.Intelligence:
                        user.CharacterStats.ImproveIntelligence(stat.ValueToImprove, stat.VfxPrefab);
                        break;
                }

                string action = move.Category == MoveCategory.BUFF ? "increased" : "decreased";
                string value = stat.ValueToImprove > 60 ? "drastically" : stat.ValueToImprove > 35 ? "significantly" : "slightly";
                OnMoveHistory?.Invoke($"{user.Name} {stat.StatToImprove.ToString().ToLower()} {value} {action}.");
            }

            Debug.Log($"Stats improved: {improvedLog}");
        }

        private float TypeModifier(CharacterTypes moveUsed, CharacterTypes targetCharacter, out string effectiveComplement)
        {
            effectiveComplement = "";
            string ineffective = "It's not very effective...";
            string effective = "It's super effective!";

            switch (moveUsed)
            {
                case CharacterTypes.NORMAL:
                    if (targetCharacter == CharacterTypes.NORMAL)
                        return 2;
                    return 1;
                case CharacterTypes.FIRE:
                    switch (targetCharacter)
                    {
                        case CharacterTypes.WATER:
                            effectiveComplement = ineffective;
                            return .5f;
                        case CharacterTypes.EARTH:
                            return 1;
                        case CharacterTypes.FIRE:
                            effectiveComplement = ineffective;
                            return .5f;
                        case CharacterTypes.AIR:
                            effectiveComplement = effective;
                            return 2;
                    }
                    break;
                case CharacterTypes.WATER:
                    switch (targetCharacter)
                    {
                        case CharacterTypes.WATER:
                            effectiveComplement = ineffective;
                            return .5f;
                        case CharacterTypes.EARTH:
                            effectiveComplement = ineffective;
                            return .5f;
                        case CharacterTypes.FIRE:
                            effectiveComplement = effective;
                            return 2;
                        case CharacterTypes.AIR:
                            return 1;
                    }
                    break;
                case CharacterTypes.EARTH:
                    switch (targetCharacter)
                    {
                        case CharacterTypes.WATER:
                            effectiveComplement = effective;
                            return 2;
                        case CharacterTypes.EARTH:
                            effectiveComplement = ineffective;
                            return .5f;
                        case CharacterTypes.FIRE:
                            return 1;
                        case CharacterTypes.AIR:
                            effectiveComplement = ineffective;
                            return .5f;
                    }
                    break;
                case CharacterTypes.AIR:
                    switch (targetCharacter)
                    {
                        case CharacterTypes.WATER:
                            return 1;
                        case CharacterTypes.EARTH:
                            effectiveComplement = effective;
                            return 2;
                        case CharacterTypes.FIRE:
                            effectiveComplement = ineffective;
                            return .5f;
                        case CharacterTypes.AIR:
                            effectiveComplement = ineffective;
                            return .5f;
                    }
                    break;
                default:
                    return 1;
            }

            return 1;
        }
    }
}