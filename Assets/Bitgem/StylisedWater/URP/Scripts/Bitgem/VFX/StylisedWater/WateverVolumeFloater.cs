#region Using statements

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#endregion

namespace Bitgem.VFX.StylisedWater
{
    public class WateverVolumeFloater : MonoBehaviour
    {
        #region Public fields

        public Vector3 Offset = Vector3.zero;
        public WaterVolumeHelper WaterVolumeHelper = null;

        [SerializeField] private bool printIfHasHeight;

        #endregion

        #region MonoBehaviour events

        void Update()
        {
            var instance = WaterVolumeHelper ? WaterVolumeHelper : WaterVolumeHelper.Instance;
            if (!instance)
            {
                return;
            }

            var hasHeight = instance.GetHeight(transform.position).HasValue;
            transform.position = new Vector3(transform.position.x, instance.GetHeight(transform.position) ?? transform.position.y, transform.position.z)
                + (hasHeight ? Offset : Vector3.zero);

            if (printIfHasHeight)
                Debug.Log("Water Has Height: " + hasHeight);
        }

        #endregion
    }
}