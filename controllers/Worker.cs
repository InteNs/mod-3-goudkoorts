namespace controllers
{
    public abstract class Worker
    {
        private int _result;
        private bool _success;
        public abstract void Work(long ticks);

        public int Result
        {
            get
            {
                var result = _result;
                _result = 0;
                return result;
            }
            set { _result = value; }
        }

        public bool Successfull
        {
            get
            {
                var success = _success;
                _success = true;
                return success;
            }
            set { _success = value; }
        }
    }
}

