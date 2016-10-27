using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace models
{
    public interface IVisitor
    {
        void Visit(Track visitee);
        void Visit(HoldingTrack visitee);
        void Visit(MergeTrack visitee);
        void Visit(SplitTrack visitee);
        void Visit(SafeTrack visitee);
        void Visit(SeaTrack visitee);
        void Visit(WareHouse<Boat> visitee);
        void Visit(WareHouse<Cart> visitee);
        void Visit(Cart visitee);
        void Visit(Boat visitee);
    }
}
