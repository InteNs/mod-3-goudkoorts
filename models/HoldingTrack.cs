namespace models
{
    public class HoldingTrack : Track
    {
        public override bool CanLeave()
        {
            return Movable.IsCompleted;
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

