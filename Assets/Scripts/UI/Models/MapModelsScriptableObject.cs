using System.Collections.Generic;
using UnityEngine;

namespace UnityTemplateProjects.UI.Models
{
    [CreateAssetMenu(menuName = "FPS/Map Models")]
    public class MapModelsScriptableObject : ScriptableObject
    {
        public List<MapModel> MapModels;
    }
}