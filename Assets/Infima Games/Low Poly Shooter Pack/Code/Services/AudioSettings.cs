//Copyright 2022, Infima Games. All Rights Reserved.

using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

namespace InfimaGames.LowPolyShooterPack
{
    /// <summary>
    /// Audio Settings used to interact with the AudioManagerService.
    /// </summary>
    [System.Serializable]
    public struct AudioSettings
    {
        /// <summary>
        /// Automatic Cleanup Getter.
        /// </summary>
        public bool AutomaticCleanup => automaticCleanup;
        /// <summary>
        /// Volume Getter.
        /// </summary>
        public float Volume => volume;
        /// <summary>
        /// Spatial Blend Getter.
        /// </summary>
        public float SpatialBlend => spatialBlend;

        public Vector3 AudioPos => audioPos;

        public void SetPosition(Vector3 set)
        {
            this.audioPos = set;
        }

        public bool Is3D => is3D;
        
        public void SetIs3D(bool set)
        {
            is3D = set;
        }

        public Transform Parent => parent;

        public void SetParent(Transform set)
        {
            this.parent = set;
        }

        public float MaxDistance => maxDistance;

        public void SetMaxDistance(float set)
        {
            this.maxDistance = set;
        }

        
        [Header("Settings")]
        
        [Tooltip("If true, any AudioSource created will be removed after it has finished playing its clip.")]
        [SerializeField]
        private bool automaticCleanup;

        [Tooltip("Volume.")]
        [Range(0.0f, 1.0f)]
        [SerializeField]
        private float volume;

        [Tooltip("Spatial Blend.")]
        [Range(0.0f, 1.0f)]
        [SerializeField]
        private float spatialBlend;
        
        private Vector3 audioPos;

        private bool is3D;
        
        private Transform parent;

        private float maxDistance;

        /// <summary>
        /// Constructor.
        /// </summary>
        public AudioSettings(float volume, float spatialBlend , bool automaticCleanup ,bool is3D, Vector3 pos , Transform parent, float maxDistance)
        {
            //Volume.
            this.volume = volume;
            //Spatial Blend.
            this.spatialBlend = spatialBlend;
            //Automatic Cleanup.
            this.automaticCleanup = automaticCleanup;
            
            
            //是否是空间音源
            this.is3D = is3D;
            //声音方位
            this.audioPos = pos;
            //父物体
            this.parent = parent;
            this.maxDistance = maxDistance;
        }
    }
}