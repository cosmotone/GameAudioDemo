using System;
using System.Collections;
using System.Collections.Generic;
using HappyHarvest;
using Template2DCommon;
using UnityEngine;
using UnityEngine.Tilemaps;
using FMODUnity;
using FMOD.Studio;

namespace HappyHarvest
{
    /// <summary>
    /// Handle playing an FMOD step sound event during walk animation. This needs to be on the same GameObject of the player
    /// with the Animator as it needs to receive the PlayStepSound events from the walking animation.
    /// Contains a list of pairing of tiles to parameter values, so it can play different sounds based on the
    /// tile under the player by setting an FMOD parameter.
    /// Note: the Tilemap that is checked for which tile is walked on needs a WalkableSurface component on it.
    /// </summary>
    public class StepSoundHandlerFmod : MonoBehaviour
    {
        [Serializable]
        public class TileSurfaceMapping
        {
            public TileBase[] Tiles;
            public string SurfaceParameterLabel;
        }

        public EventReference StepSoundEvent;

        public string SurfaceParameterName = "Surface";
        public string DefaultSurfaceLabel = "";
        public TileSurfaceMapping[] SurfaceMappings;

        private Dictionary<TileBase, string> m_Mapping = new();

        void Start()
        {
            foreach (var mapping in SurfaceMappings)
            {
                foreach (var tile in mapping.Tiles)
                {
                    m_Mapping[tile] = mapping.SurfaceParameterLabel;
                }
            }
        }

        //This is called by animation event on the walking animation of the character.
        public void PlayStepSound()
        {
            var underCell = GameManager.Instance.WalkSurfaceTilemap.WorldToCell(transform.position);
            var tile = GameManager.Instance.WalkSurfaceTilemap.GetTile(underCell);

            string surfaceLabel = (tile != null && m_Mapping.ContainsKey(tile))
                ? m_Mapping[tile]
                : DefaultSurfaceLabel;

            EventInstance stepEvent = RuntimeManager.CreateInstance(StepSoundEvent);
            stepEvent.setParameterByNameWithLabel(SurfaceParameterName, surfaceLabel);
            stepEvent.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));
            stepEvent.start();
            stepEvent.release();
        }
    }
}