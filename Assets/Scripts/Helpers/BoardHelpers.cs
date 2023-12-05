using Assets.Scripts.Consts;
using UnityEngine;

namespace Assets.Scripts.Helpers
{
    public static class BoardHelpers
    {
        public static Vector3 GetTileCenter(int x, int y)
        {
            Vector3 origin = Vector3.zero;
            origin.x += (BoardConsts.TILE_SIZE * x) + BoardConsts.TILE_OFFSET;
            origin.z += (BoardConsts.TILE_SIZE * y) + BoardConsts.TILE_OFFSET;

            return origin;
        }
    }
}
