using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace WindowsFormsGraph
{
    /// <summary>
    /// Класс, описывающий вершину графа
    /// </summary>
    public class Vertex
    {


        /// <summary>
        /// Индекс текущей вершины в списке смежности
        /// </summary>
        public int Index { get; set; }


        /// <summary>
        /// Наименование текущей вершины
        /// </summary>
        public string Name { get; }


        /// <summary>
        /// Координаты текущей вершины
        /// </summary>
        public Point point { get; set; }


     

        /// <summary>
        /// Создание вершины
        /// </summary>
        /// <param name="id">Идентификатор вершины</param>
        /// <param name="p">Координаты вершины</param>
        public Vertex(int index, int id, Point p)
        {
            Index = index;
            Name = GetName(ref id, false);
            point = p;
        }


        /// <summary>
        /// Назначение буквенного обозначения вершины по индексу
        /// </summary>
        /// <param name="index">индекс вершины</param>
        /// <returns></returns>
        private static string GetName(ref int num, bool rec)
        {
            string res = "";
            int count = 0;
            while (num >= 26)
            {
                num -= 26;
                count++;
            }
            if (count > 25)
                res += GetName(ref count, true);
            if (count != 0)
                res += (char)('A' + count - 1);
            if (!rec)
                res += (char)('A' + num);
            return res;
        }



    }
}
