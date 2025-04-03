using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public struct ItemData
    {
        public string name;
        public int iD;
        public Sprite icon;
        public int maxStack;
    }
}
