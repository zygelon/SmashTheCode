using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

struct Color
{
    public int a, b;
    public Color(int a,int b)
    {
        this.a = a;
        this.b = b;
    }
}

class Player
{
    private string[] grid;
    public int Score { get; private set; }
    public List<Color> NextBalls { get; private set; }
    public Player(){
        Score = 0;
        grid = new string[12];
        NextBalls = new List<Color>();
    }
    public Player(List<Color>nextBalls){
        Score = 0;
        grid = new string[12];
        this.NextBalls = nextBalls;
    }

    public void UpdateInput()
    {
        if (NextBalls.Count == 0)
            for (int i = 0; i < 8; i++)
            {
                string[] temp = Console.ReadLine().Split(' ');
                NextBalls.Add(new Color(Int32.Parse(temp[0]), Int32.Parse(temp[1])));
            }
        Score = int.Parse(Console.ReadLine());
        for (int i = 0; i < 12; ++i)
            grid[i] = Console.ReadLine();
    }
    public void ShowInputInformation()
    {
        Console.Error.Write("Balls:");
        foreach (var t in NextBalls)
            Console.Error.Write(t.a + t.b + " ");
        Console.Error.WriteLine();
        Console.Error.WriteLine("Score="+Score);
        for (int i = 0; i < 12; ++i)
            Console.Error.WriteLine(grid[i]);
        Console.Error.WriteLine();
    }
}
class Program {
    static void Main(string[] args)
    {
        Player me = new Player();
        Player enemy = new Player(me.NextBalls);
        me.UpdateInput();
        enemy.UpdateInput();
        me.ShowInputInformation();
        enemy.ShowInputInformation();
    }
}