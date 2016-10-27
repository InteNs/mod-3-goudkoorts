using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace presentation
{
    public class OutputView
    {
        public void DrawWelcome()
        {
            Console.WriteLine("Welcome");
        }

        public void ClearScreen()
        {
            Console.Clear();
        }

        public void DrawState(long ticks, int score)
        {
            Console.WriteLine("cycles: {0} | score: {1}", ticks, score);
        }

        public void DrawMap(int height, int width, ViewController reference)
        {
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    Console.ForegroundColor = y == 0 ? ConsoleColor.Cyan : ConsoleColor.Yellow;
                    Console.Write(reference.GettCharAt(y, x));
                }
                Console.Write("\n");
            }
        }
    }
}
