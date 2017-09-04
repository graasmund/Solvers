using System;
using System.Diagnostics;
using System.Text;

namespace ConsoleApplication3
{
    class BruteForceSolver
    {
        private long _combinationsTried;
        private DateTime? StartTime;
        private DateTime? EndTime;
        private int _runSeconds;
        private int N;
        private int TotalPlaced;
        private bool IsSolved => TotalPlaced == N;
        private bool[,] Squares;
        private bool[] QueensOnRow;
        private bool[] QueensOnColumn;
        private bool[] QueensOnLeftDiagonal;
        private bool[] QueensOnRightDiagonal;
        private long[] CombinationsOnRow;
        private double[] TimeOnOnRow;

        public BruteForceSolver(int n)
        {
            N = n;
            Squares = new bool[n, n];
            QueensOnRow = new bool[n];
            QueensOnColumn = new bool[n];
            QueensOnLeftDiagonal = new bool[n * 2];
            QueensOnRightDiagonal = new bool[n * 2];
            CombinationsOnRow = new long[n];
            TimeOnOnRow = new double[n];
        }

        public bool Add(int row, int col)
        {
            _combinationsTried++;

            if (!QueensOnRow[row]
                && !QueensOnColumn[col]
                && !QueensOnRightDiagonal[col + row]
                && !QueensOnLeftDiagonal[col - row + N])
            {
                TotalPlaced++;
                Squares[row, col] = true;
                QueensOnRow[row] = true;
                QueensOnColumn[col] = true;
                QueensOnRightDiagonal[col + row] = true;
                QueensOnLeftDiagonal[col - row + N] = true;

                return true;
            }

            return false;
        }

        public void Remove(int row, int col)
        {
            TotalPlaced--;
            Squares[row, col] = false;
            QueensOnRow[row] = false;
            QueensOnColumn[col] = false;
            QueensOnRightDiagonal[col + row] = false;
            QueensOnLeftDiagonal[col - row + N] = false;
        }

        bool InternalSolve(Stopwatch sw, int startRow = 0)
        {
            if (IsSolved)
                return true;
            else
            {
                var seconds = (int)((DateTime.Now - StartTime).Value.TotalSeconds * 10);
                if (seconds != _runSeconds)
                {
                    _runSeconds = seconds;
                    Console.SetCursorPosition(0, 0);
                    Console.WriteLine(ToString());
                }
            }

            for (int row = startRow; row < N; row++)
            {
                if (QueensOnRow[row])
                    continue;

                var elapsed = sw.Elapsed.TotalMilliseconds;

                for (int col = 0; col < N; col++)
                {
                    if (Add(row, col))
                    {
                        var res = InternalSolve(sw, startRow + 1);

                        CombinationsOnRow[row]++;
                        TimeOnOnRow[row] += sw.Elapsed.TotalMilliseconds - elapsed;
                        elapsed = sw.Elapsed.TotalMilliseconds;

                        if (res)
                            return true;
                        else
                            Remove(row, col);
                    }
                }

                if (!QueensOnRow[row])
                    return false;
            }
            return false;
        }


        public bool Solve()
        {
            StartTime = DateTime.Now;
            EndTime = null;
            _runSeconds = 0;

            Console.CursorVisible = false;
            var sw = Stopwatch.StartNew();

            var result = InternalSolve(sw);

            Console.CursorVisible = true;
            EndTime = DateTime.Now;

            return result;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            if (StartTime != null && EndTime == null)
            {
                sb.AppendLine($"Solving for {N} queens, running for {DateTime.Now - StartTime}\n");
                sb.AppendLine($"{_combinationsTried / (DateTime.Now - StartTime).Value.TotalSeconds:### ### ### ###} combinations / sec\n                 ");
            }
            else if (StartTime != null && EndTime != null)
            {
                sb.AppendLine($"Solved for {N} queens, runtime {EndTime - StartTime}");
                if (!IsSolved)
                    sb.AppendLine("\n*** Unsolvable!! ***\n");

                sb.AppendLine();
            }


            int a = 0, b = 0, c = 0;
            for (int row = 0; row < N; row++)
            {
                a = Math.Max(a, $" {TimeOnOnRow[row] / CombinationsOnRow[row]:#.0000} (".Length);
                b = Math.Max(b, $"{TimeOnOnRow[row]:0.00} / ".Length);
                c = Math.Max(c, $"{CombinationsOnRow[row]:0})".Length);
            }

            for (int row = 0; row < N; row++)
            {
                for (int col = 0; col < N; col++)
                {
                    sb.Append(Squares[row, col] ? '*' : '.');
                }

                sb.Append($" {TimeOnOnRow[row] / CombinationsOnRow[row]:#.0000} (".PadLeft(a));
                sb.Append($"{TimeOnOnRow[row]:0.00} / ".PadLeft(b));
                sb.AppendLine($"{CombinationsOnRow[row]:0})".PadLeft(c));
            }
            return sb.ToString();
        }
    }
}
