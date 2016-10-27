namespace models
{
    public class MergeTrack : SwitchTrack
    {
        public override bool CanEnter(Movable movable)
        {
            var connectedTrack = SwitchIsUp ? UpTrack : DownTrack;
            return movable.Track == connectedTrack;
        }

        public override void Accept(IVisitor visitor)
        {
            if (IsOccupied)
                Movable.Accept(visitor);
            else
                visitor.Visit(this);
        }
    }
}