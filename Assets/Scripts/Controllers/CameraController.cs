using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class CameraController : MonoBehaviour
    {
        public Transform target;
        public float rotationDuration = 1f;
        private bool isRotating = false;

        public void RotateCamera(float degrees)
        {
            if (isRotating)
            {
                return;
            }
            Vector3 currentRotation = target.transform.eulerAngles;
            Vector3 targetRotation = currentRotation + new Vector3(0f, degrees, 0f);

            target.transform.DORotate(targetRotation, rotationDuration)
                .SetEase(Ease.OutQuad)
                .OnStart(() =>
                {
                    isRotating = true;
                })
                .OnComplete(() =>
                {
                    isRotating = false;
                });
        }

    } 
}
