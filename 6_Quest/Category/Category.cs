using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Category", fileName = "Category_")]
public class Category : ScriptableObject, IEquatable<Category>
{
    [SerializeField] string _codeName;
    [SerializeField] string _displayName;

    public string CodeName => _codeName;
    public string DisplayName => _displayName;

    #region Operator
    public bool Equals(Category other)
    {
        if (other is null)
        {
            return false;
        }
        if (ReferenceEquals(other, this))
        {
            return true;
        }
        if (GetType() != other.GetType())
        {
            return false;
        }
        return _codeName == other._codeName;
    }
    public override int GetHashCode() => (CodeName, DisplayName).GetHashCode();

    public override bool Equals(object other) => base.Equals(other);
    public static bool operator ==(Category lhs, string rhs)
    {
        if (lhs is null)
        {
            return ReferenceEquals(rhs, null);
        }
        return lhs.CodeName == rhs || lhs.DisplayName == rhs;
    }
    public static bool operator !=(Category lhs, string rhs) => !(lhs == rhs);
    #endregion
}
