using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace nodos
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public class Conexion
        {
            //Peso
            public int Valor { get; set; }
            //Nodo al que se apunta
            public Clase NodoDestino { get; set; }

            public Conexion(int valor, Clase nodoDestino)
            {
                //Inicializamos
                Valor = valor;
                NodoDestino = nodoDestino;
            }
        }

        //Mi clase que llama al nodo, en el nodo se almacena el valor de la referencia
        //Aqui se usan los nodos en la lista con valor y destino
        public class Clase
        {
            public string Nombre { get; set; }
            public List<Conexion> NodoSiguiente { get; set; } = new List<Conexion>();
            
            //Devolver el nombre q1... qn
            public override string ToString()
            {
                return Nombre;
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
            Clase q0 = new Clase() { Nombre = "q0" };
            Clase q1 = new Clase() { Nombre = "q1" };
            Clase q2 = new Clase() { Nombre = "q2" };
            Clase q3 = new Clase() { Nombre = "q3" };
            Clase q4 = new Clase() { Nombre = "q4" };

            //Conexiones y Valores
            q0.NodoSiguiente.Add(new Conexion(0, q1));
            q0.NodoSiguiente.Add(new Conexion(1, q3));

            q1.NodoSiguiente.Add(new Conexion(0, q3));
            q1.NodoSiguiente.Add(new Conexion(1, q2));
            q1.NodoSiguiente.Add(new Conexion(2, q4));

            q2.NodoSiguiente.Add(new Conexion(0, q4));

            q3.NodoSiguiente.Add(new Conexion(1, q2));
            q3.NodoSiguiente.Add(new Conexion(2, q4));

            List<Clase> caminoCompleto = EncontrarCaminoMasCorto(q0, q4);

            Console.WriteLine(caminoCompleto != null ? 
                $"El camino más corto es: {string.Join(" -> ", caminoCompleto)}" :
                "No se encontró un camino.");
        }

        private List<Clase> EncontrarCaminoMasCorto(Clase inicio, Clase fin)
        {
            List<Clase> nodos = new List<Clase> { inicio };
            Dictionary<Clase, int> distancias = new Dictionary<Clase, int>();
            Dictionary<Clase, Clase> nodosAnteriores = new Dictionary<Clase, Clase>();
            HashSet<Clase> nodosObtenidos = ObtenerTodosLosNodos(inicio);

            //Inicialización de distancias y nodos anteriores
            foreach (Clase nodo in nodosObtenidos)
            {
                distancias[nodo] = int.MaxValue;
                nodosAnteriores[nodo] = null;
            }
            
            while (nodos.Count > 0)
            {
                Clase actual = nodos.OrderBy(n => distancias[n]).FirstOrDefault();
                if (actual == null) break;
                nodos.Remove(actual);

                if (actual == fin) return ReconstruirCamino(nodosAnteriores, fin);

                foreach (Conexion conexion in actual.NodoSiguiente)
                {
                    Clase vecino = conexion.NodoDestino;
                    int nuevaDistancia = distancias[actual] + conexion.Valor;

                    if (nuevaDistancia < distancias[vecino])
                    {
                        distancias[vecino] = nuevaDistancia;
                        nodosAnteriores[vecino] = actual;
                        if (!nodos.Contains(vecino)) nodos.Add(vecino);
                    }
                }
            }
            return null;
        }

        private List<Clase> ReconstruirCamino(Dictionary<Clase, Clase> nodosAnteriores, Clase fin)
        {
            List<Clase> camino = new List<Clase>();
            Clase actual = fin;

            while (actual != null)
            {
                camino.Add(actual);
                actual = nodosAnteriores[actual];
            }
            camino.Reverse();
            return camino;
        }

        private HashSet<Clase> ObtenerTodosLosNodos(Clase inicio)
        {
            HashSet<Clase> nodos = new HashSet<Clase> { inicio };
            Queue<Clase> cola = new Queue<Clase>();
            cola.Enqueue(inicio);

            while (cola.Count > 0)
            {
                Clase actual = cola.Dequeue();
                foreach (Conexion conexion in actual.NodoSiguiente)
                {
                    Clase vecino = conexion.NodoDestino;
                    if (nodos.Add(vecino)) cola.Enqueue(vecino);
                }
            }
            return nodos;
        }
    }
}
