using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/StepMesh")]
[Serializable]
public class StepMesh : ScriptableObject
{
    public int id;
    public Material mesh;
}
