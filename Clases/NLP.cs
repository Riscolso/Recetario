using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace Recetario.Clases
{
    public class NLP
    {
        public Dictionary<string, (string Palabra, string Etiqueta)> Lemas;
        /// <summary>
        /// Dirección en la que se encuentran los archivos de la clase
        /// </summary>
        String PATH = "C:/Users/USUARIO/Desktop/";
        /// <summary>
        /// En caso de no encontrarse una etiqueta en una palabra, se le asigna la Etiqueta Default
        /// </summary>
        String EtiquetaDefault = "n";
        
 
        /// <summary>
        /// Puede escoger entre varias opciones relacionadas con el diccionario de lemas y etiquetas
        /// </summary>
        /// <param name="serializado">En caso de generarse un diccionario, no se guerda; en formato JSON</param>
        /// <param name="regenerar">Forzar el generado del diccionario, si existe es reemplazado</param>
        /// <param name="usarEtiquetas">Si el diccionario de Lemas y etiquetas va a ser usado</param>
        public NLP(bool serializado = true, bool regenerar = false, bool usarEtiquetas = true)
        {
            if (usarEtiquetas)
            {
                if (File.Exists(PATH + "Lemas.json") && !regenerar) CargarLemas();
                else GenerarDicLemmas(serializado);
            }
        }
        /// <summary>
        /// Genera un Diccionario de palabras, cuyo key es la palabra y
        /// su valor es el lemma de está junto con su etiquetado
        /// </summary>
        /// <returns></returns>
        public void GenerarDicLemmas(bool serializar = true)
        {
            String line;
            try
            {
                //Pass the file path and file name to the StreamReader constructor
                StreamReader sr = new StreamReader(PATH+"Generate.txt",Encoding.UTF8);
                //Read the first line of text
                line = sr.ReadLine();
                Lemas = new Dictionary<string, (string, string)>();
                //Continue to read until you reach end of file
                while (line != null)
                {
                    if (line != "")
                    {
                        //Obtener las palabras de cada línea
                        List<string> palabras = new List<string>(line.Trim().Split(" "));
                        int tam = palabras.Count;
                        //La key
                        string clave = palabras[0];
                        //El en Generate, se separa el steam de la palabra
                        clave = clave.Replace("#", "");
                        //Nos quedamos solo con la primera aparición
                        if (!Lemas.ContainsKey(clave))
                        {
                            //Obtener la primera letra de la segunda palabra; que es el POS 
                            string etiqueta = palabras[tam - 2].ToCharArray()[0].ToString().ToLower();
                            Lemas[clave] = (palabras[tam - 1], etiqueta);
                        }
                    }
                    line = sr.ReadLine();
                }
                //close the file
                sr.Close();
                if (serializar)
                {
                    //Serializar el diccionario
                    string json = JsonConvert.SerializeObject(Lemas, Formatting.Indented);
                    File.WriteAllText(PATH+ "Lemas.json", json);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        }
        public void CargarLemas()
        {
            using (StreamReader file = File.OpenText(PATH+"Lemas.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                Lemas = (Dictionary<string, (string, string)>)serializer
                    .Deserialize(file, typeof(Dictionary<string, (string, string)>));
            }
        }
        public List<(string Palabra, string Etiqueta)> LematizarYEtiquetar(List<string> palabras, bool etiquetar = true)
        {
            List<(string Palabra, string Etiqueta)> aux = new List<(string Palabra, string Etiqueta)>();
            Regex rx = new Regex("^[A-Za-záéíóú,]+$");
            //Recorre todo
            foreach (var palabra in palabras)
            {
                //Si es que no son letras
                if (!rx.IsMatch(palabra))
                    aux.Add((palabra, "o")); //O de Otros jajaja
                else if (Lemas.ContainsKey(palabra))
                    aux.Add(Lemas[palabra]);
                else aux.Add((palabra, EtiquetaDefault));
            }
            return aux;
        }
        public static string ObtenerIngrediente(string cadenaIngredientes)
        {
            List<char> aux = new List<char>();
            foreach (var c in cadenaIngredientes)
            {
                if (c == ',')
                {
                    //Agregar espacios cuando se encuentren comas
                    //De está forma es más fácil tokenizar
                    aux.Add(' ');
                    aux.Add(',');
                    aux.Add(' ');
                }
                else aux.Add(c);
            }
            List<string> palabras = new List<string>(new string(aux.ToArray()).Trim().Split(" "));
            //Evitando espacios en blanco y números y ya de paso, minimizando
            Regex rx = new Regex("^[A-Za-záéíóú,]+$");
            for (int i = 0; i < palabras.Count; i++)
            {
                if (palabras[i] != "" && rx.IsMatch(palabras[i]))
                    palabras[i] = palabras[i].Trim().ToLower();
                else palabras.RemoveAt(i);
            }


            /*Parte de código fallido del 2do intento de hacer esto =( 
            //TODO: Quitar los alfanúmericos que están pegados a las palabras (palabra.) 
            //Lematizar y etiquetar todas las palabras
            ///var palabrasEtiquetadas = LematizarYEtiquetar(palabras);
            /*foreach (var a in palabrasetiquetadas)
            {
                console.writeline("palabra: "+ a.palabra+" etiqueta: " + a.etiqueta);
            }
             */
            //Se agrega un último caracter para que el automata finalice
            //Ya que necesita una última iteracción para regresar el resultado
            palabras.Add(",");
            int edo = 1;
            string ingredienteClave = "";
            //Autómata de ingredientes
            foreach (var palabra in palabras)
            {
                switch (edo)
                {
                    case 1:
                        if (!palabra.Equals("de") && !palabra.Equals(","))
                        {
                            edo = 2;
                            ingredienteClave = palabra;
                        }
                        else edo = 6;
                        break;
                    case 2:
                        if (!palabra.Equals("de") && !palabra.Equals(",")) edo = 2;
                        else if (palabra.Equals("de")) edo = 3;
                        else if (palabra.Equals(",")) return ingredienteClave;
                        else edo = 6;
                        break;
                    case 3:
                        if (!palabra.Equals("de") && !palabra.Equals(","))
                        {
                            edo = 4;
                            ingredienteClave = palabra;
                        }
                        else edo = 6;
                        break;
                    case 4:
                        if (!palabra.Equals("de") && !palabra.Equals(",")) edo = 4;
                        else if (palabra.Equals("de")) edo = 3;
                        else if (palabra.Equals(",")) return ingredienteClave;
                        else edo = 6;
                        break;
                    case 5:
                        //En caso de que se encuentre una coma y se siga procesando, darle cuello
                        return ingredienteClave;
                }
            }
            //En caso de que no se pueda reconocer el ingrediente con el autómata, se regresa la última palabra
            return palabras[palabras.Count - 2];
        }
        /// <summary>
        /// Obtiene una palabra y regresa la misma en singular si es el caso
        /// </summary>
        /// <param name="palabra">Palabra en plural</param>
        /// <returns>Palabra en singular, en caso de no lograrse regresa la misma palabra</returns>
        public static string Singular(string palabra)
        {
            //Minimizar palabra
            palabra = palabra.ToLower();
            //Limpiar espacios
            palabra = palabra.Trim();
            //En caso de ser singular de palabra terminada con Z
            if (palabra.EndsWith("ces"))
            {
                palabra = palabra.Remove(palabra.Length - 3);
                palabra += "z";
            }
            if (palabra.EndsWith("es"))
            {
                palabra = palabra.Remove(palabra.Length - 2);
            }
            if (palabra.EndsWith("s"))
            {
                palabra = palabra.Remove(palabra.Length - 1);
            }
            return palabra;
        }
    }
}
