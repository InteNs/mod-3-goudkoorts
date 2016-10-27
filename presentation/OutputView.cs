using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace presentation
{
    public class OutputView
    {
        public OutputView()
        {
            Console.SetWindowSize(38, 19);
            Console.CursorVisible = false;
            Console.OutputEncoding = Encoding.Unicode;
        }
        public void DrawInfo()
        {
            Console.SetCursorPosition(0,0);
            Console.WriteLine("┌─────────┬──────────────────────────┐");
            Console.WriteLine("│GoldFever│        X : Quit          │");
            Console.WriteLine("├─────────┘        R : Reset         │");
            Console.WriteLine("│                1-5 : Flip Switch   │");
            Console.WriteLine("├────────────────────────────────────┤");
        }

        public void DrawState(long ticks, int score)
        {
            Console.SetCursorPosition(0,5);
            Console.WriteLine("│cycles: {0} | score: {1}", ticks, score);
            Console.WriteLine("├────────────────────────────────────┤");
        }

        public void DrawMap(int height, int width, ViewController reference)
        {
            const int startLine = 7;
            for (var y = 0; y < height; y++)
            {
                Console.SetCursorPosition(0, startLine + y);
                Console.Write("│");
                for (var x = 0; x < width; x++)
                {
                    Console.Write(reference.GettCharAt(y, x));
                }
                Console.Write("│");
                Console.Write("\n");
            }
            Console.WriteLine("└────────────────────────────────────┘");
        }

        public void DrawEnd(int finalScore)
        {
            Console.SetCursorPosition(0, 5);
            Console.Write("│ GAME OVER, you scored {0} points", finalScore);
        }
    }
}
