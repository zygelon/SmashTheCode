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
    public const int ROWS = 6;
    public const int COLUMNS = 12;
    private string[] grid;
    private bool isKnownNextBalls;
    public Color[] NextBalls { get; private set; }
    public int Score { get; private set; }
    public int[] HeightBalls { get; private set; }

    public Player(Color[] NextBalls=null){
        HeightBalls = new int[ROWS];
        for (int i = 0; i < ROWS; ++i)
            HeightBalls[i] = -1;
        Score = 0;
        grid = new string[COLUMNS];
        if (NextBalls == null)
        {
            isKnownNextBalls = false;
            this.NextBalls = new Color[8];
        }
        else
        {
            this.NextBalls = NextBalls;
            isKnownNextBalls = true;
        }
    }

    public void UpdateInput()
    {
        for (int i = 0; i < ROWS; ++i)
            HeightBalls[i] = -1;
        if (!isKnownNextBalls)
            for (int i = 0; i < 8; i++)
            {
                string[] temp = Console.ReadLine().Split(' ');
                NextBalls[i].ChangeColor(int.Parse(temp[0]), int.Parse(temp[1]));
            }
        Score = int.Parse(Console.ReadLine());
        for (int i = 0; i < COLUMNS; ++i)
        {
            grid[i] = Console.ReadLine();
            for (int j = 0; j < ROWS; ++j)
                if (HeightBalls[j] == -1 && grid[i][j] != '.') HeightBalls[j] = COLUMNS - 1 - i;
        }
    }
    public void ShowInputInformation()
    {
        Console.Error.Write("Balls:");
        foreach (var t in NextBalls)
            Console.Error.Write(t.a + t.b + " ");
        Console.Error.WriteLine();

        Console.Error.WriteLine("Score="+Score);

        Console.Error.Write("HeightBalls:");
        foreach (var t in HeightBalls)
            Console.Error.Write(t + " ");
        Console.Error.WriteLine();

        for (int i = 0; i < COLUMNS; ++i)
            Console.Error.WriteLine(grid[i]);
        Console.Error.WriteLine();
    }
}
class BaseStrategy
{
    //virtual protected int Score(int b,int cb,int cp,int gb)//b - ощичено блоков за этот ход, cp=chain power
    //CP is the chain power, starting at 0 for the first step. It is worth 8 for the second step and for each following step it is worth twice as much as the previous step.
        //CB is the Color Bonus, depending on how many different color blocks were cleared in the step. It is worth 
        //GB is the Group Bonus, depending on how many blocks are destroyed per group. When several groups have been cleared, the bonuses of all groups are summed. It is worth 
        //The value of(CP + CB + GB) is limited to between 1 and 999 inclusive.

    
}

class Program {
    static void Main(string[] args)
    {
        Player me = new Player();
        Player enemy = new Player(me.NextBalls);
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