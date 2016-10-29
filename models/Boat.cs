namespace models
{
    public class Boat : Movable
    {
        public Boat()
        {
            Cargo = 0;
        }

        public override bool IsCompleted
        {
            get { return Cargo >= 8; }
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override int Points
        {
            get { return 10; }
        }
    }
}

