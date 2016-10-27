using System;

namespace models
{
    public class WareHouse<TMovable> : IVisitable
        where TMovable : Movable, new()
    {
        public virtual Track Track { get; set; }

        public virtual TMovable SpawnMovable()
        {
            var movable = new TMovable{Track = Track};
            Track.Movable = movable;
            return movable;
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit((dynamic)this);
        }
    }
}

