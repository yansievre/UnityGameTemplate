using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEngine;

namespace Utility.Editor
{
    public class MeshPickWindow : OdinEditorWindow
    {
        [TableList(AlwaysExpanded = true), ShowInInspector, HideLabel]
        private List<MeshPreview> _foundMeshFilters;

        private Action<MeshFilter> _callback;
        
        public static void OpenWindow(List<MeshFilter> meshFilters, Action<MeshFilter> callback)
        {
            var window = GetWindow<MeshPickWindow>();

            window._foundMeshFilters = meshFilters.Select(x => new MeshPreview()
            {
                meshFilter = x,
                selected = false,
                update = window.UpdateSelection,
                name = x.gameObject.name
            }).ToList();
            window._callback = callback;
                
            window.Show();
        }
        
        private bool CanSubmit()
        {
            return _foundMeshFilters.Any(x => x.selected);
        }

        [Button, EnableIf(nameof(CanSubmit))]
        public void Submit()
        {
            _callback?.Invoke(_foundMeshFilters.First(x => x.selected).meshFilter);
        }

        private void UpdateSelection(MeshPreview preview)
        {
            foreach (var meshPreview in _foundMeshFilters)
            {
                meshPreview.selected = meshPreview == preview;
            }
        }
        
        private class MeshPreview
        {
            public Action<MeshPreview> update;
            public bool selected;

            [ShowInInspector, ReadOnly]
            public string name;
            
            [PreviewField]
            [ShowInInspector, ReadOnly]
            public MeshFilter meshFilter;

            [Button, DisableIf(nameof(selected))]
            public void Select()
            {
                update?.Invoke(this);
            }
        }
    }
}