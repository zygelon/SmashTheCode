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
    public void ChangeColor(int a,int b)
    {
        this.a = a;
        this.b = b;
    }
}

class Player
{
    private string[] grid;
    private bool isKnownNextBalls;
    public int Score { get; private set; }
    public List<Color> nextBalls;

    public Player(){
        isKnownNextBalls = false;
        Score = 0;
        grid = new string[12];
        nextBalls = new List<Color>();
        for (int i = 0; i < 8; ++i) nextBalls.Add(new Color(0, 0));
    }
    public Player(List<Color>nextBalls){
        isKnownNextBalls = true;
        Score = 0;
        grid = new string[12];
        this.nextBalls = nextBalls;
    }

    public void UpdateInput()
    {
        if (!isKnownNextBalls)
            for (int i = 0; i < 8; i++)
            {
                string[] temp = Console.ReadLine().Split(' ');
                nextBalls[i].ChangeColor(int.Parse(temp[0]), int.Parse(temp[1]));
            }
        Score = int.Parse(Console.ReadLine());
        for (int i = 0; i < 12; ++i)
            grid[i] = Console.ReadLine();
    }
    public void ShowInputInformation()
    {
        Console.Error.Write("Balls:");
        foreach (var t in nextBalls)
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
        Player enemy = new Player(me.nextBalls);
        while (true)
        {
            me.UpdateInput();
            enemy.UpdateInput();
            me.ShowInputInformation();
            enemy.ShowInputInformation();
            Console.WriteLine("0 1");
        }
    }
}