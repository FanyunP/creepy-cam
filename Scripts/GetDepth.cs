using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetDepth : MonoBehaviour
{
    // Start is called before the first frame update

    //depth
    public float targetRangeWidth,unit,filter,gap,depthFilter;
    public int Num;
    Vector2 RectRange;
    float lastDepth,total,average;
    private ZEDManager zedManager;
    private Camera leftCamera;
    Vector3 testPos,middlePoint,middlePointLeft,middlePointRight;
    Vector3[] mainPos;
    Vector2 resolution,Xrange;
    int dir;
    public float[] lastDepths;
    public List<float> depths = new List<float>();
    
    //List<float> lastDepths = new List<float>();

    private void Awake()
    {
        zedManager = ZEDManager.GetInstance(sl.ZED_CAMERA_ID.CAMERA_ID_01);
        leftCamera = zedManager.GetLeftCameraTransform().gameObject.GetComponent<Camera>();

        Cursor.visible = true;
    }

    void Start()
    {
        lastDepth = 0f;
        RectRange = new Vector2(targetRangeWidth,Screen.height/2);
        Xrange = new Vector2(targetRangeWidth + gap, Screen.width - targetRangeWidth-gap);
        dir = 1;
        if (Num %2 ==0) {
            Num = Num + 1;
        }
        mainPos = new Vector3[Num];
        lastDepths = new float[Num];
    }

    // Update is called once per frame
    void Update()
    {
        resolution = new Vector2(zedManager.zedCamera.ImageWidth, zedManager.zedCamera.ImageHeight);
        if (resolution.x != 0) {
            //testPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
            middlePoint = new Vector3(Screen.width / 2, Screen.height / 2, 0);
            depths.Clear();
            dir = 0;
            for (int i = 0; i<Num;i++) {
                mainPos[i] = new Vector3(middlePoint.x + gap * (i -Num/2), middlePoint.y);
                depths.Add(AverageDepth(mainPos[i], RectRange, unit, filter));
                Debug.Log(i+": " + "last: " + lastDepths[i] +" current: " +depths[i]);
                if (Mathf.Abs(lastDepths[i] - depths[i]) < depthFilter)
                {
                    dir += 0;
                }
                else {
                    dir += (int)((lastDepths[i] - depths[i]) / Mathf.Abs(lastDepths[i] - depths[i]));
                }
                lastDepths[i] = depths[i];
            }

            Debug.Log("dir: " + dir);
            //middlePoint = new Vector3(Screen.width/2,Screen.height/2,0);
            //middlePointLeft = new Vector3(middlePoint.x - gap,middlePoint.y);
            //middlePointRight = new Vector3(middlePoint.x + gap, middlePoint.y);
            //depths.Clear();
            //depths.Add(AverageDepth(middlePointLeft, RectRange, unit, filter));
            //depths.Add(AverageDepth(middlePoint, RectRange, unit, filter));
            //depths.Add(AverageDepth(middlePointRight, RectRange, unit, filter));

            //depths = AverageDepths(Xrange, 2 * targetRangeWidth, RectRange,targetRangeWidth,filter);
            //average = AverageDepth(middlePoint,RectRange,unit,filter);
            //Debug.Log("Average: " + average);
            ////get the median
            //depths.Sort();
            //float middleDepth = zedManager.zedCamera.GetDepthValue(testPos);
            //Debug.Log("middle Depth: " + middleDepth);
        }
    }

    List<float> AverageDepths(Vector2 xRange, float xUnit, Vector2 range, float standardUnit, float filterDiff)
    {
        List<float> aveDeps = new List<float>();
        for (float i = xRange.x; i < xRange.y; i += xUnit)
        {
            aveDeps.Add(AverageDepth(new Vector2(i, middlePoint.y), RectRange, unit, filter));
        }
        return aveDeps;
    }

    float AverageDepth(Vector2 middlePoint, Vector2 range, float standardUnit,float filterDiff) {
        List<float> dep = new List<float>();
        float all=0f;
        float ave = 0f;
        for (float i = middlePoint.x - range.x; i < middlePoint.x + range.x;i+=standardUnit) {
            for (float j = middlePoint.y-range.y ; j<middlePoint.y + range.y; j+= standardUnit) {
                Vector3 currentPoint = new Vector3(i,j,0);
                float depth = zedManager.zedCamera.GetDepthValue(currentPoint);
                float diff = depth - lastDepth;
                lastDepth = depth;
                if (Mathf.Abs(diff) < filterDiff)
                {
                    all += depth;
                    dep.Add(depth);
                    //Debug.Log("depth: " + depth);
                }
            }
        }
        if (dep.Count != 0) {
            ave = all / dep.Count;
        }
        return ave;
    }
}
