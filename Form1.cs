using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK.Graphics.OpenGL;
using OpenTK;
using System.Windows.Forms.VisualStyles;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Reflection;
using Microsoft.Win32;
using System.Net;

namespace WindowsFormsApp10
{
    public partial class Form1 : Form
    {

        Random ran;
        Random ran2;
        List<double[]> Vertex; //[x, y номер]
        List<int[]> Edge; //[ номер 1 вершины , номер 2 вершины ,Вес]

        int[,] matrix;
        public Form1()
        {

            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GL.ClearColor(1.0f, 1.0f, 1.0f, 1.0F);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();

            GL.Ortho(-10, 10, -10, 10, 10, -10);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

        }

        private void glControl1_Load(object sender, EventArgs e)
        {

        }
        void Draw()
        {




            GL.Clear(ClearBufferMask.ColorBufferBit);

            GL.PushMatrix();


            grah();

            GL.PopMatrix();

            glControl1.SwapBuffers();
        }



        void CreatTriangle(double a, double b)
        {
            GL.Begin(PrimitiveType.LineStrip);
            GL.Color3(Color.Black);


            GL.Vertex2(a, b - 0.3);

            GL.Vertex2(a + 0.3, b + 0.3);

            GL.Vertex2(a - 0.3, b + 0.3);
            GL.Vertex2(a, b - 0.3);



            GL.End();
        }
        void CreatSquare(double a, double b)
        {
            GL.Begin(PrimitiveType.LineStrip);
            GL.Color3(Color.Black);
            double r = 0.3;
            for (int i = 0; i <= 6; ++i)
            {
                double angle = 2 * Math.PI * i / 6;
                double x = (a + r * Math.Cos(angle));
                double y = (b + r * Math.Sin(angle));
                GL.Vertex2(x, y);
            }





            GL.End();
        }
        void CreatCircle(double a, double b)
        {
            double alpha;
            GL.Begin(PrimitiveType.LineStrip);
            GL.Color3(Color.Black);

            for (int i = 0; i < 360; i++)

            {
                alpha = i * Math.PI / 180.0;
                GL.Vertex2(0.3 * Math.Sin(alpha) + a, 0.3 * Math.Cos(alpha) + b);
            }
            GL.End();




        }
        void CreatCircle2(double a, double b)
        {
            GL.Begin(PrimitiveType.LineStrip);
            GL.Color3(Color.Black);

            for (int i = 0; i < 360; i++)
            {
                GL.Vertex2(0.15 * Math.Sin(i) + a, 0.15 * Math.Cos(i) + b);
            }
            GL.End();
            GL.Begin(PrimitiveType.LineStrip);
            GL.Color3(Color.Black);

            for (double i = 0; i <= 2 * Math.PI + 0.1; i += 0.1)
            {
                GL.Vertex2(4 * Math.Sin(i), 4 * Math.Cos(i));
            }
            GL.End();



        }
        double[,] Points;
        double[] Center;
        double B(float t, int i)
        {
            Center = new double[2];
            Center[0] = 2;
            Center[1] = 2;
            return Math.Pow((1 - t), 2) * Vertex[i][1] + 3 * 2 * Math.Pow((1 - t), 1) * 0 + t * t * Vertex[i + 1][1];
        }
        void bezie()
        {

            double step = 0.01f;// Возьмем шаг 0.01 для большей точности

            double k = 0.5;
            //Vertex[Edge[i][2]][0]=x;Vertex[Edge[i][2]][1]=y; if(Edge[Vertex[i][2]][3]!=0 или matrix[i][j]!=0)
            List<double[]> BezierCurveVertices = new List<double[]>();
            GL.Begin(PrimitiveType.LineStrip);
            int length = matrix.GetLength(0);
            double controlPoint1;
            double controlPoint2;
            for (int i = 0; i < length; i++)
            {
                for (int j =i+1; j < length; j++)
                {
                    if (matrix[i, j] != 0)
                    {
                        // Вычисляем контрольные точки для кривой Безье
                        controlPoint1 = Math.Sqrt((Vertex[i][0] - Vertex[j][0]) * (Vertex[i][0] - Vertex[j][0]) + (Vertex[i][1] - Vertex[j][1]) * (Vertex[i][1] - Vertex[j][1])) / 2;
                        controlPoint2 = (Vertex[i][1] + Vertex[j][1]) ;
                        for (double t = 0; t <= 1; t += 0.01)
                        {
                            double x = (1 - t)* (1 - t) * Vertex[i][0] + 2 * t * (1 - t) * controlPoint1 + Math.Pow(t, 2) * Vertex[j][0];
                            double y = (1 - t) * (1 - t) * Vertex[i][1] + 2 * t * (1 - t) * controlPoint1 + Math.Pow(t, 2) * Vertex[j][1];

                            GL.Vertex2(x, y);
                        }

                    }
                }
            }
            GL.End();
        }
    
                
            
        

        

        
        
       
        void grah()
        {
            int N = Convert.ToInt32(textBox1.Text);
            Vertex = new List<double[]>(); // [ x,y номер]
            int h = 0;
            ran = new Random();
            int count = 0;
            int flag = 1;
           int i = 0;
            while (count !=N*N)
            {

                if (i % Math.Sqrt(N*N)==0)
                {
                    h++;
                    i = 0;
                
                }
                if (count % 2 == 0)
                {
                    CreatSquare(i * 1.1, h * 1.2);
                    Vertex.Add(new double[3] { i * 1.1, 1.2 * h, count + 1 });
                }
                else if (count % 3 == 0)
                {
                    CreatTriangle(i * 1.1, h * 1.2);
                    Vertex.Add(new double[3] { i * 1.1, 1.2 * h, count + 1 });
                }
                else
                {
                    CreatCircle(i * 1.1, h * 1.2);
                    Vertex.Add(new double[3] { i * 1.1, 1.2 * h, count + 1 });
                }
              
                  
              
                i--;
                count++;
                flag = (int)Math.Pow(-1, count);
            }
            CreatEdge(N*N);
        }
        void grah2()
        {
            int N = Convert.ToInt32(textBox1.Text);
            Vertex = new List<double[]>();
          
            ran = new Random();
            for (int i = 0; i < N; i++)
            {


                CreatCircle2(4 * Math.Sin(2 * i), 4 * Math.Cos(2 * i));
                Vertex.Add(new double[3] { 4 * Math.Sin(2 * i), 4 * Math.Cos(2 * i),i});

            }
            bezie();
            CreatEdge2(N);
        }
        void grah1()
        {
            int N = Convert.ToInt32(textBox1.Text);
            Vertex = new List<double[]>();
            int h = 0;
            ran = new Random();
            int count = 0;
            int i = 0;
            int flag = 1;
            while (count != N)
            {

                if (i / Math.Sqrt(N) != 0)
                {
                    h++;
                    i = 0;
                }
                CreatCircle(i * 1.1, h * 1.2);
                if (flag == 1)
                {
                    Vertex.Add(new double[4] { i * 1.1, 1.2 * h, count, 1 }); //// говороим что имеет соседей в ряду
                }
                else
                {
                    Vertex.Add(new double[4] { i * 1.1, 1.2 * h, count, 0 });
                }
                i--;
                count++;
                flag = (int)Math.Pow(-1, count);
            }
            CreatEdge(N);

        }
        void CreatEdge2(int N)
        {

            Edge = new List<int []>();
            float[,] mas = new float[101, 101];
            ran2 = new Random();


            GL.LineWidth(3.0f);
            GL.Begin(PrimitiveType.Lines);
            GL.Color3(Color.Red);
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {


               
                        
                    if (i != j)
                    {
                        Edge.Add(new int[3] { (int)Vertex[i][2], (int)Vertex[j][2], matrix[i, j] });
                    }
                }
            }
            
