using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab1._1
{
    public partial class Form1 : Form
    {
        public IStringParse parse = new StringParse();
        public class Edge
        {
            public int a, b, weight;
            public Edge(int a, int b, int weight)
            {
                this.a = a;
                this.b = b;
                this.weight = weight;
            }
        }
        public int size = 2;
        int weight;
        public int[] points = new int[10];
        public List<Edge> edges = new List<Edge>();
        public string result = "";
        public int resultW = 0;
        public Form1()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            size = comboBox1.SelectedIndex + 2;
            bool[] points = new bool[10];
            edges = new List<Edge>();
            weight = 0;
            textBox4.Text = "";
            result = "";
            textBox1.Text = "";
        }

        private void comboBox1_TextUpdate(object sender, EventArgs e)
        {
            bool[] points = new bool[10];
            edges = new List<Edge>();
            weight = 0;
            textBox4.Text = "";
            result = "";
            if (int.TryParse(comboBox1.Text, out int a))
            {
                int n = int.Parse(comboBox1.Text);
                if (!(n < 2 || n > 10))
                {
                    size = n;
                    return;
                }
            }
            comboBox1.SelectedIndex = 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            add(textBox2.Text);
        }

        public int addCounter = 0;

        public void add(string text)
        {
            var res = parse.Parse(text);
            if (res != -1)
            {
                int a = res / 100;
                int b = res / 10 % 10;
                int w = res % 10;
                if (a > 0 && a <= size && b > 0 && b <= size && w > 0 && a != b)
                {
                    edges.Add(new Edge(a, b, w));
                    addCounter = 0;
                    for (int i = edges.Count - 1; i > 0; i--)
                    {
                        addCounter++;
                        if (edges[i - 1].weight > edges[i].weight)
                        {
                            var buf = edges[i];
                            edges[i] = edges[i - 1];
                            edges[i - 1] = buf;
                        }
                        else
                        {
                            break;
                        }
                    }
                    textBox4.Text += textBox2.Text + "\r\n";
                }
            }
            textBox2.Text = "";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            edges = new List<Edge>();
            textBox4.Text = "";
            /*var numbers = textBox3.Text.Split(' ');
            if (numbers.Length == 3)
            {
                try
                {
                    int a = int.Parse(numbers[0]);
                    int b = int.Parse(numbers[1]);
                    int w = int.Parse(numbers[2]);
                    if (a > 1 && a <= size && b > 1 && b <= size && w > 0 && a != b)
                    {
                        for (int i = 0; i < edges.Count; i++)
                        {
                            if (edges[i].a == a && edges[i].b == b && edges[i].weight == w)
                            {
                                edges.RemoveAt(i);
                                break;
                            }
                        }
                    }
                    var index = textBox4.Text.IndexOf(textBox3.Text);
                    textBox4.Text = textBox4.Text.Remove(index, textBox3.Text.Length + 2);
                }
                catch (Exception)
                {

                }
                finally
                {
                    textBox3.Text = "";
                }
            }*/
        }

        private void button4_Click(object sender, EventArgs e)
        {
            calculate();
        }

        public int count = 0;
        public void calculate()
        {
            count = 0;
            resultW = 0;
            result = "";
            points = new int[size];
            for (int i = 0; count < size - 1 && i < edges.Count; i++)
            {
                count = addEdge(count, i);
            }
            if (count != size - 1)
            {
                textBox1.Text = "Невозможно построить остовное дерево";
            }
            else
            {
                textBox1.Text = result + "\r\n" + resultW;
            }
        }

        public int addEdge(int count, int i)
        {
            addCounter = 0;
            if (points[edges[i].a - 1] == 0 && points[edges[i].b - 1] == 0)
            {
                count++;
                result += edges[i].a + "-" + edges[i].b + "\r\n";
                resultW += edges[i].weight;
                points[edges[i].b - 1] = i + 1;
                points[edges[i].a - 1] = i + 1;
            }
            else
            {
                if (points[edges[i].a - 1] == 0 || points[edges[i].b - 1] == 0)
                {
                    count++;
                    result += edges[i].a + "-" + edges[i].b + "\r\n";
                    resultW += edges[i].weight;
                    if (points[edges[i].a - 1] == 0)
                    {
                        points[edges[i].a - 1] = points[edges[i].b - 1];
                    }
                    else
                    {
                        points[edges[i].b - 1] = points[edges[i].a - 1];
                    }
                }
                else
                {
                    if (points[edges[i].a - 1] != points[edges[i].b - 1])
                    {
                        var buf = points[edges[i].b - 1];
                        for (int j = 0; j < size; j++)
                        {
                            addCounter++;
                            if (points[j] == buf)
                            {
                                points[j] = points[edges[i].a - 1];
                            }
                        }
                        result += edges[i].a + "-" + edges[i].b + "\r\n";
                        resultW += edges[i].weight;
                    }
                }
            }
            return count;
        }
    }
    public interface IStringParse
    {
        int Parse(string numbers);
    }

    public class StringParse : IStringParse
    {
        public StringParse()
        {

        }

        public int Parse(string text)
        {
            var numbers = text.Split(' ');
            if (int.TryParse(numbers[0], out int aa) && int.TryParse(numbers[1], out int bb) && int.TryParse(numbers[2], out int cc))
            {
                return int.Parse(numbers[0]) * 100 + int.Parse(numbers[1]) * 10 + int.Parse(numbers[2]);
            }
            else
            {
                return -1;
            }
        }
    }
}
