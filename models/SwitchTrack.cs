namespace models
{
    public abstract class SwitchTrack : Track
    {
        public int Number { get; set; }
        public virtual Track UpTrack { get; set; }

        public virtual Track DownTrack { get; set; }

        public virtual bool SwitchIsUp { get; set; }

        public virtual void Flip()
        {
            SwitchIsUp = !SwitchIsUp;
        }
    }
}

