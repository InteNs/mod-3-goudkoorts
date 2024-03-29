﻿using System;
using System.Text;

namespace presentation
{
    public class OutputView
    {
        public OutputView()
        {
            //Console.SetWindowSize(38, 19);
            Console.Clear();
            Console.CursorVisible = false;
            
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

        public void DrawState(long ticks, int score, int countDown)
        {
            Console.SetCursorPosition(0,5);
            Console.WriteLine("│score: {0}| Next: {1} ", score, countDown);
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
                    Console.Write(reference.Visualize(y, x));
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
