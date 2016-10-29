namespace models
{
    public class NoCollisionTrack : SafeTrack
    {
        public override bool CanLeave()
        {
            if (Next == null) return true;
            return !Next.IsOccupied;
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
