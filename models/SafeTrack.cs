﻿namespace models
{
    public class SafeTrack : Track
    {
        public override bool CanLeave()
        {
            return Next != null && !Next.IsOccupied;
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

