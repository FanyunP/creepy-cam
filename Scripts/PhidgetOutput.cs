using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Phidget22;
using Phidget22.Events;

namespace OpenCVForUnityExample
{
    public class PhidgetOutput : MonoBehaviour
    {
        // Start is called before the first frame update
        VoltageOutput output = new VoltageOutput();
        public int[] outputInfo = new int[2];//SerialNum,Channel
        public Vector2 physicalcarThreshold, ballThreshold;
        public double outputvoltage;

        //get car position
        //public GameObject face;
        //bool isfacedetacked;
        float posX,physicalMiddleThreshold;
        //Vector3 pos;
        int dir;
        public GameObject ball,cam;
        //public float unit,speed, filter;
        //float carThreshold;

        private void VoltageOutput_Attach(object sender, Phidget22.Events.AttachEventArgs e)
        {
            VoltageOutput attachedDevice = ((VoltageOutput)sender);
            attachedDevice.Enabled = true;
            Debug.Log("Digital Output Attached!");
        }

        void loadPhidget(int[] _info, VoltageOutput _output)
        {
            _output.DeviceSerialNumber = outputInfo[0];
            _output.Channel = outputInfo[1];

            _output.Attach += VoltageOutput_Attach;
            _output.Open(5000);
        }

        void detechedPhidget(VoltageOutput _output)
        {
            _output.Enabled = false;
            _output.Attach -= VoltageOutput_Attach;
            _output.Close();
            Debug.Log("Is Phidgets Attached: " + _output.Attached);
        }

        void Start()
        {
            loadPhidget(outputInfo, output);
            physicalMiddleThreshold = physicalMiddleThreshold / 2;
            // carThreshold = car.GetComponent<CarMovement>().threshold;
        }

        // Update is called once per frame
        void Update()
        {

            //case 1: camera fixed on a spot
            //posX = ball.transform.position.x;
            //outputvoltage = Remap(posX, -1 * Screen.width/2, Screen.width / 2, physicalcarThreshold.x, physicalcarThreshold.y);
            //Debug.Log("posX: " +posX + " outputvoltage: " + outputvoltage);
            //case 2: camera on the machine(side)
            posX = cam.transform.position.x;
            outputvoltage = Remap(posX, -1 * Screen.width / 2, Screen.width / 2, physicalcarThreshold.x, physicalcarThreshold.y);
            //Debug.Log("posX: " + posX);

        }

        public static float Remap(float currentValue, float Fmin, float Fmax, float Tmin, float Tmax)
        {
            float Fabs = Fmax - Fmin;
            float Tabs = Tmax - Tmin;
            float Fdis = currentValue - Fmin;
            float normal = Fdis / Fabs;
            float ToValue = Tabs * normal + Tmin;

            return ToValue;
        }
    }
}
