using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OpenCVForUnityExample
{
    public class headMotion : MonoBehaviour
    {
        // Start is called before the first frame update
        public GameObject face;
        public GameObject camera;
        public Vector2 offset;
        public float diff,camSpeed,camAccl,HypotenuseFilter;
        Vector2 mainCenterPos, pos,size;
        float camThreshold, lastHypotenuse,currentHypotenuse,hypotenuse;
        Vector3 camPos;
        void Start()
        {
            offset += new Vector2(-1 * Screen.width/2,0);
            camPos = camera.transform.position;
            camThreshold = Screen.width / 2;
        }

        // Update is called once per frame
        void Update()
        {
            pos = face.GetComponent<DnnObjectDetectionWebCamTextureExample>().pos;
            size = face.GetComponent<DnnObjectDetectionWebCamTextureExample>().size;
            lastHypotenuse = hypotenuse;
            hypotenuse = Mathf.Sqrt(Mathf.Pow(size.x,2) + Mathf.Pow(size.y, 2));
            float diffHypotenuse = hypotenuse - lastHypotenuse;
            Debug.Log("hypotenuse: " + hypotenuse);
            if (Mathf.Abs(hypotenuse) > HypotenuseFilter ) {
                Debug.Log("diff1: " + diffHypotenuse);
                //Debug.Log("hypotenuse: " + hypotenuse);
                ////case 1: camera fixed on a spot

                transform.position = new Vector3((pos.x + offset.x), 0, 0);
                //Debug.Log("centerPos: " + pos);
                //case 2: camera on the machine(side)
                camAccl = transform.position.x;
                camSpeed = camAccl * Time.deltaTime;
                camPos.x += camSpeed;
                if (Mathf.Abs(camPos.x) > camThreshold)
                {
                    camPos.x = camPos.x / Mathf.Abs(camPos.x) * camThreshold;
                }
                camera.transform.position = camPos;
                //mainCenterPos = new Vector2(Screen.width/2,Screen.height/2);
                //diff = transform.position.x - mainCenterPos.x;
                //Debug.Log("cam pos:" + camera.transform.position.x);
            }

        }
    }
}
