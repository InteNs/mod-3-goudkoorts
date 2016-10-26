﻿namespace models
{
    public class Boat : Movable
    {
        public Boat()
        {
            Cargo = 0;
        }

        public override bool IsCompleted
        {
            get { return Cargo >= 16; }
        }

        public override int PointsIfComplete
        {
            get { return 18; }
        }
    }
}