            GL.End();
            for (int i = 0; i < Edge.Count; ++i)
                groupBox1.Text += Edge[i][0] + "-" + Edge[i][1].ToString() + ' ' + Edge[i][2] + "//";

        }
        void Print(int[] distance, int count)
        {
            Console.WriteLine("Расстояние до вершины от источника");

            for (int i = 0; i < count; ++i)
              groupBox1.Text+=i.ToString()+"-"+distance[i].ToString()+' '+"//";
        }

        List<int> BellmanFord(int A, int B,int N)
        {
            int numVerticces =2*Vertex.Count;
            int numEdges = Edge.Count;

            double[] distance = new double[numVerticces];
            int[] previous = new int[numVerticces];

            for (int i = 0; i < numVerticces; i++)
            {
                distance[i] = double.PositiveInfinity;
                previous[i] = -1;
            }

            distance[A] = 0;

            for (int i = 0; i<numVerticces-1; i++)
            {
                for(int j = 0; j < numEdges; ++j)
                {
                    int u = Edge[j][0];
                    int v = Edge[j][1];
                    int weight = Edge[j][2];

                    if (distance[v] != double.PositiveInfinity && distance[v] + weight < distance[u])
                    {
                        distance[u] = distance[v] + weight;
                        previous[u] = v;
                    }
                }
            }

           
            List<int> path = new List<int>();
            int currentVertex = B;
            while (currentVertex != -1)
            {
                path.Add(currentVertex);
                currentVertex = previous[currentVertex];
            }
            //  Print(distance, numVertices);
            path.Reverse();
            return path;
        }







        //// void solve()
        // {
        //     vector<int> d(n, INF);
        //     d[v] = 0;
        //     vector<int> p(n, -1);
        //     for (; ; )
        //     {
        //         bool any = false;
        //         for (int j = 0; j < m; ++j)
        //             if (d[e[j].a] < INF)
        //                 if (d[e[j].b] > d[e[j].a] + e[j].cost)
        //                 {
        //                     d[e[j].b] = d[e[j].a] + e[j].cost;
        //                     p[e[j].b] = e[j].a;
        //                     any = true;
        //                 }
        //         if (!any) break;
        //     }

