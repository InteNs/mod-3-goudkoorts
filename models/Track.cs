namespace models
{
    public class Track : IVisitable
    {
        private Track _next;
        private Track _previous;

        public virtual bool IsOccupied
        {
            get { return Movable != null; }
        }

        public virtual Movable Movable { get; set; }

        public virtual char Orientation { get; set; }

        public virtual Track Next
        {
            get { return _next; }
            set
            {
                _next = value;
                if (_next != null)
                    _next.Previous = this;
            }
        }

        public virtual Track Previous
        {
            get { return _previous; }
            set { _previous = value; }
        }

        public virtual bool CanEnter(Movable movable)
        {
            return true;
        }

        public virtual bool CanLeave()
        {
            return true;
        }

        public virtual void Accept(IVisitor visitor)
        {
            if(IsOccupied)
                Movable.Accept(visitor);
            else 
                visitor.Visit(this);
        }
    }
}

