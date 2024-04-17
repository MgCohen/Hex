using System.Collections.Generic;
using Hex.Models;

namespace Hex.Controllers
{
    public class PathCache : Dictionary<ICell, Dictionary<ICell, List<ICell>>>
    {
        public bool TryGetPath(ICell start, ICell end, out List<ICell> path)
        {
            if (TryGetValue(start, out var paths))
            {
                return paths.TryGetValue(end, out path);
            }
            path = null;
            return false;
        }

        public void CachePath(List<ICell> path)
        {
            ICell start = path[0];
            ICell end = path[^1];

            if (!TryGetValue(start, out var paths))
            {
                paths = new Dictionary<ICell, List<ICell>>();
                Add(start, paths);
            }
            paths[end] = path;

            if (!TryGetValue(end, out paths))
            {
                paths = new Dictionary<ICell, List<ICell>>();
                Add(end, paths);
            }

            path.Reverse();
            paths[start] = path;

        }
    }
}
