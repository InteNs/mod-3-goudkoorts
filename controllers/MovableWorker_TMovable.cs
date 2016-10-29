using System.Linq;

namespace controllers
{
	using models;
	using System;
	using System.Collections.Generic;

	public class MovableWorker<TMovable> : Worker
		where TMovable : Movable, new()
	{
	    public MovableWorker()
	    {
	        TicksSinceLastSpawn = 0;
	        Successfull = true;
	        BaseInterval = 20;
            Movables = new List<Movable>();
	    }
	    private int TicksSinceLastSpawn { get; set; }
        private int BaseInterval { get; set; }
	    public Func<long, bool> SpawnAlgorithm;
		public List<Path<TMovable>> Paths { get; set; }
	    public List<Movable> Movables { get; set; }

	    private bool CanSpawnThisTick(long ticks)
	    {
	        return SpawnAlgorithm(ticks);
	    }

	    private int NextSpawn()
	    {
	        return new Random().Next(0, Paths.Count);
	    }

		public override void Work(long tickCount)
		{
            //remove any completed and disappeared movables
		    foreach (var movable in Movables.Where(m => m.CanDelete).ToList())
		    {
		        if (movable.IsCompleted) Result += movable.Points;
		        Movables.Remove(movable);
		    }

            //move all movables
            foreach (var movable in Movables)
            {
                var result = movable.Move();
                if (result == false) Successfull = false;
            }

            //add new movables
		    if ( CanSpawnThisTick(tickCount))
		    {
		        Movables.Add(Paths[NextSpawn()].SpawnMovable());
		        TicksSinceLastSpawn = 0;
		    }
		    else
		    {
		        TicksSinceLastSpawn++;
		    }
		}

        /***********spawn algorithnms****/
        public bool Static(long ticks)
        {
            return ticks % 5 == 0;
        }

        public bool AllCompleted(long ticks)
        {
            return Movables.TrueForAll(m => m.IsCompleted);
        }

        public bool EveryTick(long ticks)
        {
            return ticks % 2 == 0;
        }

        public bool TimeLinear(long ticks)
        {
            var interval = BaseInterval - 15 - ticks / 50;
            return TicksSinceLastSpawn >= interval;
        }
	}
}

