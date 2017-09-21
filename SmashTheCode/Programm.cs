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
    protected char[,] Grid;
    protected int[] HeightBalls;

    protected BaseInformation() { }
    public char[,] GetGridCopy()
    {
        char[,] retGrid = new char[Constants.Columns, Constants.Rows];
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


class Player : BaseInformation
{
    private bool isKnownNextBalls;
    public Color[] NextBalls { get; private set; }
    public int Score { get; private set; }

    public Player(Color[] NextBalls=null){
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

        for (int i = 0; i < Constants.Columns; ++i)
        {
            for (int j = 0; j < Constants.Rows; ++j)
                Console.Error.Write(Grid[i, j] + " ");
            Console.Error.WriteLine();
        }
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
    public bool CanMove(int posX, Rotation rot)
    {
        if ((rot == Rotation.Down || rot == Rotation.Up) && HeightBalls[posX] >= Constants.Columns - 1
             || rot == Rotation.Left && (posX == 0 || HeightBalls[posX] == Constants.Columns || HeightBalls[posX - 1] == Constants.Columns)
             || rot == Rotation.Right && (posX == Constants.Rows - 1 || HeightBalls[posX] == Constants.Columns || HeightBalls[posX + 1] == Constants.Columns))
            return false;
        else return true;
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
                if (Constants.Columns - 3 - HeightBalls[posX] < 0) return;//Не проиграю ли
                Grid[Constants.Columns - 1 - HeightBalls[posX], posX] = (char)(ball.b + '0');
                ++HeightBalls[posX];
                Grid[Constants.Columns - 1 - HeightBalls[posX], posX] = (char)(ball.a + '0');
                ++HeightBalls[posX];
                if (Grid[Constants.Columns - HeightBalls[posX] - 1, posX] != Grid[HeightBalls[posX], posX])
                    DestroyAndFall(posX, Constants.Columns - HeightBalls[posX] - 1);
                break;
            case Rotation.Up:
                if (Constants.Columns - 3 - HeightBalls[posX] < 0) return;//Не проиграю ли
                Grid[Constants.Columns - 1 - HeightBalls[posX], posX] = (char)(ball.a + '0');
                ++HeightBalls[posX];
                Grid[Constants.Columns - 1 - HeightBalls[posX], posX] = (char)(ball.b + '0');
                ++HeightBalls[posX];
                if (Grid[Constants.Columns - HeightBalls[posX] + 1, posX] != Grid[HeightBalls[posX], posX])
                    DestroyAndFall(posX, Constants.Columns - HeightBalls[posX]);
                break;
            case Rotation.Left:
                if (Constants.Columns - 2 - HeightBalls[posX] < 0 || Constants.Columns - 2 - HeightBalls[posX - 1] < 0) return;//Не проиграю ли
                Grid[Constants.Columns - 1 - HeightBalls[posX], posX] = (char)(ball.a + '0');
                ++HeightBalls[posX];
                Grid[Constants.Columns - 1 - HeightBalls[posX - 1], posX - 1] = (char)(ball.b + '0');
                ++HeightBalls[posX - 1];
                if (Grid[Constants.Columns - HeightBalls[posX - 1], posX - 1] != Grid[Constants.Columns - HeightBalls[posX], posX] || HeightBalls[posX - 1] != HeightBalls[posX])
                    DestroyAndFall(posX - 1, Constants.Columns - HeightBalls[posX - 1]);
                break;
            case Rotation.Right:
                if (Constants.Columns - 2 - HeightBalls[posX] < 0 || Constants.Columns - 2 - HeightBalls[posX + 1] < 0) return;//Не проиграю ли
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
            if (willDestroyed.Get().y - 1 >= 0 && Grid[willDestroyed.Get().y - 1, willDestroyed.Get().x] == '0')//Уничтожение черепов
            {
                Grid[willDestroyed.Get().y - 1, willDestroyed.Get().x] = '.';
                --HeightBalls[willDestroyed.Get().x];
            }
            if (willDestroyed.Get().y + 1 < Constants.Columns && Grid[willDestroyed.Get().y + 1, willDestroyed.Get().x] == '0')
            {
                Grid[willDestroyed.Get().y + 1, willDestroyed.Get().x] = '.';
                --HeightBalls[willDestroyed.Get().x];
            }
            if (willDestroyed.Get().x - 1 >= 0 && Grid[willDestroyed.Get().y, willDestroyed.Get().x - 1] == '0')
            {
                Grid[willDestroyed.Get().y, willDestroyed.Get().x - 1] = '.';
                --HeightBalls[willDestroyed.Get().x - 1];
            }
            if (willDestroyed.Get().x + 1 < Constants.Rows && Grid[willDestroyed.Get().y, willDestroyed.Get().x + 1] == '0')
            {
                Grid[willDestroyed.Get().y, willDestroyed.Get().x + 1] = '.';
                --HeightBalls[willDestroyed.Get().x+1];
            }
            if (Constants.Columns - willDestroyed.Get().y - 1 < lyingBalls[willDestroyed.Get().x])
                lyingBalls[willDestroyed.Get().x] = Constants.Columns - willDestroyed.Get().y - 1;

            willDestroyed.Pop();
        }
        Fall();
    }
    public void ShowInformation()
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

class Calculation : BaseInformation
{
    int deep;
    Random rand;
    //int score;
    int c = 0;
    int[] bestMove = new int[2];
    readonly Player player;
    //Simulate StartPos;
    //Simulate StartPos;
    public Calculation(Player player) : this(player,3) { }
    public Calculation(Player player,int deep)
    {
     //   StartPos = new Simulate(player.GetGridCopy(), player.GetHeightBallsCopy());

        this.player = player;
        rand = new Random();
        if (deep > 0 && deep < 9)
            this.deep = deep;
        else
        {
            this.deep = 2;
            Console.Error.WriteLine("DEEP ERROR");
        }
    }
    /*
    public void StupidMove()
    {
        StartPos = new Simulate(player.GetGridCopy(), player.GetHeightBallsCopy());
        do
        {
            bestMove[0] = rand.Next(0, 6);
            bestMove[1] = rand.Next(0, 4);
        } while (! StartPos.CanMove(bestMove[0], (Rotation)bestMove[1]));
        for (int i = 0; i < 6; ++i)
            Console.Error.Write(player.GetHeightBallsCopy()[i]+ " " );
        Console.Error.WriteLine();
    }*/
    public int FindBestMove(Simulate currentPosition,  int score=0,int currentDeep=0)
    {
        c += 1;
        if (currentDeep == deep) return score;
        int biggestScore = score;
        for (int rot = 0; rot < 4; ++rot)
            for (int i = 0; i < Constants.Rows; ++i)
            {
                if (!currentPosition.CanMove(i, (Rotation)rot)) continue;
                Simulate newPosition = new Simulate(currentPosition.GetGridCopy(), currentPosition.GetHeightBallsCopy());
                newPosition.SimulateMove(player.NextBalls[currentDeep], i, (Rotation)rot);
                int tempScore=FindBestMove(newPosition, newPosition.Score() + score, currentDeep + 1);
                if (currentDeep == 0 && tempScore > biggestScore)
                {
                    biggestScore = tempScore;
                    //this.score = biggestScore;
                    bestMove[0] = i;
                    bestMove[1] = rot;
                }
            }
        
        return biggestScore;
    }
    public int[] GetNextMove()
    {
        //if (score == 0 && bestMove[0] == 0 && bestMove[1] == 0) StupidMove();
        Console.Error.WriteLine(c);
        c = 0;
        return bestMove;
    }
}

class Program
{
    static void Main()
    {
        Player me = new Player();
        Player enemy = new Player(me.NextBalls);
        Calculation AI = new Calculation(me);
        while (true)
        {
            me.UpdateInput();
            enemy.UpdateInput();
        //    me.ShowInformation();
            AI.FindBestMove(new Simulate(me.GetGridCopy(), me.GetHeightBallsCopy()));
            Console.WriteLine(AI.GetNextMove()[0] + " " + AI.GetNextMove()[1]);
        }
    }
}