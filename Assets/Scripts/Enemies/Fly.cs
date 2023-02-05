using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Fly : RandomWalkFlyingEnemy
{
    protected override void Start()
    {
        base.Start();

        speed = 2f;
        minMoveTime = 0.1f;
        maxMoveTime = 0.4f;
        maxWanderDistance = 1f;
    }
}
