namespace controllers
{
	using models;

	public class DockWorker : Worker
	{
		public virtual Track Input { get; set; }

	    public virtual Track Output { get; set; }

	    public override void Work(long ticks)
	    {
	        if (Input.IsOccupied && Output.IsOccupied)
	        {
	            Input.Movable.Cargo--;
	            Output.Movable.Cargo++;
	        }
	    }
	}
}

