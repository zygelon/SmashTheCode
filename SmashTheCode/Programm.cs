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
    public MyStack() : this(10) { }
    void IncreaseStack()
    {
        T[] newStack = new T[Length * 2];
        for (int i = 0; i < Length; ++i)
            newStack[i] = stack[i];
        Length *= 2;
        stack = newStack;
    }
    public void Add(T element)
    {
        if (index == Length) IncreaseStack();
        stack[index++] = element;
    }
    public bool IsEmpty()
    {
        if (index == 0) return true;
        return false;
    }
    public T Get()
    {
        if (index == 0)
        {
            Console.WriteLine("Error: stack is empty");
            return stack[0];
        }
        return stack[index-1];
    }
    public void Pop()
    {
        if (index == 0)
        {
            Console.WriteLine("Error: stack is empty");
        }
        --index;
        return;
    }
    public void Show()
    {
        Console.WriteLine("index=" + index);
        foreach (T el in stack)
            Console.Write(el + " ");
        Console.WriteLine();
    }
}
class MyQueue<T>
{
    int frontIndex;
    int backIndex;
    T[] queue;
    public int Length { get; private set; }

    public MyQueue()
    {
        frontIndex = 0;
        backIndex = 0;
        Length = 20;
        queue = new T[Length];
    }

    public MyQueue(int Length)
    {
        frontIndex = 0;
        backIndex = 0;
        this.Length = Length;
        queue = new T[Length];
    }

    public bool IsEmpty()
    {
        if (frontIndex == backIndex) return true;
        else return false;
    }

    public void IncreaseQueue()
    {
        T[] newQueue =new T[Length * 2];
        for (int i = frontIndex; i < backIndex; ++i)
            newQueue[i - frontIndex] = queue[i];
        backIndex -= frontIndex;
        frontIndex = 0;
        queue = newQueue;
        Length *= 2;
    }

    public void Add(T element)
    {
        if (backIndex == Length) IncreaseQueue();
        queue[backIndex++] = element;
    }

    public T Get()
    {
        if(backIndex==frontIndex)
        {
            Console.Error.WriteLine("ERROR QUEUE. BackIndex==FrontIndex. Get()");
        }
        return queue[frontIndex];
    }

    public void Pop()
    {
        if(backIndex==frontIndex)
        {
            Console.Error.WriteLine("ERROR QUEUE. BackIndex==FrontIndex. Pop()");
            return;
        }
        frontIndex++;
    }
}

enum Rotation { Right,Up,Left,Down};

static class Constants
{
    public const int Rows = 6, Columns = 12, CountColors=6;
}

abstract class BaseInformation
{
    public const int NULL=-1;
    protected char[,] Grid;
    protected int[] HeightBalls;

    protected BaseInformation() { }
    public abstract void ShowInformation();
}


class Player : BaseInformation
{
    Random rand;
    private bool isKnownNextBalls;
    public Color[] NextBalls { get; private set; }
    public int Score { get; private set; }

    public Player(Color[] NextBalls=null){
        rand = new Random();
        HeightBalls = new int[Constants.Rows];
        Score = 0;
        Grid = new char[Constants.Columns,Constants.Rows];
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

    public void StupidMove()
    {
        int num;
        do
        {
            num = rand.Next(0, 6);
        }
        while (HeightBalls[num] > 10);
       Console.WriteLine(num + " " + (int)Rotation.Down);
    }
    public void UpdateInput()
    {
        for (int i = 0; i < Constants.Rows; ++i)
            HeightBalls[i] = 0;
        if (!isKnownNextBalls)
            for (int i = 0; i < 8; i++)
            {
                string[] temp = Console.ReadLine().Split(' ');
                NextBalls[i].ChangeColor(int.Parse(temp[0]), int.Parse(temp[1]));
            }
        Score = int.Parse(Console.ReadLine());
        for (int i = 0; i < Constants.Columns; ++i)
        {
            string s = Console.ReadLine();
            for (int j = 0; j < Constants.Rows; ++j)
            {
                Grid[i, j] = s[j];
                if (HeightBalls[j] == 0 && Grid[i, j] != '.') HeightBalls[j] =Constants.Columns - i;
            }
        }
    }
    public override void ShowInformation()
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

        for (int i = 0; i < Constants.Columns; ++i)
        {
            for (int j = 0; j < Constants.Rows; ++j)
                Console.Error.Write(Grid[i, j] + " ");
            Console.Error.WriteLine();
        }
    }
    public char[,] GetGridCopy()
    {
        char[,] retGrid=new char[Constants.Columns,Constants.Rows];
        for (int i = 0; i < Constants.Columns; ++i)
            for (int j = 0; j < Constants.Rows; ++j)
                retGrid[i, j] = Grid[i, j];
        return retGrid;
    }

