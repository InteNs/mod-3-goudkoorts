namespace models
{
    public abstract class Movable : IVisitable
    {
        private Track _track;

        public int Cargo { get; set; }

        public virtual int Points { get { return 0; } }

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
            get { return Track == null; }
        }

        public bool Move()
        {
            //this cart is out of the map -> SKIP
            if (Track == null) return true;
            //this cart can't leave -> SKIP
            if (!Track.CanLeave()) return true;
            //this cart can't enter the next track -> SKIP
            if (Track.Next != null && !Track.Next.CanEnter(this)) return true;
            //this cart has a collision -> ABORT
            if (Track.Next != null && Track.Next.IsOccupied) return false;

            //move
            Track.Movable = null;
            Track = Track.Next;
            return true;
        }

        public abstract void Accept(IVisitor visitor);
    }
}

