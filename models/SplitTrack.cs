namespace models
{
    public class SplitTrack : SwitchTrack
    {
        public override Track Next
        {
            get { return SwitchIsUp ? UpTrack : DownTrack; }
        }
    }
}

