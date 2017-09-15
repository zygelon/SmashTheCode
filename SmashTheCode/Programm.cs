using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

struct Coor
{
    public int x, y;
    public Coor(int x,int y)
    {
        this.x = x;
        this.y = y;
    }
}

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

class MyStack<T>
{
    public int Length { get; private set; }
    int index;
    T[] stack;
    public MyStack(int Length) : base()
    {
        this.Length = Length;
        index = 0;
        stack = new T[Length];
    }
    public MyStack()
    {
        Length = 30;
        index = 0;
        stack = new T[Length];
    }
    void IncreaseStack()
    {
        T[] newStack = new T[Length * 2];
        for (int i = 0; i < Length; ++i)
            newStack[i] = stack[i];
        Length *= 2;
        stack = newStack;
    }
    public void Push(T element)
    {
        if (index == Length) IncreaseStack();
        stack[index++] = element;
    }
    public bool IsEmpty()
    {
        if (index == 0) return true;
        return false;
    }
    public T Pop()
    {
        if (index == 0)
        {
            Console.WriteLine("Error: stack is empty");
            return stack[0];
        }
        return stack[--index];
    }
    public void Show()
    {
        Console.WriteLine("index=" + index);
        foreach (T el in stack)
            Console.Write(el + " ");
        Console.WriteLine();
    }
}
enum Rotation { Right,Up,Left,Down};

class Player
{
    public const int ROWS = 6, COLUMNS = 12;
    private char[,] Grid; 
    private bool isKnownNextBalls;
    public Color[] NextBalls { get; private set; }
    public int Score { get; private set; }
    public int[] HeightBalls;

    public Player(Color[] NextBalls=null){
        HeightBalls = new int[ROWS];
        Score = 0;
        Grid = new char[COLUMNS,ROWS];
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
            string s = Console.ReadLine();
            for (int j = 0; j < ROWS; ++j)
            {
                Grid[i, j] = s[j];
                if (HeightBalls[j] == 0 && Grid[i, j] != '.') HeightBalls[j] = COLUMNS - i;
            }
        }
    }
    public void ShowInformation()
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
                Console.Error.Write(Grid[i, j] + " ");
            Console.Error.WriteLine();
        }
    }
    public char[,] GetGridCopy()
    {
        char[,] retGrid=new char[COLUMNS,ROWS];
        for (int i = 0; i < COLUMNS; ++i)
            for (int j = 0; j < ROWS; ++j)
                retGrid[i, j] = Grid[i, j];
        return retGrid;
    }

    public int[] GetHeightBallsCopy()
    {
        int[] retHeightBalls = new int[ROWS];
        HeightBalls.CopyTo(retHeightBalls, 0);
        return retHeightBalls;
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
    int[] HeightBalls { get; set; }
    int B, CP, CB, GB;
    public Simulate(char[,] currentGrid,int[] HeightBalls)
    {
        this.HeightBalls = HeightBalls;
        this.currentGrid = currentGrid;
        this.currentGrid = currentGrid;
        B = 0; CP = 0; CB = 0; GB = 0;
    }
    public void SimulateMove(Color ball, int x, Rotation rot)
    {
        if (x == 0 && rot == Rotation.Left || x == ROWS - 1 && rot == Rotation.Right)
        {
            Console.Error.WriteLine("A ball isn't in the Grid");
            return;
        }
        switch (rot)
        {
            case Rotation.Down:
                currentGrid[COLUMNS - 1 - HeightBalls[x], x] = (char)(ball.b + '0');
                ++HeightBalls[x];
                currentGrid[COLUMNS - 1 - HeightBalls[x], x] = (char)(ball.a + '0');
                ++HeightBalls[x];
                break;
            case Rotation.Up:
                currentGrid[COLUMNS - 1 - HeightBalls[x], x] = (char)(ball.a + '0');
                ++HeightBalls[x];
                currentGrid[COLUMNS - 1 - HeightBalls[x], x] = (char)(ball.b + '0');
                ++HeightBalls[x];
                break;
            case Rotation.Left:
                currentGrid[COLUMNS - 1 - HeightBalls[x], x] = (char)(ball.a + '0');
                ++HeightBalls[x];
                currentGrid[COLUMNS - 1 - HeightBalls[x - 1], x - 1] = (char)(ball.b + '0');
                ++HeightBalls[x - 1];
                break;
            case Rotation.Right:
                currentGrid[COLUMNS - 1 - HeightBalls[x], x] = (char)(ball.a + '0');
                ++HeightBalls[x];
                currentGrid[COLUMNS - 1 - HeightBalls[x + 1], x + 1] = (char)(ball.b + '0');
                ++HeightBalls[x + 1];
                break;
        }
        Console.WriteLine("IsNeedDestroy=" + IsNeedDestroy(x, COLUMNS -  HeightBalls[x]));
    }
    void FallCurrentBall(int x,int y)
    {
        currentGrid[COLUMNS - HeightBalls[x], x] = currentGrid[y, x];
        currentGrid[y, x] = '.';
    }
    bool IsNeedDestroy(int x,int y)
    {
        char selectedColor = currentGrid[y, x];
        if (selectedColor == '.')
        {
            for (int i = 0; i < 30; ++i)
                Console.Error.WriteLine("ERROR: in IsNeedDestroy selectedColor is '.'");
            Console.Error.WriteLine("y=" + y + " x=" + x);
        }
        bool[,] used = new bool[COLUMNS, ROWS];
        MyStack<Coor> way = new MyStack<Coor>();
        way.Push(new Coor(x, y));
        int count = 0;
        while (!way.IsEmpty())
        {
            Coor cur = way.Pop();
            if (used[cur.y, cur.x]) continue;
            used[cur.y, cur.x] = true;
            ++count;
            if (count >= 4) return true;
            if (cur.y + 1 < COLUMNS && !used[cur.y + 1, cur.x] && currentGrid[cur.y+1,cur.x]==selectedColor)
                way.Push(new Coor(cur.x, cur.y + 1));

            if (cur.y - 1 >= 0 && !used[cur.y - 1, cur.x] && currentGrid[cur.y-1,cur.x]==selectedColor)
                way.Push(new Coor(cur.x, cur.y - 1));

            if (cur.x + 1 < ROWS && !used[cur.y, cur.x + 1] && currentGrid[cur.y,cur.x+1]==selectedColor)
                way.Push(new Coor(cur.x + 1, cur.y));

            if (cur.x - 1 >= 0 && !used[cur.y, cur.x - 1] && currentGrid[cur.y,cur.x-1]==selectedColor)
                way.Push(new Coor(cur.x - 1, cur.y));
        }
        return false;
    }
    void Destroy(int x,int y)
    {
    }
    public void ShowInformation()
    {

        Console.Error.WriteLine("Simulate");
        for (int i = 0; i < COLUMNS; ++i)
        {
            for (int j = 0; j < ROWS; ++j)
                Console.Error.Write(currentGrid[i, j] + " ");
            Console.Error.WriteLine();
        }
        Console.Error.Write("HeightBalls:");
        foreach (var el in HeightBalls)
            Console.Error.Write(el + " ");
        Console.Error.WriteLine();
    }
}


class Program
{
    static void Main()
    {
        Player me = new Player();
        Player enemy = new Player(me.NextBalls);
        me.UpdateInput();
        me.ShowInformation();
        Simulate analize = new Simulate(me.GetGridCopy(), me.GetHeightBallsCopy());
        analize.SimulateMove(me.NextBalls[0], 3, Rotation.Right);
        analize.ShowInformation();
    }
}