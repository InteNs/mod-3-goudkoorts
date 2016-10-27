using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using presentation;

namespace controllers
{
	using System.Collections.Generic;
    using models;

	public class GameController
	{
        public GameController()
        {
            Map = new Map();
            View = new ViewController{Objects = Map.LocationMap};
            Workers = new List<Worker>();
            Score = 0;
            Ticks = 0;
            InitializeWorkers();
            new Thread(Refresh).Start();
            Play();
        }
		public int Score { get; set; }

	    public long Ticks { get; set; }

        public ViewController View { get; set; } 

	    public List<Worker> Workers { get; set; }

        public Map Map { get; set; }

	    public virtual bool DoTick()
	    {
	        foreach (var worker in Workers)
	        {
	            worker.Work(Ticks);
	            if (!worker.Successfull) return false;
	            Score += worker.Result;
	        }
            Ticks++;
	        return true;
	    }

	    public void Play()
	    {
	        while (DoTick())
	        { 
                Thread.Sleep(1000);
	        }
            
	    }

	    public void Refresh()
	    {
            while (true)
            {
                CheckInput();
                View.DrawTick(Ticks, Score);
            }
	    }

	    private void CheckInput()
	    {
	        if (!Console.KeyAvailable) return;
	        var key = Console.ReadKey(true).KeyChar;
	        switch (key)
	        {
	            case '1': Map.Switch(1).Flip(); break;
	            case '2': Map.Switch(2).Flip(); break;
	            case '3': Map.Switch(3).Flip(); break;
	            case '4': Map.Switch(4).Flip(); break;
	            case '5': Map.Switch(5).Flip(); break;
	            default : break;
	        }
	    }

		public void InitializeWorkers()
		{
            Workers.Add(new DockWorker { Input = Map.DockTrackIn, Output = Map.DocktrackOut });

            var cartWorker = HireMovableWorker(Map.CartStarts);
            cartWorker.SpawnAlgorithm = cartWorker.TimeLinear;

            var boatWorker = HireMovableWorker(Map.BoatStarts);
		    boatWorker.SpawnAlgorithm = boatWorker.BasedOnCargo;

            
		}

	    private MovableWorker<T> HireMovableWorker<T>(List<WareHouse<T>> wareHouses)
            where T : Movable, new()
	    {
	        var worker = new MovableWorker<T> {WareHouses = wareHouses};
	        Workers.Add(worker);
	        return worker;
	    }
	}
}

