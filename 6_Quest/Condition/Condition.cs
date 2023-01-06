using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "")]
public abstract class Condition : ScriptableObject
{
    [SerializeField] string _desciption;
    public abstract bool IsPass(Quest quest);
}
