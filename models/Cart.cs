namespace models
{
    public class Cart : Movable
    {
        public Cart()
        {
            Cargo = 2;
        }

        public override int PointsIfComplete
        {
            get { return 1; }
        }

        public override bool IsCompleted
        {
            get { return Cargo <= 0; }
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}

