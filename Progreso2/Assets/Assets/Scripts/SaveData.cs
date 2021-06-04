using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    [System.Serializable]
    public class SaveData
    {
        public float positionX;
        public float positionY;
        public float positionZ;

        public List<float> livingTargetPositionsX = new List<float>();
        public List<string> livingTargetsTypes = new List<string>();

        public override string ToString()
        {
            return $"{positionX}, {positionY}, {positionZ}";
        }
    }
}
