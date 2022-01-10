using UnityEngine;


// 因为棋盘是方格化的，所以每个格子在有XYZ轴坐标的同时，还有一个XY的编号坐标，类似第N行第M列
// Geometry就负责做这个转换
// 改变方格大小后需要调整这里的数值
// 这里把水平默认在了0，如有需要修正模型的话需要修改
public class Geometry
{
	// 编号转坐标
    static public Vector3 PointFromGrid(Vector2Int gridPoint)
    {
        float x = -3.5f + 1.0f * gridPoint.x;
        float z = -3.5f + 1.0f * gridPoint.y;
        return new Vector3(x, 0, z);
    }

	// 返回标准编号
    static public Vector2Int GridPoint(int col, int row)
    {
        return new Vector2Int(col, row);
    }

	// 坐标转编号，做求整
    static public Vector2Int GridFromPoint(Vector3 point)
    {
        int col = Mathf.FloorToInt(4.0f + point.x);
        int row = Mathf.FloorToInt(4.0f + point.z);
        return new Vector2Int(col, row);
    }
}
