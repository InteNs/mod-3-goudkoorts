namespace models
{
    public class HoldingTrack : Track
    {
        public override bool CanLeave()
        {
            return Movable.IsCompleted;
        }
    }
}

