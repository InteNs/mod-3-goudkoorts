using System;
using System.CodeDom;
using System.Dynamic;
using System.Linq;
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
            Switches = new List<SwitchTrack>(5);
            BoatStarts = new List<WareHouse<Boat>>(1);
            CartStarts = new List<WareHouse<Cart>>(3);
            LocationMap = new IVisitable[9,12];
            Generate();
        }
        public IVisitable[,] LocationMap { get; set; }

        public virtual Track DockTrackIn { get; set; }

        public Track DocktrackOut { get; set; }

        public List<WareHouse<Cart>> CartStarts { get; set; }

        public List<WareHouse<Boat>> BoatStarts { get; set; }

        public List<SwitchTrack> Switches { get; set; }

        public SwitchTrack Switch(int number)
        {
            return Switches.FirstOrDefault(s => s.Number == number);
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
            BoatStarts.Add(BuildWareHouse<Boat>(0, 0));
            
            CartStarts.Add(BuildWareHouse<Cart>(3, 0));
            CartStarts.Add(BuildWareHouse<Cart>(5, 0));
            CartStarts.Add(BuildWareHouse<Cart>(7, 0));
        }

        private void InitSwitches()
        {
            Switches.Add(BuildSwitch<MergeTrack>(4, 3, 1, true));
            Switches.Add(BuildSwitch<SplitTrack>(4, 5, 2, true));
            Switches.Add(BuildSwitch<MergeTrack>(4, 9, 3, true));
            Switches.Add(BuildSwitch<MergeTrack>(6, 6, 4, false));
            Switches.Add(BuildSwitch<SplitTrack>(6, 8, 5, false));
        }

        private void InitTrackLists()
        {
            /*******************_dockTracks**/
            Track last;
            //1 normal tracks
            _dockTracks = BuildTrackList<Track>(4, 10, 1, 'E', out last);
            //4 normal tracks
            last.Next = BuildTrackList<Track>(4, 11, 4, 'N', out last);
            //1 normal tracks
            last.Next = BuildTrackList<Track>(1, 10, 1, 'W', out last);
            //1 holding tracks
            last = BuildTrack<HoldingTrack>(1, 9, previous: last);
            DockTrackIn = last;
            //9 normal tracks
            last.Next = BuildTrackList<Track>(1, 8, 9, 'W', out last);

            /*******************_safeTracks**/
            //3 normal tracks
            _safeTracks = BuildTrackList<Track>(7, 8, 3, 'E', out last);
            //2 normal tracks
            last.Next = BuildTrackList<Track>(7, 11, 2, 'S', out last);
            //2 normal tracks
            last.Next = BuildTrackList<Track>(8, 10, 2, 'W', out last);
            //8 safe tracks
            last.Next = BuildTrackList<SafeTrack>(8, 8, 8, 'W', out last);

            /*******************_seaTracks**/
            //8 normal tracks
            _seaTracks = BuildTrackList<SeaTrack>(0, 1, 8, 'E', out last);
            //1 holding track
            last = BuildTrack<HoldingTrack>(0, 9, previous: last);
            DocktrackOut = last;
            //2 normal tracks
            last.Next = BuildTrackList<SeaTrack>(0, 10, 2, 'E', out last);
        }

        private void MapTracks()
        {
            BoatStarts[0].Track = _seaTracks;

            Switch(5).DownTrack = _safeTracks;
            Switch(3).Next = _dockTracks;

            Track last;
            //B -> 1
            CartStarts[0].Track = BuildTrackList<Track>(3, 1, 3, 'E', out last);
            last.Next = Switch(1);
            Switch(1).UpTrack = last;

            //C -> 1
            CartStarts[1].Track = BuildTrackList<Track>(5, 1, 3, 'E', out last);
            last.Next = Switch(1);
            Switch(1).DownTrack = last;
            //D -> 4
            CartStarts[2].Track = BuildTrackList<Track>(7, 1, 6, 'E', out last);
            last.Next = Switch(4);
            Switch(4).DownTrack = last;
            //2 -> 4
            Switch(2).DownTrack = BuildTrackList<Track>(5, 5, 2, 'E', out last);
            last.Next = Switch(4);
            Switch(4).UpTrack = last;
            //2 -> 5
            Switch(2).UpTrack = BuildTrackList<Track>(3, 5, 5, 'E', out last);
            last.Next = Switch(3);
            Switch(3).UpTrack = last;
            //5 -> 3
            Switch(5).UpTrack = BuildTrackList<Track>(5, 8, 2, 'E', out last);
            last.Next = Switch(3);
            Switch(3).DownTrack = last;

            //1 -> 2
            Switch(1).Next = BuildTrack<Track>(4, 4, Switch(2));
            
            //4 -> 5
            Switch(4).Next = BuildTrack<Track>(6, 7, Switch(5));
        }

        /*************HELPER METHODS FOR GENERATION*************/

        private void Locate(int y, int x, IVisitable value)
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

        private char OrientationForDir(char dir)
        {
            if (dir == 'N' || dir == 'S') return 'V';
            if (dir == 'E' || dir == 'W') return 'H';
            return ' ';
        }

        private T BuildTrackList<T>(int y, int x, int amount, char dir, out Track last) where T : Track, new()
        {
            var first = BuildTrack<T>(y, x);
            first.Orientation = OrientationForDir(dir);
            var currentTrack = first;
            for (var i = 1; i < amount; i++)
            {
                IncrementCoord(ref y, ref x, dir);
                var newTrack = BuildTrack<T>(y, x);
                newTrack.Orientation = OrientationForDir(dir);
                currentTrack.Next = newTrack;
                currentTrack = newTrack;
            }
            last = currentTrack;
            return first;
        }

        private WareHouse<T> BuildWareHouse<T>(int y, int x) where T : Movable, new()
        {
            var piece = new WareHouse<T>();
            Locate(y, x, piece);
            return piece;
        }
        private T BuildTrack<T>(int y, int x, Track next = default(T), Track previous = null) where T : Track, new()
        {
            var piece = new T { Next = next };
            if (previous != null) previous.Next = piece;
            Locate(y, x, piece);
            return piece;
        }

        private T BuildSwitch<T>(int y, int x, int number, bool isUp) where T : SwitchTrack, new()
        {
            var piece = new T { Number = number, SwitchIsUp = isUp};
            Locate(y, x, piece);
            return piece;
        }
    }
}

