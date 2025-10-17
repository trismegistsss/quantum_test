using AGS.EditorView.ShipEditor;
using AGS.EditorView.ShipEditor.Data;
using AGS.EditorView.ShipEditor.Objects;
using AGS.Geom;
using AGS.Graber.Character;
using AGS.Graber.Model;
using CustomUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using VW.View;
using Zenject;
using static AGS.EditorView.ShipEditor.Data.RoomsData;

namespace AGS.Graber
{
    public class GrabLevelView : ShipBaseEditorView
    {
        [Inject] GrabLevel _level;

        [SerializeField] NavMeshView _navmesh;
        [SerializeField] Transform _plane;

        private readonly List<ShipElementView> elements = new List<ShipElementView>();
        private List<Room> rooms;
        private Dictionary<int, List<ShipElementView>> doors;
        private Dictionary<int, List<ShipElementView>> enemies;

        public void Refrash()
        {
            Refresh();
           // FillBackGround();
        }

        public new void Load(ShipLevelData data)
        {
            elements.ForEach(e => XMLHelper.Destroy(e.gameObject));

            elements.Clear();

            base.Load(data);

            var loader = ShipElementsLoader.Instance;

            doors = new Dictionary<int, List<ShipElementView>>();
            enemies = new Dictionary<int, List<ShipElementView>>();

            // rooms
            if (null != data.Rooms)
                rooms = data.Rooms.RoomsList;

            // elements 
            foreach (var cell in data.Cells)
            {
                foreach (var item in cell.Elements)
                {
                    var view = loader.LoadElementView(item.Element);

                    if (view == null)
                        continue;

                    view.Id = item.Id;
                    view.Cell = cell.Point;
                    view.SortOrder((int)view.Layer);

                    // add doors in rooms
                    if(view.Data.Door || view.Data.Enemy)
                    {
                        var sroom = GetCurrentRoom(view.Cell);

                        if (view.Data.Door)
                        {
                            if (!doors.ContainsKey(sroom.Index))
                            {
                                var list = new List<ShipElementView>();
                                list.Add(view);
                                doors.Add(sroom.Index, list);
                            }
                            else
                                doors[sroom.Index].Add(view);
                        }

                        if(view.Data.Enemy)
                        {
                            if (!enemies.ContainsKey(sroom.Index))
                            {
                                var list = new List<ShipElementView>();
                                list.Add(view);
                                enemies.Add(sroom.Index, list);
                            }
                            else
                                enemies[sroom.Index].Add(view);
                        }
                    }

                    AddView(view);
                }
            }

            var player = GetPlayer().GetComponent<GrabPlayerCharacter>();
            foreach (var item in enemies)
            {
                foreach (var enm in item.Value)
                {
                    var ech = enm.GetComponent<GrabEnemyCharacter>();
                    ech.InitBehavior(player);
                }
            }

            _plane.localScale = new Vector3(Width/2f, 1, Height/2f);
        }

        public void CreateNavMesh()
        {
            _navmesh.Backe();
        }

        public void UpdateNavMesh()
        {
            foreach (var nav in _navmesh.Surfaces)
            {
                nav.UpdateNavMesh(nav.navMeshData);
            }
        }

        public void AddView(ShipElementView view)
        {
            view.transform.SetParent(transform);
            var np =  ConvertCellToWorldPoint(view.Cell);
            view.transform.localPosition = new Vector3(np.x, 0, np.y);
            view.Level = this;

            if (view.Item) _level.ItemsInLevel++;

            elements.Add(view);
        }

        public void AddView(ShipElementView view, Point cell)
        {
            view.Cell = cell;

            AddView(view);
        }

        public void RemoveView(ShipElementView view)
        {
            elements.Remove(view);

            XMLHelper.Destroy(view.gameObject);
        }

        public ShipElementView GetView(Point cell, ShipElementLayer layer)
        {
            return elements.Find(e => e.Cell == cell && e.Layer == layer);
        }

        public ShipElementView GetView(Point cell)
        {
            var elements = GetElements(cell).OrderByDescending(e => e.Layer);

            return elements.Any() ? elements.First() : null;
        }

        public ShipElementView GetView(string id)
        {
            return elements.Find(e => e.Id == id);
        }

        public ShipElementView[] GetElements(Point cell)
        {
            return elements.Where(e => e.Cell == cell).ToArray();
        }

        public ShipElementView[] GetElements(ShipElementLayer layer)
        {
            return elements.Where(e => e.Layer == layer).ToArray();
        }

        public ShipElementView GetPlayer()
        {
            return elements.FirstOrDefault(e => e.Player);
        }

        public Room GetCurrentRoom(Point position)
        {
            return (from room in rooms where room.Points.Any(pr => pr == position) select room).SingleOrDefault();
        }

        public void KillEnemyInRoom(int roomid, ShipElementView enemy)
        {
            enemies[roomid].Remove(enemy);
            rooms.FirstOrDefault(pr=>pr.Index == roomid).EnemyCount--;
        }

        public List<ShipElementView> GetRoomDoors(int roomid)
        {
            if (doors.ContainsKey(roomid))
                return doors[roomid];
            return null;
        }

        public ShipElementView GetRoomTargetEnemy(int roomid, Transform player)
        {
            var targetpos = 1000f;
            ShipElementView target = null;
            if (enemies.ContainsKey(roomid))
            {
                foreach(var enemy in enemies[roomid])
                {
                    var ttarg = (enemy.transform.position - player.position).magnitude;
                    if (ttarg < targetpos && !enemy.Death)
                    {
                        targetpos = ttarg;
                        target = enemy;
                    }
                }
            }

            return target;
        }

        // gameloop functional

        public void DestroyItem(ShipElementView view)
        {
            RemoveView(view);
            var player = GetPlayer();

            if (view.Cell == player.Cell)
            {
                RemoveView(player);
                Observable.Timer(TimeSpan.FromSeconds(1))
                  .Subscribe(_ =>
                  {
                      SceneManager.LoadScene("Graber");
                  }).AddTo(this);
            }
        }
    }
}