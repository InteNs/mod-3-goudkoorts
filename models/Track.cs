namespace models
{
    public class Track
    {
        public virtual bool IsOccupied
        {
            get { return Movable != null; }
        }

        public virtual Movable Movable { get; set; }

        public virtual Track Next { get; set; }

        public virtual bool CanEnter(Movable movable)
        {
            return true;
        }

        public virtual bool CanLeave()
        {
            return true;
        }

    }
}

