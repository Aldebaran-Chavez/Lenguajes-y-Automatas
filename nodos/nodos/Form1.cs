using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace nodos
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //Con el nodo guardo la referencia al heap y entre el stack de los objetos
        //ej visto en clase con la referencia de las listas
        public class Nodo
        {
            public Clase destino;
            public int Valor { get; set; }
            public Clase Direccion { get; set; }

            public Nodo(int valor, Clase direccion)
            {
                Valor = valor;
                Direccion = direccion;
            }
        }

        //Mi clase que llama al nodo, en el nodo se almacena el valor de la referencia
        //Aqui se usan los nodos en la lista con valor y destino
        public class Clase
        {
            public string Nombre { get; set; }
            public List<Nodo> nodoSiguiente { get; set; } = new List<Nodo>();
            public void AgregarConexion(int valor, Clase destino)
            {
                nodoSiguiente.Add(new Nodo(valor, destino));
            }

            // Sobrescribe ToString para imprimir un identificador o nombre
            public override string ToString()
            {
                return Nombre;
            }
        }

        private static void EncontrarCaminoMasCorto(Clase inicio, Clase fin)
        {
            List<Clase> nodosPorExplorar = new List<Clase> { inicio };
            List<int> distancias = new List<int> { 0 };
            List<Clase> padres = new List<Clase> { null };

            Console.WriteLine("^w^");

            //Un tour por todos los nodos
            for (int i = 0; i < nodosPorExplorar.Count; i++)
            {
                //Nodo actual, distancia de un nodo actual al nodo inicio
                var actual = nodosPorExplorar[i];
                int distanciaActual = distancias[i];

                //Recorrido por los nodos vecinos del nodo actual
                for (int j = 0; j < actual.nodoSiguiente.Count; j++)
                {
                    Clase vecino = actual.nodoSiguiente[j].Direccion;

                    //Para los nodos no explorados, como en cuevas
                    if (!nodosPorExplorar.Contains(vecino))
                    {
                        /*
                         *Se pone el nodo vecino a la lista por explorar
                         *Se cuenta la distancia
                         *Se pone de donde viene el nodo con padre
                        */
                        nodosPorExplorar.Add(vecino);
                        distancias.Add(distanciaActual + 1);
                        padres.Add(actual);

                        if (vecino == fin)
                        {
                            //Output del camino
                            List<Clase> camino = new List<Clase>();
                            Clase nodoActual = fin;

                            //Recorre el arbol genealogico de padre a hijo (jojos)
                            while (nodoActual != null)
                            {
                                camino.Add(nodoActual);
                                nodoActual = padres[nodosPorExplorar.IndexOf(nodoActual)];
                            }

                            camino.Reverse();
                            Console.Write("Camino mas corto: ");
                            for (int k = 0; k < camino.Count; k++)
                            {
                                Console.Write(camino[k].ToString() + " ─> ");
                            }
                            Console.WriteLine();
                        }
                    }
                }
            }
        }

            private void button1_Click(object sender, EventArgs e)
        {

            //Representacion; q4 es la meta
            /*
             * q0 (valor0 = 0, direccion = q1), (valor1 = 1, direccion = q3)
             * q1 (valor0 = 0, direccion = q3), (valor1 = 1, direccion = q2), 
                  (valor2 = 2, direccion = q4)
             * q2 (valor0 = 0, direccion = q4)
             * q3 (valor0 = 1, direccion = q2), (valor1 = 2, direccion = q4)
             * q4 (Fin)
             */
            
            //Q0 con sus valores segun la representacion
            Clase q0 = new Clase { Nombre = "q0" };
            Clase q1 = new Clase { Nombre = "q1" };
            Clase q2 = new Clase { Nombre = "q2" };
            Clase q3 = new Clase { Nombre = "q3" };
            Clase q4 = new Clase { Nombre = "q4" };

            //Representaciones con valores y direcciones
            q0.AgregarConexion(0, q1);
            q0.AgregarConexion(1, q3);
            q1.AgregarConexion(0, q3);
            q1.AgregarConexion(1, q2);
            q1.AgregarConexion(2, q4);
            q2.AgregarConexion(0, q4);
            q3.AgregarConexion(1, q2);
            q3.AgregarConexion(2, q4);

            EncontrarCaminoMasCorto(q0, q4);

        }
    }
}
