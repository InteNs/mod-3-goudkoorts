using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using models;

namespace presentation
{
    public class ViewController : IVisitor
    {
        public IVisitable[,] Objects { get; set; }
        public OutputView View { get; set; }

        private string _fieldString;

        public ViewController()
        {
            Console.OutputEncoding = Encoding.Unicode;
            View = new OutputView();
            View.DrawWelcome();
        }

        public string GettCharAt(int y, int x)
        {
            var piece = Objects[y, x];
            if (piece == null) return "   ";
            piece.Accept(this);
            return _fieldString;
        }

        public void DrawTick(long ticks, int score)
        {
            View.ClearScreen();
            View.DrawState(ticks, score);
            View.DrawMap(Objects.GetLength(0), Objects.GetLength(1), this);
        }

        /*********guestroom*********/

        public void Visit(Track visitee)
        {
            if (visitee.Next is MergeTrack)
            {
                _fieldString = ((SwitchTrack)visitee.Next).UpTrack == visitee ? "═╗ " : "═╝ ";
            } else if (visitee.Previous is SplitTrack)
            {
                _fieldString = ((SwitchTrack) visitee.Previous).UpTrack == visitee ? " ╔═" : " ╚═";
            }
            else
            {
                if (visitee.Previous != null && visitee.Next != null)
                {
                    if (visitee.Orientation == 'V' && (visitee.Previous.Orientation == 'H' || visitee.Next.Orientation == 'H'))
                    {
                        _fieldString = "═╣ "; return;
                    }
                }
                _fieldString = visitee.Orientation == 'V' ? " ║ " : "═══";
            }
        }

        public void Visit(HoldingTrack visitee)
        {
            _fieldString = "╡░╞";
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

        public void Visit(SeaTrack visitee)
        {
            _fieldString = "~≈~";
        }

        public void Visit(WareHouse<Boat> visitee)
        {
            _fieldString = "|B├";
        }

        public void Visit(WareHouse<Cart> visitee)
        {
            _fieldString = "|C╞";
        }

        public void Visit(Cart visitee)
        {
            string cargoChar;
            switch (visitee.Cargo)
            {
                case 1:
                    cargoChar = "◙";
                    break;
                case 2:
                    cargoChar = "■";
                    break;
                default:
                    cargoChar = "–";
                    break;
            }
            _fieldString = "◄" + cargoChar + "►";
        }

        public void Visit(Boat visitee)
        {
            _fieldString = visitee.Cargo.ToString("D2") + "►";
        }
    }
}
