using models;

namespace presentation
{
    public class ViewController : IVisitor
    {
        public IVisitable[,] Objects { get; set; }
        public OutputView View { get; set; }

        private string _fieldString;
        private long _ticks;

        public ViewController()
        {
            View = new OutputView();
        }

        public string Visualize(int y, int x)
        {
            var piece = Objects[y, x];
            if (piece == null) return "   ";
            piece.Accept(this);
            return _fieldString;
        }

        public void DrawTick(long ticks, int score, int countDown)
        {
            _ticks = ticks;
            View.DrawInfo();
            View.DrawState(ticks, score, countDown);
            View.DrawMap(Objects.GetLength(0), Objects.GetLength(1), this);
        }

        public void DrawEnd(int finalScore)
        {
            View.DrawEnd(finalScore);
        }

        /*********guestroom*********/

        public void Visit(Track visitee)
        {
            _fieldString = visitee.Orientation == 'V' ? " ║ " : "═══";
            if (visitee.Previous == null || visitee.Next == null) return;

            var next = visitee.Next;
            var previous = visitee.Previous;

            if (visitee.Orientation == 'V' && (next.Orientation == 'H' || previous.Orientation == 'H'))
            {
                _fieldString = "═╣ ";
                return;
            }

            if (next is MergeTrack)
            {
                _fieldString = ((SwitchTrack) next).UpTrack == visitee ? "═╗ " : "═╝ ";
                return;
            }

            if (previous is SplitTrack)
            {
                _fieldString = ((SwitchTrack) previous).UpTrack == visitee ? " ╔═" : " ╚═";
            }
        }


        public void Visit(HoldingTrack visitee)
        {
            _fieldString = "≈÷≈";
        }

        public void Visit(MergeTrack visitee)
        {
            var direction = visitee.SwitchIsUp ? "╚" : "╔";
            _fieldString = visitee.Number + direction + "═";
        }

        public void Visit(SplitTrack visitee)
        {
            var direction = visitee.SwitchIsUp ? "╝" : "╗";
            _fieldString = "═" + direction + visitee.Number;
        }

        public void Visit(SafeTrack visitee)
        {
            _fieldString = "╧══";
        }

        public void Visit(NoCollisionTrack visitee)
        {
            _fieldString = _ticks%2 == 0 ? "≈~≈" : "~─~";
        }

        public void Visit(Path<Boat> visitee)
        {
            _fieldString = ">├~";
        }

        public void Visit(Path<Cart> visitee)
        {
            _fieldString = ">╞═";
        }

        public void Visit(Cart visitee)
        {
            var cargoChar = visitee.Cargo == 0 ? "–" : "▀";
            _fieldString = "•" + cargoChar + "•";
        }

        public void Visit(Boat visitee)
        {
            _fieldString = "<" + visitee.Cargo + ">";
        }
    }
}