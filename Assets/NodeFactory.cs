using UnityEngine;

public class NodeFactory: MonoBehaviour
{
    [SerializeField] private NodeView nodePrefab;
    [SerializeField] private Grid grid;

    public NodeView CreateNode(Vector3Int index)
    {
        NodeView node = Instantiate(nodePrefab, grid.transform);
        Vector3 pos = grid.GetCellCenterWorld(index);
        pos.z = pos.y;
        node.transform.position = pos;
        return node;
    }
}