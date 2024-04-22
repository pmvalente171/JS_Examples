using System;
using UnityEngine;

[Serializable]
// Struct containing the fish data
public struct Fish
{
    public Sprite icon;
    public string name;
    public int value;

    public Fish(Sprite icon, string name, int value)
    {
        this.icon = icon;
        this.name = name;
        this.value = value;
    }

    public static bool operator ==(Fish a, Fish b) => a.icon == b.icon && a.name == b.name && a.value == b.value;
    public static bool operator !=(Fish a, Fish b) => a.icon != b.icon || a.name != b.name || a.value != b.value;

    public override bool Equals(object obj) => obj is Fish fish && this == fish;
    public override int GetHashCode() => base.GetHashCode();

    public override string ToString() => $"Fish: {name} - Value: {value}";
}

[CreateAssetMenu(fileName = "Fish", menuName = "Game/Fish", order = 0)]
public class FishSO : ScriptableObject
{
    [Header("Values")]
    public Fish fish;
}