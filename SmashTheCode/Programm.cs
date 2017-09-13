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

enum Rotation { Right,Up,Left,Down};

class Player
{
    public const int ROWS = 6, COLUMNS = 12;
    private char[,] grid;
    private bool isKnownNextBalls;
    public Color[] NextBalls { get; private set; }
    public int Score { get; private set; }
    public int[] HeightBalls { get; private set; }

    public Player(Color[] NextBalls=null){
        HeightBalls = new int[ROWS];
        Score = 0;
        grid = new char[COLUMNS,ROWS];
        if (NextBalls == null)
        {
            Console.Error.WriteLine("22222");
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
            HeightBalls[i] = 0;
        if (!isKnownNextBalls)
            for (int i = 0; i < 8; i++)
            {
                string[] temp = Console.ReadLine().Split(' ');
                NextBalls[i].ChangeColor(int.Parse(temp[0]), int.Parse(temp[1]));
            }
        Score = int.Parse(Console.ReadLine());
        for (int i = 0; i < COLUMNS; ++i)
        {
            for (int j = 0; j < ROWS; ++j)
            {
                grid[i, j] = (char)Console.Read();
                if (HeightBalls[j] == 0 && grid[i, j] != '.') HeightBalls[j] = COLUMNS - i;
            }
            Console.Read();//ignore end of line
        }
    }
    public void ShowInputInformation()
    {
        Console.Error.Write("Balls:");
        foreach (var t in NextBalls)
            Console.Error.Write(t.a +""+t.b + " ");
        Console.Error.WriteLine();

        Console.Error.WriteLine("Score="+Score);

        Console.Error.Write("HeightBalls:");
        foreach (var el in HeightBalls)
            Console.Error.Write(el + " ");
        Console.Error.WriteLine();

        for (int i = 0; i < COLUMNS; ++i)
        {
            for (int j = 0; j < ROWS; ++j)
                Console.Error.Write(grid[i, j] + " ");
            Console.Error.WriteLine();
        }
    }
}
    //b - ощичено блоков за этот ход, cp=chain power
    //CP is the chain power, starting at 0 for the first step. It is worth 8 for the second step and for each following step it is worth twice as much as the previous step.
        //CB is the Color Bonus, depending on how many different color blocks were cleared in the step. It is worth 
        //GB is the Group Bonus, depending on how many blocks are destroyed per group. When several groups have been cleared, the bonuses of all groups are summed. It is worth 
        //The value of(CP + CB + GB) is limited to between 1 and 999 inclusive.
class Simulate
{
    private const int ROWS = 6, COLUMNS = 12;
    char[,] currentGrid;
    int B, CP, CB, GB;
    Simulate(char[,] currentGrid)
    {
        this.currentGrid = currentGrid;
        B = 0; CP = 0; CB = 0; GB = 0;
    }
    void SimulateMove(int x,Rotation rot)
    {
        if(x==0 && rot==Rotation.Left || x==ROWS-1 && rot==Rotation.Right)
        {
            Console.Error.WriteLine("A ball isn't in the Grid");
            return;
        }
    }
    void FindAndDestroy(int x,int y)
    {

    }
}
    

class Program {
    static void Main()
    {
        Player me = new Player();
        while (true)
        {
            me.UpdateInput();
            me.ShowInputInformation();
        Player enemy = new Player(me.NextBalls);
            enemy.UpdateInput();
            enemy.ShowInputInformation();
            Console.WriteLine("0 1");
        }
    }
}