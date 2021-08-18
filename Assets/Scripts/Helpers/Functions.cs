using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Helper
{
    public class Functions : MonoBehaviour
    {

        public IEnumerator rotateObject(Transform item, Vector3 rotationAxis, float rotationSpeed, float waveHeight)
        {
            while (true)
            {
                item.Rotate(rotationAxis, 1 * rotationSpeed, Space.Self);
                item.position += Mathf.Sin(Time.time * Mathf.PI) / waveHeight * Vector3.up;
                yield return null;
            }
        }
        public IEnumerator lerpPositions2D(RectTransform objectToLerp, Vector3 startingPos, Vector3 targetPos, float speed, bool controlVal)
        {

            float lerpVal = 0;
            while (lerpVal < 1)
            {
                controlVal = true;
                objectToLerp.localPosition = Vector3.Lerp(startingPos, targetPos, lerpVal);

                yield return null;
                lerpVal += Time.deltaTime * speed;
            }
            if (lerpVal >= 1)
                controlVal = false;
        }

        private void FloatRotateObjectFixed(Transform myObject, float floatingWaveLenght, float floatingSpeed, float rotatingSpeed)
        {
            myObject.position += Mathf.Sin(Time.time * floatingSpeed) * (Vector3.up) * floatingWaveLenght;
            myObject.RotateAround(myObject.position, myObject.up, rotatingSpeed);
        }
        
        public void LoadURL( string urlName )
        {
            Application.OpenURL(urlName);
        }
    }
}
