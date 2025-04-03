using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect
{
    protected EffectStats stats;

    public Effect(EffectStats stats)
    {
        this.stats = stats;
    }

    public virtual void OnSpawn()
    {

    }
}
