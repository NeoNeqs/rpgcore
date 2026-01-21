using System;
using Godot;
using Godot.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Attribute = rpgcore.gizmo.enums.Attribute;

namespace rpgcore.gizmo.components;

[GlobalClass, Tool]
public abstract partial class EquipmentComponent : GizmoComponent {
    [Export]
    public ulong ItemLevel {
        private set {
            field = value;
            if (Engine.IsEditorHint()) {
                RecalculateStats();
            }
        }
        get;
    }

    [Export]
    public Attributes? Weights {
        private set {
            if (Engine.IsEditorHint()) {
                if (field is not null) {
                    field.AttributeChanged -= RecalculateStats;
                }

                field = value;

                if (field is not null) {
                    field.AttributeChanged += RecalculateStats;
                }

                RecalculateStats();
            } else {
                field = value;
            }
        }
        get;
    } = null!;

    [Export] public Attributes?[] RandomStatPools { get; set; } = null!;

    [Export, Notify]
    public ulong MaxDurability {
        private set {
            if (Durability > value) Durability = value;
            field = value;
        }
        get;
    }

    [Export, Notify]
    public ulong Durability {
        private set {
            if (value > MaxDurability) value = MaxDurability;
            _durability.Set(value);
        }
        get => _durability.Get();
    }

    public ReadonlyAttributes? Attributes {
        get;
        private set {
            field = value;
            RecalculateStats();
        }
    }

    protected abstract float GetAdditionalPrice();


    private void RecalculateStats(Attribute attribute, float oldValue, float newValue) {
        RecalculateStats();
    }

    protected void RecalculateStats([CallerMemberName] string? pCaller = null) {
        if (pCaller != nameof(Weights)) {
            return;
        }

        foreach (Attribute stat in Enum.GetValues<Attribute>()) {
            Attributes?.Set2(stat, Mathf.Inf);
        }

        float mainBudget = ItemLevel;

        // --- Pass 1: Guaranteed Stats ---
        float guaranteedTotalWeight = Weights?.Sum() ?? 1.0f;
        GD.Print(Weights);
        if (guaranteedTotalWeight > 0 && Weights is not null) {
            foreach ((string statStr, float weight) in Weights.Iter()) {
                GD.Print(4);
                if (weight <= 0) continue;
                var stat = Enum.Parse<Attribute>(statStr);

                float share = weight / guaranteedTotalWeight;
                float price = Pricing.GetBasePrice(stat) + GetAdditionalPrice();
                float finalValue = Mathf.Ceil((mainBudget * share) / price);

                if (finalValue > 0) {
                    Attributes?.Set2(stat, finalValue);
                }
            }
        }

        float secondaryBudget = mainBudget * 0.1f;
        // --- Pass 2: Random Stats (One roll per pool) ---
        if (Engine.IsEditorHint() || RandomStatPools.Length <= 0) return;
        GD.Print(2);
        foreach (Attributes? pool in RandomStatPools) {
            if (pool == null) continue;

            IReadOnlyDictionary<string, float> poolIter = pool.Iter();
            if (poolIter.Count == 0) continue;

            // 1. Get all stats with a positive weight to treat them as equal candidates
            var candidates = new List<string>();
            float poolTotalWeight = 0;
            foreach ((string name, float weight) in poolIter) {
                if (!(weight > 0)) continue;
                candidates.Add(name);
                poolTotalWeight += weight;
            }

            if (candidates.Count == 0) continue;

            // 2. Pick one stat with equal probability (1 / candidates.Count)
            var index = (int)(GD.Randi() % candidates.Count);
            string statStr = candidates[index];
            var stat = Enum.Parse<Attribute>(statStr);
            float weightForBudget = poolIter[statStr];

            // 3. Calculate budget for the picked stat
            // We use the weight relative to the pool's total to determine the budget share
            float share = weightForBudget / poolTotalWeight;
            float price = Pricing.GetBasePrice(stat) + GetAdditionalPrice();
            float finalValue = Mathf.Floor((secondaryBudget * share) / price);

            if (finalValue > 0) {
                Attributes?.Increase(stat, finalValue);
            }
        }
    }

    public override Array<Dictionary> _GetPropertyList() {
        return [
            ExportUtils.Export(nameof(Attributes), Variant.Type.Object, PropertyHint.ResourceType,
                nameof(ReadonlyAttributes), PropertyUsageFlags.Default)
        ];
    }
}