        //     if (d[t] == INF)
        //         cout << "No path from " << v << " to " << t << ".";
        //     else
        //     {
        //         vector<int> path;
        //         for (int cur = t; cur != -1; cur = p[cur])
        //             path.push_back(cur);
        //         reverse(path.begin(), path.end());

        //         cout << "Path from " << v << " to " << t << ": ";
        //         for (size_t i = 0; i < path.size(); ++i)
        //             cout << path[i] << ' ';
        //     }
        // }

        void CreatEdge(int N)
        {

            Edge = new List<int[]>();
            float[,] mas = new float[101, 101];
            ran2 = new Random();


            GL.LineWidth(1.0f);
            GL.Begin(PrimitiveType.Lines);



            for (int i = 0; i < N; i++) { 
                GL.Color3(Color.Red);
            GL.Vertex2(Vertex[i][0] + 0.15, Vertex[i][1]);
            GL.Vertex2(Vertex[i][0] + 0.5, Vertex[i][1]);
            GL.Vertex2(Vertex[i][0] - 0.15, Vertex[i][1]);
            GL.Vertex2(Vertex[i][0] - 0.5, Vertex[i][1]);
            GL.Vertex2(Vertex[i][0], Vertex[i][1] - 0.15);
            GL.Vertex2(Vertex[i][0], Vertex[i][1] - 0.5);
            GL.Vertex2(Vertex[i][0], Vertex[i][1] + 0.15);
            GL.Vertex2(Vertex[i][0], Vertex[i][1] + 0.5);


        }





            GL.End();

            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++) // начинаем с i + 1, чтобы избежать дублирования и самосвязывания
                {
                    if (i != j && matrix[i, j] != 0) Edge.Add(new int[3] { (int)Vertex[i][2], (int)Vertex[j][2], matrix[i, j] });
                }
            }
          //  bezie();

            //for (int i = 0; i < Edge.Count; ++i)
            //    groupBox1.Text += Edge[i][0] + "-" + Edge[i][1].ToString() + ' ' + Edge[i][2] + "//";

            int A = Convert.ToInt32(textBox2.Text);
            int B = Convert.ToInt32(textBox3.Text);
            List<int> shortestPath = BellmanFord(A, B, N);
            GL.Begin(PrimitiveType.Lines);
            GL.Color3(Color.Blue);
            for (int i = 0; i < shortestPath.Count-1; i++)
            {

                GL.Vertex2(Vertex[shortestPath[i]-1][0], Vertex[shortestPath[i]-1][1]);
                GL.Vertex2(Vertex[shortestPath[i+1]-1][0], Vertex[shortestPath[i+1]-1][1]);

            }
          
            GL.End();


        }

        private void timer1_Tick(object sender, EventArgs e)
        {








        }
     

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int N = Convert.ToInt32(textBox1.Text);
              
            ran = new Random();

       
            if (N <= 0)
            {
                MessageBox.Show("N<0");
                
            }
            matrix = new int[N*N, N*N];

            for (int i = 0; i < N * N; i++)
            {
                int row = i / N;
                int col = i % N;

                // Соединяем с вершинами слева, справа, сверху, снизу
                if (row > 0)
                {
                    matrix[i, i - N] = ran.Next(1, 10); // вверх
                    matrix[i - N, i] = matrix[i, i - N]; // симметричное значение
                }
                if (row < N - 1)
                {
                    matrix[i, i + N] = ran.Next(1, 10); // вниз
                    matrix[i + N, i] = matrix[i, i + N]; // симметричное значение
                }
                if (col > 0)
                {
                    matrix[i, i - 1] = ran.Next(1, 10); // влево
                    matrix[i - 1, i] = matrix[i, i - 1]; // симметричное значение
                }
                if (col < N - 1)
                {
                    matrix[i, i + 1] = ran.Next(1, 10); // вправо
                    matrix[i + 1, i] = matrix[i, i + 1]; // симметричное значение
                }
            }
            dataGridView2.RowCount = N * N ;
            dataGridView2.ColumnCount = N * N;
            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                for (int j = 0; j < dataGridView2.Columns.Count; j++)
                {
                   
                    dataGridView2[i , j ].Value = matrix[i, j];
                   
                }
            }
           
            Draw();


        }

        private void glControl1_MouseClick(object sender, MouseEventArgs e)
        {
         
            


           
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void A_Click(object sender, EventArgs e)
        {

        }


        private void button2_Click(object sender, EventArgs e)
        {
            groupBox1.Text = " ";
            int N = Convert.ToInt32(textBox1.Text);
           
            int A = Convert.ToInt32(textBox2.Text);
            int B = Convert.ToInt32(textBox3.Text);
            int sum = 0;
            List<int> shortestPath = BellmanFord(A, B,N);
            for (int i = 0; i < shortestPath.Count-1; i++)
            {
                groupBox1.Text += (shortestPath[i]).ToString() + "->";
                sum += Edge[shortestPath[i]+1][2];

            }
            groupBox1.Text += B+" cost="+sum.ToString();
            Draw();
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
