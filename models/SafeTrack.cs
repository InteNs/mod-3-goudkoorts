namespace models
{
    public class SafeTrack : Track
    {
        public override bool CanEnter(Movable movable)
        {
            return !IsOccupied;
        }
    }
}

