using UnityEngine;
using Hex.Models;

namespace Hex.Views
{
    public class NodeFactory : MonoBehaviour
    {
        [SerializeField] private NodeView nodePrefab;
        [SerializeField] private NodeView blockPrefab;
        [SerializeField] private Grid grid;

        public NodeView CreateNode(ICell data)
        {
            var prefab = data.Walkable ? nodePrefab : blockPrefab;
            NodeView node = Instantiate(prefab, grid.transform);
            Vector3 pos = grid.GetCellCenterWorld(data.Index);
            pos.z = pos.y;
            node.transform.position = pos;
            return node;
        }
    }
}