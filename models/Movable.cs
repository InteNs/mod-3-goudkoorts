namespace models
{
    public abstract class Movable
    {
        private Track _track;

        public int Cargo { get; set; }

        public virtual int PointsIfComplete { get { return 0; } }

        public virtual bool IsCompleted
        {
            get { return false; }
        }

        public virtual Track Track
        {
            get { return _track; }
            set {
                _track = value;
                if(_track != null) _track.Movable = this;
            }
        }

        public bool CanDelete
        {
            get { return IsCompleted && Track == null; }
        }

        private bool CanMove
        {
            get{ return Track.Next == null || Track.CanLeave() && Track.Next.CanEnter(this); }
        }

        public bool Move()
        {
            if (!CanMove) return true;
            if (Track.Next.IsOccupied) return false;

            Track.Movable = null;
            Track = Track.Next;
            return true;
        }
    }
}

