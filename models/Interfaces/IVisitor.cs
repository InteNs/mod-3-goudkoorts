namespace models
{
    public interface IVisitor
    {
        void Visit(Track visitee);
        void Visit(HoldingTrack visitee);
        void Visit(MergeTrack visitee);
        void Visit(SplitTrack visitee);
        void Visit(SafeTrack visitee);
        void Visit(NoCollisionTrack visitee);
        void Visit(Path<Boat> visitee);
        void Visit(Path<Cart> visitee);
        void Visit(Cart visitee);
        void Visit(Boat visitee);
    }
}
