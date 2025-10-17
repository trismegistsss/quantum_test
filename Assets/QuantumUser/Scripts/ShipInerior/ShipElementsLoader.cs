using AGS.EditorView.ShipEditor.Config;
using AGS.EditorView.ShipEditor.Data;
using AGS.EditorView.ShipEditor.Models;
using AGS.EditorView.ShipEditor.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AGS.EditorView.ShipEditor
{
    public class ShipElementsLoader : MonoBehaviour
    {
        private readonly List<ShipElementView> elements = new List<ShipElementView>();

        public static ShipElementsLoader Instance
        {
            get
            {
                var instance = FindObjectOfType<ShipElementsLoader>();

                if (instance == null)
                {
                    var obj = new GameObject { name = "_ShipElementsLoader" };

                    instance = obj.AddComponent<ShipElementsLoader>();
                    instance.Refresh();
                }

                instance.elements.RemoveAll(e => e == null);

                return instance;
            }
        }

        public void Refresh()
        {
            ShipConfig.Load();

            var settings = ShipConfig.Assets;
            var loaded = Resources.LoadAll(settings.Elements);
            elements.Clear();

            var select = loaded.Select(o => o as GameObject);
            elements.AddRange(select.Select(o => o.GetComponent<ShipElementView>()));
            elements.RemoveAll(e => e == null || e.Type == ShipElement.None);
        }

        public ShipElementView LoadElementView(int type)
        {
            return LoadElementView(Guid.NewGuid().ToString(), type);
        }

        public ShipElementView LoadElementView(string id, int type)
        {
            var find = FindElement(type);

            if (find != null)
            {
                var clone = Instantiate(find.gameObject);
                var view = clone.GetComponent<ShipElementView>();

                if (view != null)
                {
                    Debug.Assert(!string.IsNullOrEmpty(id), "Element id empty!");

                    view.Id = id;

                    return view;
                }
            }

            Debug.LogErrorFormat("Can't load view for element {0}", type);

            return null;
        }

        public ShipElementView FindElement(int type)
        {
            return type == ShipElement.None ? null : elements.Find(e => e.Type == type);
        }

        public ShipElementView[] Elements
        {
            get { return elements.ToArray(); }
        }

        public ShipElementData[] Models
        {
            get { return elements.Select(e => e.Data).ToArray(); }
        }

        public string[] TypeNames
        {
            get { return elements.Select(e => e.name).ToArray(); }
        }
    }
}
