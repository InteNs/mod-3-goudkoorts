using System;
using System.Dynamic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

namespace models
{

    using System.Collections.Generic;

    public class Map
    {
        public Map()
        {
            Generate();
        }
        public object[,] LocationMap { get; set; }

        public virtual Track DockTrackIn
        {
            get;
            set;
        }

        public virtual Track DocktrackOut
        {
            get;
            set;
        }

        public virtual List<WareHouse<Cart>> CartStarts
        {
            get;
            set;
        }

        public virtual List<WareHouse<Boat>> BoatStarts
        {
            get;
            set;
        }

        public virtual List<SwitchTrack> Switches
        {
            get;
            set;
        }

        /*******************GENERATION***************/

        private Track _dockTracks;
        private Track _safeTracks;
        private Track _seaTracks;

        private void Generate()
        {
            InitStarts();
            InitSwitches();
            InitTrackLists();
            MapTracks();
        }

        private void InitStarts()
        {
            BoatStarts[0] = BuildWareHouse<Boat>(0, 0);

            CartStarts[0] = BuildWareHouse<Cart>(3, 0);
            CartStarts[1] = BuildWareHouse<Cart>(5, 0);
            CartStarts[2] = BuildWareHouse<Cart>(7, 0);
        }

        private void InitSwitches()
        {
            Switches[1] = new MergeTrack { Number = 1 };
            Locate(4, 3, Switches[0]);
            Switches[2] = new SplitTrack { Number = 2 };
            Locate(4, 5, Switches[1]);
            Switches[3] = new MergeTrack { Number = 3 };
            Locate(4, 9, Switches[0]);
            Switches[4] = new MergeTrack { Number = 4 };
            Locate(6, 6, Switches[0]);
            Switches[5] = new SplitTrack { Number = 5 };
            Locate(6, 8, Switches[0]);
        }

        private void InitTrackLists()
        {
            /*******************_dockTracks**/
            //9 normal tiles
            var dockPointer = BuildTrackList(1, 0, 9, 'E');
            //holding tile
            dockPointer = BuildTrack<HoldingTrack>(1, 9, dockPointer);
            DockTrackIn = dockPointer;
            //2 normal tiles
            dockPointer = BuildTrackList(1, 10, 2, 'E', dockPointer);
            //3 normal tiles
            dockPointer = BuildTrackList(4, 11, 3, 'N', dockPointer);
            //one normal tile
            _dockTracks = BuildTrack<Track>(4, 10, dockPointer);

            /*******************_safeTracks**/
            //8 safe tiles
            var safePointer = BuildTrackList(8, 1, 8, 'E');
            //3 normal tiles
            safePointer = BuildTrackList(8, 9, 3, 'E', safePointer);
            //4 normal tiles
            _safeTracks = BuildTrackList(7, 11, 4, 'W', safePointer);

            /*******************_seaTracks**/
            //2 normal tiles
            var seaPointer = BuildTrackList(0, 11, 2, 'W');
            //holding tile
            seaPointer = BuildTrack<HoldingTrack>(0, 9, seaPointer);
            DocktrackOut = seaPointer;
            //8 normal tiles
            _seaTracks = BuildTrackList(0, 8, 8, 'W', seaPointer);

            /*******************_bTo1**/

        }

        private void MapTracks()
        {
            BoatStarts[0].Track = _seaTracks;

            Switches[5].DownTrack = _safeTracks;
            Switches[3].Next = _dockTracks;

            Track last;
            //B -> 1
            CartStarts[0].Track = BuildTrackList(3, 1, 3, 'E', out last, Switches[1]);
            Switches[1].UpTrack = last;
            //C -> 1
            CartStarts[1].Track = BuildTrackList(5, 1, 3, 'E', out last, Switches[1]);
            Switches[1].DownTrack = last;
            //D -> 4
            CartStarts[2].Track = BuildTrackList(7, 1, 6, 'E', out last, Switches[4]);
            Switches[4].DownTrack = last;
            //2 -> 4
            Switches[2].DownTrack = BuildTrackList(5, 5, 2, 'E', out last, Switches[4]);
            Switches[4].UpTrack = last;
            //2 -> 5
            Switches[2].UpTrack = BuildTrackList(3, 5, 5, 'E', out last, Switches[3]);
            Switches[3].UpTrack = last;
            //5 -> 3
            Switches[5].UpTrack = BuildTrackList(5, 8, 2, 'E', out last, Switches[3]);
            Switches[3].DownTrack = last;
            //1 -> 2
            Switches[1].Next = BuildTrack<Track>(4, 4, Switches[2]);
            //4 -> 5
            Switches[4].Next = BuildTrack<Track>(6, 7, Switches[5]);
        }

        /*************HELPER METHODS FOR GENERATION*************/

        private void Locate(int y, int x, object value)
        {
            LocationMap[y, x] = value;
        }

        private void IncrementCoord(ref int y,ref int x, char dir)
        {
            switch (dir)
            {
                case 'N': y--;
                    break;
                case 'E': x++;
                    break;
                case 'S': y++;
                    break;
                case 'W': x--;
                    break;
            }
        }
        private Track BuildTrackList(int y, int x, int amount, char dir, Track attachTo = null)
        {
            Track dummy;
            return BuildTrackList(y,x, amount, dir, out dummy, attachTo);
        }


        private Track BuildTrackList(int y, int x, int amount, char dir, out Track last, Track attachTo = null)
        {
            var first = BuildTrack<Track>(y, x);

            var currentTrack = first;
            for (var i = 1; i < amount; i++)
            {
                IncrementCoord(ref y, ref x, dir);
                currentTrack = BuildTrack<Track>(y, x, currentTrack);
            }
            last = currentTrack;
            last.Next = attachTo;
            return first;
        }

        private WareHouse<T> BuildWareHouse<T>(int y, int x) where T : Movable, new()
        {
            var piece = new WareHouse<T>();
            Locate(y, x, piece);
            return piece;
        }
        private T BuildTrack<T>(int y, int x, Track attachTo = default(T)) where T : Track, new()
        {
            var piece = new T { Next = attachTo };
            Locate(y, x, piece);
            return piece;
        }
    }
}

