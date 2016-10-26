using System.Data;
using System.Linq;

namespace controllers
{
	using System.Collections.Generic;
    using models;

	public class GameController
	{
        public GameController()
        {
            Map = new Map();
            Score = 0;
            InitializeWorkers();
            Play();
        }
		public int Score { get; set; }

	    public long Ticks { get; set; }

	    public List<Worker> Workers { get; set; }

        public Map Map { get; set; }

	    public virtual bool DoTick()
	    {
	        foreach (var worker in Workers)
	        {
	            worker.Work(Ticks);
	            if (!worker.Successfull) return false;
	            Score += worker.Result;
	            Ticks++;
	        }
	        return true;
	    }

	    public void Play()
	    {
            //drawstart
	        while (DoTick())
	        { 
                //draw stuff probably
	        }
            //drawend
	    }

		public void InitializeWorkers()
		{
            HireMovableWorker(Map.CartStarts);
            HireMovableWorker(Map.BoatStarts);

            Workers.Add(new DockWorker{Input = Map.DockTrackIn, Output = Map.DocktrackOut});
		}

	    private void HireMovableWorker<T>(List<WareHouse<T>> wareHouses)
            where T : Movable, new()
	    {
	        Workers.Add(new MovableWorker<T> {WareHouses = wareHouses});
	    }
	}
}

