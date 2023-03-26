using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Lab_5
{
    class Program
    {
        // глобальные переменные
        const int INF = 1000000;
        static int N;
        static int[,] M;

        enum Color { White, Gray, Black };

        static bool ReadGraph(string FileName) //  функция для считывание данных о графе из файла
        {
            // ограничения на количество вершин
            const int N_MAX = 20;
            const int N_MIN = 1;
            // ограничения на длину ребра
            const int LENGTH_MAX = 100;
            const int LENGTH_MIN = 0;
            try
            {
                StreamReader F = new StreamReader(FileName);
                N = int.Parse(F.ReadLine());


                M = new int[N, N];
                if (N > N_MAX || N < N_MIN) // ограничение для количества вершин 
                {
                    Console.WriteLine("Неверное количество вершин(от 1 до 20)");
                    return false;
                }
                for (int i = 0; i < N; i++)
                {

                    string S = F.ReadLine(); // Как в первой работе проверяем на наличие данных в файле
                    if (S == null)
                    {
                        Console.WriteLine("Пустой ввод!");
                        return false;
                    }

                    string[] Buffer;
                    Buffer = S.Split(' ');
                    if (Buffer.Length != N) // количество ребёр и вершин должно сопадать
                    {
                        Console.WriteLine("Рёбра и вершины не совпадают.");
                        return false;
                    }


                    for (int j = 0; j < N; j++)
                    {

                        M[i, j] = int.Parse(Buffer[j]);

                        if ((M[i, j] > LENGTH_MAX) || (M[i, j] < LENGTH_MIN)) // ограничение для длины ребра
                        {
                            Console.WriteLine("Максимальная длина ребёр 20,а минимальное 0");
                            return false;
                        }
                        if (int.Parse(Buffer[j]) == 0)
                        {
                            M[i, j] = INF;
                        }
                    }
                }
                F.Close();
            }
            catch
            {
                Console.WriteLine("Ошибка чтения файла");

                return false;
            }
            return true;
        }

        static void PrintAdjacencyMatrix() // функция для вывода матрицы смежности.
        {
            Console.WriteLine("МАТРИЦА СМЕЖНОСТИ");
            PrintMatrix(M);
        }

        static void PrintEdgesList() // функция для вывода списков ребёр
        {
            List<int[]> Edges = new List<int[]>();
            for (int i = 0; i < N; ++i)
            {
                for (int j = 0; j < N; ++j)
                    if (M[i, j] < INF) // Если элемент матрицы отличен от "бесконечности", то добавляем в список новый массив с i,j,M[i,j]
                    {
                        int[] buffer = { i, j, M[i, j] };
                        Edges.Add(buffer);
                    }
            }
            Console.WriteLine("Вершины: A-{0}", Convert.ToChar('A' + N - 1));

            if (IsDirectedGraph() == true && IsWeightedGraph() == false)
            {
                foreach (int[] E in Edges)
                    if (E[0] < E[1])
                        Console.WriteLine("{0}-{1}", Convert.ToChar('A' + E[0]), Convert.ToChar('A' + E[1]));
            }
            else if (IsDirectedGraph() == true)
            {
                foreach (int[] E in Edges)
                    if (E[0] < E[1])
                        Console.WriteLine("{0}-{1} ({2})", Convert.ToChar('A' + E[0]), Convert.ToChar('A' + E[1]), E[2]);
            }
            else if (IsWeightedGraph() == false)
            {
                foreach (int[] E in Edges)
                    Console.WriteLine("{0}->{1}", Convert.ToChar('A' + E[0]), Convert.ToChar('A' + E[1]));
            }
            else
            {
                foreach (int[] E in Edges)
                    Console.WriteLine("{0}->{1} ({2})", Convert.ToChar('A' + E[0]), Convert.ToChar('A' + E[1]), E[2]);
            }

        }

        static void PrintAdjacencyLists() // Функция для вывода списков смежности
        {
            List<int[]>[] AdjacencyLists = new List<int[]>[N];
            for (int i = 0; i < N; ++i)
            {
                AdjacencyLists[i] = new List<int[]>();
                for (int j = 0; j < N; ++j)
                    if (M[i, j] < INF) // Если элемент строки отличен от бесконечности,то в список  добавляем новый массив
                        AdjacencyLists[i].Add(new int[] { j, M[i, j] });
            }
            for (int i = 0; i < AdjacencyLists.Length; ++i)
            {
                Console.Write("{0}: ", Convert.ToChar('A' + i));
                foreach (int[] E in AdjacencyLists[i])
                    if (IsWeightedGraph() == false)  // Если граф невзвешенный,то рёбра веса не выводятся :
                        Console.Write("{0} ", Convert.ToChar('A' + E[0]));
                    else
                        Console.Write("{0}({1}) ", Convert.ToChar('A' + E[0]), E[1]);
                Console.WriteLine();
            }

        }

        static void PrintGraphProperties() // Функция для вывода свойств графа
        {
            bool Loops = false;
            for (int i = 0; i < N; ++i)
                if (M[i, i] < INF)
                {
                    if (!Loops)
                    {
                        Console.Write("В графе есть петли: ");
                        Loops = true;
                    }
                    Console.Write("{0}({1}) ", Convert.ToChar('A' + i), M[i, i]);
                }
            if (Loops)
                Console.WriteLine();
            else
                Console.WriteLine("В графе нет петель.");


            if (IsDirectedGraph() == true) //
                Console.WriteLine("Граф неориентированный.");
            else
                Console.WriteLine("Граф ориентированный.");


            if (IsWeightedGraph() == true)
                Console.WriteLine("Граф взвешенный.");
            else
                Console.WriteLine("Граф невзвешенный.");

        }

        static bool IsDirectedGraph() // Функция для определения ориентированности графа
        {
            bool f = true; // Флаг. 
            for (int i = 0; i < M.GetLength(0); i++) // Перебераем  все элементы матрицы смежности.
                for (int j = 0; j < M.GetLength(1); j++)
                {   // Если значение хотя бы одного элемента матрицы не равно значению симметричного ему элемента относительно главной диагонали,то f = false
                    if (M[j, i] != M[i, j])
                        f = false;
                }
            return f; // возращаем флаг со значением true/false
        }

        static bool IsWeightedGraph() // Функция для определения взвешенности графа
        {
            int MinM = 1;
            bool f = false;
            for (int i = 0; i < M.GetLength(0); i++)
                for (int j = 0; j < M.GetLength(1); j++)
                    // Если значение хотя бы одного элемента матрицы больше единицы и не является «бесконечным», то f = true
                    if (M[i, j] > MinM & M[i, j] != INF)
                        f = true;
            return f;
        }

        static void FloydWarshall()
        {
            Console.WriteLine("МАТРИЦА КРАТЧАЙШИХ РАССТОЯНИЙ" + " (АЛГОРИТМ ФЛОЙДА - УОРШЕЛЛА)");
            int[,] R = new int[N, N];
            for (int i = 0; i < N; i++) // Копия матрицы смежности
                for (int j = 0; j < N; j++)
                    R[i, j] = i == j ? 0 : M[i, j];

            for (int k = 0; k < N; k++) // Алгоритм Флойда — Уоршелла
                for (int i = 0; i < N; i++)
                    for (int j = 0; j < N; j++)
                        R[i, j] = Math.Min(R[i, j], R[i, k] + R[k, j]);
            PrintMatrix(R);
        } // Функция алгоритма Флойда — Уоршелла

        static void Dijkstra() // Функция алгоритма Дейкстры
        {
            Console.WriteLine("КРАТЧАЙШЕЕ РАССТОЯНИЕ ОТ ВЕРШИНЫ" + " ДО ОСТАЛЬНЫХ ВЕРШИН (АЛГОРИТМ ДЕЙКСТРЫ)");
            int S = GetVertex();
            if (S == -1)
            {
                Console.WriteLine("ОШИБКА: Неверно указана вершина.");
                return;
            }

            int[] Distance = new int[N];
            bool[] Visited = new bool[N];
            for (int i = 0; i < N; ++i)
            {
                Distance[i] = INF;
                Visited[i] = false;
            }
            Distance[S] = 0;
            int mind;
            do
            {
                mind = INF;
                int MinV = -1;
                for (int i = 0; i < N; i++)
                    if (Distance[i] < mind && !Visited[i])
                    {
                        mind = Distance[i];
                        MinV = i;
                    }
                if (MinV == -1)
                    break;
                for (int i = 0; i < N; i++)
                    if (M[MinV, i] < INF && !Visited[i])
                        Distance[i] = Math.Min(Distance[i], Distance[MinV] + M[MinV, i]);
                Visited[MinV] = true;
            }
            while (mind < INF);
            Console.WriteLine("Кратчайшие расстояния до вершин:");
            PrintByVertices(Distance);

            Console.WriteLine("Кратчайшие пути:");
            for (int i = 0; i < N; ++i)
                if (Distance[i] > 0 && Distance[i] < INF)
                {
                    int T = i;
                    string R = "";
                    while (T != S)
                    {
                        for (int j = 0; j < N; ++j)
                            if (M[j, T] < INF && Distance[j] == Distance[T] - M[j, T])
                            {
                                T = j;
                                R = Convert.ToChar('A' + T) + "-" + R;
                                break;
                            }
                    }
                    Console.Write(R);
                    Console.WriteLine("{0:C}", Convert.ToChar('A' + i));
                }

        }

        static void PrintByVertices(int[] D)
        {
            for (int i = 0; i < N; ++i)
            {
                Console.Write("{0,6}", Convert.ToChar('A' + i));
            }
            Console.WriteLine();

            for (int i = 0; i < N; ++i)
            {
                if (D[i] == INF)
                    Console.Write("{0,6}", "-");
                else
                    Console.Write("{0,6}", D[i]);
            }
            Console.WriteLine();
        }
        static int GetVertex() // Запрос вершин
        {
            try
            {
                int f = -1;
                char V = ' ', MaxLetter = Convert.ToChar('A' + N - 1);
                Console.WriteLine("Введите имя исходной вершины (А-{0:C}): ", MaxLetter);
                V = char.ToUpper(char.Parse(Console.ReadLine()));
                if (V >= 'A' & V <= MaxLetter)
                    return V - 'A';
                else
                    return f;
            }
            catch
            {
                return -1;
            }
        }

        static void PrintMatrix(int[,] T)
        {
            Console.Write(" ");
            for (int i = 0; i < N; ++i)
            {
                Console.Write("{0,6}", Convert.ToChar('A' + i));
            }
            Console.WriteLine();

            for (int i = 0; i < N; i++)
            {
                Console.Write(Convert.ToChar('A' + i));
                for (int j = 0; j < N; j++)
                {
                    if (T[i, j] == INF)
                        Console.Write("{0,6}", "-");
                    else
                        Console.Write("{0,6}", T[i, j]);
                }
                Console.WriteLine();
            }
        } // Новая функция вывода матрицы смежности или кратчайших расстояний


        static void BFS() // Функция алгоритма поиска в ширину
        {
            Console.WriteLine("МИНИМУМ ПЕРЕХОДОВ ОТ ВЕРШИНЫ" + " ДО ОСТАЛЬНЫХ ВЕРШИН (ПОИСК В ШИРИНУ)");
            int S = GetVertex();
            if (S == -1)
            {
                Console.WriteLine("ОШИБКА: Неверно указана вершина.");
                return;
            }
            int[] D = new int[N];
            for (int i = 0; i < N; ++i)
                D[i] = INF;
            Queue<int> Q = new Queue<int>();
            int T = S;
            D[T] = 0;
            Q.Enqueue(T);
            while (Q.Count > 0)
            {
                T = Q.Dequeue();
                for (int i = 0; i < N; ++i)
                    if (M[T, i] < INF && D[i] == INF)
                    {
                        D[i] = D[T] + 1;
                        Q.Enqueue(i);
                    }
            }
            Q.Enqueue(T);
            Console.Write("+" + Convert.ToChar('A' + T) + "(" + Convert.ToString(D[T]) + ") ");

            Console.WriteLine();
            Console.WriteLine("Минимум переходов:");
            PrintByVertices(D);

            Console.WriteLine("Минимальные пути:");
            for (int i = 0; i < N; ++i)
            {
                if (D[i] > 0 && D[i] < INF)
                {
                    T = i;
                    string R = "";
                    while (T != S)
                    {

                        for (int j = 0; j < N; ++j)
                            if (M[j, T] < INF && D[j] == D[T] - 1)
                            {
                                T = j;
                                R = Convert.ToChar('A' + T) + "-" + R;
                                break;
                            }
                    }
                    Console.Write(R);
                    Console.WriteLine("{0:C}", Convert.ToChar('A' + i));
                }
            }

        }


        static void DFS() // Функция алгоритма поиска в глубину
        {
            Console.WriteLine("СВЯЗНОСТЬ ГРАФА И ОПРЕДЕЛЕНИЕ ЦИКЛОВ" + " (ПОИСК В ГЛУБИНУ)");
            bool Directed = IsDirectedGraph();
            if (Directed == false)
                Console.WriteLine("Граф ориентированный." + " Связность не определяется.");
            int[] Components = new int[N];
            Stack<int> GrayPath = new Stack<int>();
            List<int> Cycle = new List<int>();
            Color[] Colors = new Color[N];
            for (int i = 0; i < N; ++i)
            {
                Components[i] = 0;
                Colors[i] = Color.White;
            }

            int ComponentsCount = 0;
            for (int i = 0; i < N; ++i)
                if (Components[i] == 0)
                {
                    ComponentsCount++;
                    GrayPath.Push(i);

                    while (GrayPath.Count > 0)
                    {
                        int V = GrayPath.Peek();
                        if (Colors[V] == Color.White)
                        {
                            Colors[V] = Color.Gray;
                            Console.Write("(" + Convert.ToChar('A' + V) + " ");
                            Components[V] = ComponentsCount;
                        }

                        bool FoundWhite = false;

                        for (int j = 0; j < N; ++j)
                        {
                            if (M[V, j] < INF && Colors[j] == Color.Gray)
                            {
                                int g = GrayPath.Pop();
                                int Prev = GrayPath.Count == 0 ? -1 : GrayPath.Peek();
                                GrayPath.Push(g);
                                if (!Directed || Directed && j != Prev)
                                {
                                    Cycle.Clear();
                                    while (j != GrayPath.Peek())
                                        Cycle.Insert(0, GrayPath.Pop());
                                    foreach (int U in Cycle)
                                        GrayPath.Push(U);
                                    Cycle.Insert(0, j);
                                }
                            }

                            if ((M[V, j] < INF) && (Colors[j] == Color.White))
                            {
                                FoundWhite = true;
                                GrayPath.Push(j);
                                break;
                            }
                        }
                        if (!FoundWhite)
                        {
                            Console.Write(Convert.ToChar('A' + V) + ") ");
                            Colors[V] = Color.Black;
                            GrayPath.Pop();
                        }
                    }
                }
            Console.WriteLine();

            if (Directed == true)
                if (ComponentsCount == 1)
                    Console.WriteLine("Граф связный.");
                else
                {
                    Console.WriteLine("Граф несвязный. Количество компонент: {0:D}", ComponentsCount);
                    Console.WriteLine("Принадлежность к компонентам связности:");
                    for (int i = 1; i <= ComponentsCount; ++i)
                    {
                        Console.Write("{0:D}: ", i);
                        for (int j = 0; j < N; ++j)
                            if (Components[j] == i)
                                Console.Write("{0} ", Convert.ToChar('A' + j));
                        Console.WriteLine();
                    }
                }
            if (Cycle.Count == 0)
                Console.WriteLine("В графе нет циклов.");
            else
            {
                Console.Write("В графе есть цикл: ");
                foreach (int V in Cycle)
                    Console.Write("{0} ", Convert.ToChar('A' + V));
                Console.WriteLine();
            }

        }

        static void TopologicalSort() // Топологическая сортировка
        {
            Console.WriteLine("ТОПОЛОГИЧЕСКАЯ СОРТИРОВКА (АЛГОРИТМ ТАРЬЯНА)");


            int[] Order = new int[N];
            Color[] Colors = new Color[N];
            for (int i = 0; i < N; ++i)
                Colors[i] = Color.White;
            int Key = N;
            bool Success = true;
            for (int i = 0; i < N; ++i)
                if (Success && Colors[i] == Color.White)
                    DFS(i, ref Colors, ref Order, ref Key, ref Success);

            if (Success == false)
            {
                Console.WriteLine("Граф неориентированный или содержит циклы.Топологическая сортировка невозможна.");
                return;
            }

            PrintByVertices(Order);


        }

        static void DFS(int v, ref Color[] Colors, ref int[] Order, ref int Key, ref bool Success)
        {
            Colors[v] = Color.Gray;
            for (int i = 0; i < N; ++i)
                if (v != i && M[v, i] < INF)
                    if (Colors[i] == Color.White)
                        DFS(i, ref Colors, ref Order, ref Key, ref Success);
                    else if (Colors[i] == Color.Gray)
                    {
                        Success = false;
                        break;
                    }
            if (!Success)
                return;
            Colors[v] = Color.Black;
            Order[v] = Key;
            Key--;



        }

        static void MinSpanningTree()
        {
            Console.WriteLine("МИНИМАЛЬНОЕ ОСТОВНОЕ ДЕРЕВО" + " (АЛГОРИТМ КРАСКАЛА)");
            if (IsDirectedGraph() == false)
            {
                Console.WriteLine("Граф ориентированный. Построение остовного дерева невозможно.");
                return;
            }
            List<int[]> Edges = new List<int[]>();


            for (int i = 0; i < N; ++i)
            {
                for (int j = 0; j < N; ++j)
                    if (M[i, j] < INF)
                    {
                        Edges.Add(new int[] { i, j, M[i, j] });
                    }
            }
            Edges = Edges.OrderBy(x => x[2]).ToList();
            int[,] MST = new int[N, N];
            for (int i = 0; i < N; ++i)
                for (int j = 0; j < N; ++j)
                    MST[i, j] = INF;
            int[] Components = new int[N];
            for (int i = 0; i < N; ++i)
                Components[i] = i + 1;
            int E = 0;
            int S = 0;
            foreach (int[] Edge in Edges)
            {
                int C1 = Components[Edge[0]];
                int C2 = Components[Edge[1]];
                if (C1 != C2)
                {
                    int CMin = Math.Min(C1, C2);
                    int CMax = Math.Max(C1, C2);
                    MST[Edge[0], Edge[1]] = Edge[2];
                    MST[Edge[1], Edge[0]] = Edge[2];
                    for (int i = 0; i < N; ++i)
                        if (Components[i] == CMax)
                            Components[i] = CMin;
                    Console.WriteLine("{0}-{1} ({2})", Convert.ToChar('A' + Edge[0]), Convert.ToChar('A' + Edge[1]), Edge[2]);
                    for (int i = 0; i < N; i++)
                    {
                        string ResComp;
                        string s;
                        ResComp = "";

                        for (int j = 0; j < N; j++)
                        {
                            if (ResComp.Length == 0)
                                s = "";
                            else
                                s = " ";
                            if (Components[j] == i)
                                ResComp = ResComp + s + Convert.ToChar('A' + j);
                        }

                        if (ResComp.Length > 0) // если нет элементов,то закрываем.
                            Console.Write(" (" + ResComp + ") ");
                    }

                    E++;
                    S += Edge[2];
                    Console.WriteLine();
                }






                if (E == N - 1)
                    break;
            }

            if (E != N - 1)
            {
                Console.WriteLine("Граф несвязный.Построение остовного дерева невозможно.");
                return;
            }


            Console.WriteLine("Вес минимального остовного дерева:  " + S);
            PrintMatrix(MST);

        }



        static void Main(string[] args)
        {
            const int MinR = 1;
            DirectoryInfo Dir = new DirectoryInfo(Directory.GetCurrentDirectory());
            Console.WriteLine();
            FileInfo[] Files = Dir.GetFiles("*.txt");  // каталог файлов

            if (Files.Length != 0)
            {

                Console.WriteLine("В каталоге приложения обнаружены текстовые файлы:");
                for (int i = 0; i < Files.Length; i++)
                    Console.WriteLine((i + 1) + ": " + Files[i].Name);
                Console.WriteLine("Номер файла с описанием графа (1 .. " + Files.Length + "):");
                int R;
                try
                {


                    R = Convert.ToInt32(Console.ReadLine()); // Выбор файла.
                    if (R > Files.Length || R < MinR)
                    {
                        Console.WriteLine("Число вышло из диапозона ввода");
                        return;
                    }
                }
                catch
                {
                    Console.WriteLine("ОШИБКА: Неверный номер файла.");
                    return;
                }
                if (!(R > Files.Length || R < 1))
                {
                    bool ReadOK = ReadGraph(Files[R - 1].Name);
                    if (!ReadOK)
                        return;
                    int i = 0;

                    do
                    {
                        Console.WriteLine("Операция над графом" + '"' + Files[R - 1].Name + '"' + ':');
                        Console.WriteLine("1. Вывод матрицы смежности.");
                        Console.WriteLine("2. Вывод списка рёбер.");
                        Console.WriteLine("3. Вывод списков смежности.");
                        Console.WriteLine("4. Определение свойств графа");
                        Console.WriteLine("5. Матрица кратчайших расстояний (алгоритм Флойда - Уоршелла).");
                        Console.WriteLine("6.Кратчайшее расстояние от вершины до остальных вершин (алгоритм Дейкстры).");
                        Console.WriteLine("7. Минимум переходов от вершины до остальных вершин (поиск в ширину).");
                        Console.WriteLine("8. Связность графа и определение циклов (поиск в глубину).");
                        Console.WriteLine("9. Топологическая сортировка (алгоритм Тарьяна).");
                        Console.WriteLine("10. Минимальное остовное дерево (алгоритм Краскала).");
                        Console.WriteLine("11. Выход из программы.");
                        try
                        {

                            i = int.Parse(Console.ReadLine());
                            if (i > 11 || i < 1)
                            {
                                Console.WriteLine("Диапозон ввода: 1..11");
                                continue;
                            }
                            switch (i)
                            {
                                case 1:
                                    PrintAdjacencyMatrix();
                                    break;

                                case 2:
                                    PrintEdgesList();
                                    break;
                                case 3:
                                    PrintAdjacencyLists();
                                    break;
                                case 4:
                                    PrintGraphProperties();
                                    break;
                                case 5:
                                    FloydWarshall();
                                    break;
                                case 6:
                                    Dijkstra();
                                    break;
                                case 7:
                                    BFS();
                                    break;
                                case 8:
                                    DFS();
                                    break;
                                case 9:
                                    TopologicalSort();
                                    break;
                                case 10:
                                    MinSpanningTree();
                                    break;
                                case 11:
                                    Console.WriteLine("Выход из программы.");
                                    return;
                            }
                        }
                        catch
                        {
                            Console.WriteLine("Неверный ввод");
                            continue;
                        }

                    }
                    while (i != 11);

                }

            }
            else
            {
                Console.WriteLine("В каталоге приложения не обнаружено текстовых файлов.");
                return;

            }
        }
    }
}

