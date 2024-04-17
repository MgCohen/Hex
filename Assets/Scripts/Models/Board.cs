using System.Collections.Generic;
using UnityEngine;

namespace Hex.Models
{
    public class Board : Dictionary<Vector3Int, ICell>, IMap
    {

    }
}