    public int[] GetHeightBallsCopy()
    {
        int[] retHeightBalls = new int[Constants.Rows];
        HeightBalls.CopyTo(retHeightBalls, 0);
        return retHeightBalls;
    }
}
    //b - ощичено блоков за этот ход, cp=chain power
    //CP is the chain power, starting at 0 for the first step. It is worth 8 for the second step and for each following step it is worth twice as much as the previous step.
        //CB is the Color Bonus, depending on how many different color blocks were cleared in the step. It is worth 
        //GB is the Group Bonus, depending on how many blocks are destroyed per group. When several groups have been cleared, the bonuses of all groups are summed. It is worth 
        //The value of(CP + CB + GB) is limited to between 1 and 999 inclusive.
class Simulate : BaseInformation
{
    int B, CP, CB, GB;
    int[] lyingBalls;
    bool[] isThisColorDestroyed;

    public Simulate(char[,] Grid,int[] HeightBalls)
    {
        this.HeightBalls = HeightBalls;
        this.Grid = Grid;
        this.Grid = Grid;
        isThisColorDestroyed = new bool[Constants.CountColors];
        B = 0; CP = 0; CB = 0; GB = 0;
        lyingBalls = new int[Constants.Rows];
        for (int i = 0; i < Constants.Rows; ++i)
            lyingBalls[i] = HeightBalls[i];
    }
    public int Score()
    {
        return B;
    }
    public void SimulateMove(Color ball, int posX, Rotation rot)
    {
        if (posX == 0 && rot == Rotation.Left || posX == Constants.Rows - 1 && rot == Rotation.Right)
        {
            for(int i=0;i<50;++i)
                Console.Error.WriteLine("A ball isn't in the Grid");
            return;
        }
        switch (rot)
        {
            case Rotation.Down:
                if (Constants.Columns - 3 - HeightBalls[posX] < 0) break;//костыль
                Grid[Constants.Columns - 1 - HeightBalls[posX], posX] = (char)(ball.b + '0');
                ++HeightBalls[posX];
                Grid[Constants.Columns - 1 - HeightBalls[posX], posX] = (char)(ball.a + '0');
                ++HeightBalls[posX];
                if (Grid[Constants.Columns - HeightBalls[posX] - 1, posX] != Grid[HeightBalls[posX], posX])
                    DestroyAndFall(posX, Constants.Columns - HeightBalls[posX] - 1);
                break;
            case Rotation.Up:
                Grid[Constants.Columns - 1 - HeightBalls[posX], posX] = (char)(ball.a + '0');
                ++HeightBalls[posX];
                Grid[Constants.Columns - 1 - HeightBalls[posX], posX] = (char)(ball.b + '0');
                ++HeightBalls[posX];
                if (Grid[Constants.Columns - HeightBalls[posX] + 1, posX] != Grid[HeightBalls[posX], posX])
                    DestroyAndFall(posX, Constants.Columns - HeightBalls[posX]);
                break;
            case Rotation.Left:
                Grid[Constants.Columns - 1 - HeightBalls[posX], posX] = (char)(ball.a + '0');
                ++HeightBalls[posX];
                Grid[Constants.Columns - 1 - HeightBalls[posX - 1], posX - 1] = (char)(ball.b + '0');
                ++HeightBalls[posX - 1];
                if (Grid[Constants.Columns - HeightBalls[posX - 1], posX - 1] != Grid[Constants.Columns - HeightBalls[posX], posX] || HeightBalls[posX - 1] != HeightBalls[posX])
                    DestroyAndFall(posX - 1, Constants.Columns - HeightBalls[posX - 1]);
                break;
            case Rotation.Right:
                Grid[Constants.Columns - 1 - HeightBalls[posX], posX] = (char)(ball.a + '0');
                ++HeightBalls[posX];
                Grid[Constants.Columns - 1 - HeightBalls[posX + 1], posX + 1] = (char)(ball.b + '0');
                ++HeightBalls[posX + 1];
                if (Grid[Constants.Columns - HeightBalls[posX + 1], posX + 1] != Grid[HeightBalls[posX], posX] || HeightBalls[posX + 1] != HeightBalls[posX])
                    DestroyAndFall(posX + 1, Constants.Columns - HeightBalls[posX + 1]);
                break;
        }
        DestroyAndFall(posX, Constants.Columns-HeightBalls[posX]);
    }
   
