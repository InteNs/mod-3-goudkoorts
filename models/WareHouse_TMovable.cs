namespace models
{
    public class WareHouse<TMovable>
        where TMovable : Movable, new()
    {
        public virtual Track Track { get; set; }

        public virtual TMovable SpawnMovable()
        {
            var movable = new TMovable();
            Track.Movable = movable;
            return movable;
        }
    }
}

