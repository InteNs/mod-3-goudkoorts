namespace models
{
    public class Cart : Movable
    {
        public Cart()
        {
            Cargo = 2;
        }

        public override bool IsCompleted
        {
            get { return Cargo <= 0; }
        }
    }
}