    void Fall()
    {
        MyQueue<Coor> needCheck = new MyQueue<Coor>();
        for (int i = 0; i < Constants.Rows; ++i)
        {
            if (HeightBalls[i] == lyingBalls[i]) continue;
            for (int j = Constants.Columns-2-lyingBalls[i]; j>=0; --j)
            {
                if(Grid[j,i]!='.')
                {
                    Grid[Constants.Columns - lyingBalls[i] - 1,i] = Grid[j, i];
                    Grid[j, i] = '.';
                    ++lyingBalls[i];
                    if (Grid[Constants.Columns - lyingBalls[i], i] == '.') for (int g = 0; g < 30; ++g) Console.WriteLine("ERROR IN FUNCTION FALL. FUNCTION IS GOINT TO DELETE '.'");
                    if(Grid[Constants.Columns-lyingBalls[i],i]!='0')
                        needCheck.Add(new Coor(i,Constants.Columns-lyingBalls[i]));
                }
            }
        }
        while (!needCheck.IsEmpty())
        {
            DestroyAndFall(needCheck.Get().x, needCheck.Get().y);
            needCheck.Pop();
        }
    }
    void DestroyAndFall(int x, int y)
    {
        if (y == Constants.Columns || Grid[y, x] == '.' || Grid[y,x]=='0') return;
        //Console.WriteLine("y=" + y + " x=" + x);
        isThisColorDestroyed[Grid[y, x] - '0'] = true;
        char currentColor = Grid[y, x];
        bool[,] used = new bool[Constants.Columns, Constants.Rows];
        MyQueue<Coor> willDestroyed = new MyQueue<Coor>();
        MyQueue<Coor> way = new MyQueue<Coor>();
        way.Add(new Coor(x, y));
        used[way.Get().y, way.Get().x] = true; ;
        int count = 0;
        while (!way.IsEmpty())
        {
            willDestroyed.Add(way.Get());
            ++count;
            if (way.Get().y + 1 < Constants.Columns && !used[way.Get().y + 1, way.Get().x] && Grid[way.Get().y + 1, way.Get().x] == currentColor)
            {
                used[way.Get().y + 1, way.Get().x] = true;
                way.Add(new Coor(way.Get().x, way.Get().y + 1));
            }

            if (way.Get().y - 1 >= 0 && !used[way.Get().y - 1, way.Get().x] && Grid[way.Get().y - 1, way.Get().x] == currentColor)
            {
                used[way.Get().y - 1, way.Get().x] = true;
                way.Add(new Coor(way.Get().x, way.Get().y - 1));
            }

            if (way.Get().x + 1 < Constants.Rows && !used[way.Get().y, way.Get().x + 1] && Grid[way.Get().y, way.Get().x + 1] == currentColor)
            {
                used[way.Get().y, way.Get().x + 1] = true;
                way.Add(new Coor(way.Get().x + 1, way.Get().y));
            }

            if (way.Get().x - 1 >= 0 && !used[way.Get().y, way.Get().x - 1] && Grid[way.Get().y, way.Get().x - 1] == currentColor)
            {
                way.Add(new Coor(way.Get().x - 1, way.Get().y));
                used[way.Get().y, way.Get().x - 1] = true;
            }
            way.Pop();
        }
        //Console.WriteLine("Count="+count);
        if (count < 4) return;
        while (!willDestroyed.IsEmpty())
        {
            Grid[willDestroyed.Get().y, willDestroyed.Get().x] = '.';
            --HeightBalls[willDestroyed.Get().x];
            ++B;
            int newLyingBall = Constants.Columns - willDestroyed.Get().y - 1;
            //   Console.WriteLine("newLyingBall=" + newLyingBall + " x=" + willDestroyed.Get().x + " y=" + willDestroyed.Get().y +" res=" + newLyingBall);
            if (newLyingBall < lyingBalls[willDestroyed.Get().x]) lyingBalls[willDestroyed.Get().x] = newLyingBall;

            willDestroyed.Pop();
        }
        Fall();
    }
    public override void ShowInformation()
    {
        Console.Error.WriteLine("Simulate");
        for (int i = 0; i < Constants.Columns; ++i)
        {
            for (int j = 0; j < Constants.Rows; ++j)
                Console.Error.Write(Grid[i, j] + " ");
            Console.Error.WriteLine();
        }
        Console.Error.Write("HeightBalls:");
        foreach (var el in HeightBalls)
            Console.Error.Write(el + " ");
        Console.Error.WriteLine();
        Console.Error.Write("LyingBalls: ");
        foreach (var t in lyingBalls)
            Console.Error.Write(t + " ");
        Console.Error.WriteLine();
    }
}


class Program
{
    static void Main()
    {
        Player me = new Player();
        Player enemy = new Player(me.NextBalls);
        Simulate analize;
        while (true)
        {
            me.UpdateInput();
            enemy.UpdateInput();
            me.ShowInformation();
            analize = new Simulate(me.GetGridCopy(), me.GetHeightBallsCopy());
            bool done = false;
            for (int i = 0; i < Constants.Rows && !done; ++i)
            {
                analize = new Simulate(me.GetGridCopy(), me.GetHeightBallsCopy());
                analize.SimulateMove(me.NextBalls[0], i, Rotation.Down);
                if (analize.Score() > 4)
                {
                    Console.WriteLine(i + " " + Rotation.Down);
                    done = true;
                }
            }
            if(!done)
            {
                me.StupidMove();
            }
        }
    }
}