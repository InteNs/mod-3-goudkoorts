using System.Linq;

namespace controllers
{
	using models;
	using System;
	using System.Collections.Generic;

	public class MovableWorker<TMovable> : Worker
		where TMovable : Movable, new()
	{
	    private int TicksSinceLastSpawn { get; set; }
        private int BaseInterval { get; set; }

	    public Func<long, bool> SpawnAlgorithm;
		public virtual List<WareHouse<TMovable>> WareHouses { get; set; }

	    public virtual List<Movable> Movables { get; set; }

	    public bool TimeLinear(long ticks)
	    {
	        var interval = BaseInterval - ticks/50;
	        return TicksSinceLastSpawn >= interval;
	    }

	    public bool WhenAllCompleted(long ticks)
	    {
	        return Movables.TrueForAll(m => m.IsCompleted);
	    }

	    private bool CanSpawnThisTick(long ticks)
	    {
	        return SpawnAlgorithm(ticks);
	    }

		public override void Work(long tickCount)
		{
            //remove any completed and disappeared movables
		    foreach (var movable in Movables.Where(m => m.CanDelete).ToList())
		    {
		        Result += movable.PointsIfComplete;
		        Movables.Remove(movable);
		    }

            //move all movables
            foreach (var movable in Movables)
            {
                if (!movable.Move()) Successfull = false;
            }

            //add new movables
		    if ( CanSpawnThisTick(tickCount))
		    {
		        var nextIndex = new Random().Next(0, WareHouses.Count);
		        WareHouses[nextIndex].SpawnMovable();
		        TicksSinceLastSpawn = 0;
		    }
		    else
		    {
		        TicksSinceLastSpawn++;
		    }
		}
	}
}

