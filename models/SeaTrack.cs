using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace models
{
    public class SeaTrack : SafeTrack
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
