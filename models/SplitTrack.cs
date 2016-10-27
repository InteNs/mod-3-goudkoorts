namespace models
{
    public class SplitTrack : SwitchTrack
    {
        private Track _upTrack;
        private Track _downTrack;

        public override Track Next
        {
            get { return SwitchIsUp ? UpTrack : DownTrack; }
        }

        public override void Accept(IVisitor visitor)
        {
            if (IsOccupied)
                Movable.Accept(visitor);
            else
                visitor.Visit(this);
        }

        public override Track UpTrack
        {
            get { return _upTrack; }
            set
            {
                if ((_upTrack = value) != null) _upTrack.Previous = this;

            }
        }

        public override Track DownTrack
        {
            get { return _downTrack; }
            set
            {
                if ((_downTrack = value) != null) _downTrack.Previous = this;

            }
        }
    }
}

