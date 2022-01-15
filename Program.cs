using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using ConsoleApplication2.Models;
using Newtonsoft.Json;
using System.Globalization;
using System.Threading.Tasks;
using StackExchange.Redis;


namespace ConsoleApplication2
{
    

    class Class1
    {

        public static string Hash(string llave)
        {
            string source = llave;
            using (SHA256 sha256Hash = SHA256.Create())
            {
                string hash = GetHash(sha256Hash, source);

                if (VerifyHash(sha256Hash, source, hash))
                {
                }
                else
                {
                    Console.WriteLine("Error con el Hash.");
                }

                return hash;
            }
        }

        private static string GetHash(HashAlgorithm hashAlgorithm, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            var sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        // Verify a hash against a string.
        private static bool VerifyHash(HashAlgorithm hashAlgorithm, string input, string hash)
        {
            // Hash the input.
            var hashOfInput = GetHash(hashAlgorithm, input);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            return comparer.Compare(hashOfInput, hash) == 0;
        }

        public static string GetLinksJson(string archivo)
        {
            string Links;
            using(var reader = new StreamReader(archivo))
            {
                Links = reader.ReadToEnd();
            }
            return Links;
        }
        public static List<Link>  DeserializeFile(string Links)
        {
            var Link = JsonConvert.DeserializeObject<List<Link>>(Links);

            return Link;
        }

        public static List<user>  DeserializeLogeo(string user)
        {
            var users = JsonConvert.DeserializeObject<List<user>>(user);

            return users;
        }

        public static string Logeo(string usuario, string llave)
        {
            string dirección = "";
            List<user> users = new List<user>();
            var Lectura = GetLinksJson(Directory.GetCurrentDirectory()+ @"\Usuarios\users.Json");
            users = DeserializeLogeo(Lectura);
            foreach(user user in users)
            {
                if(user.Usuario == usuario & Hash(llave) == user.contraseña)
                {
                    dirección = user.contraseña;
                }
                break;
            };

            return dirección;
        }

        

       [STAThread]
       static async Task Main(string[] args)
       {
           string CarpetaBasedeDatos = "";
           while(!Directory.Exists(CarpetaBasedeDatos)){
                Console.WriteLine("Cúal es el directorio de Base de Datos:");
                CarpetaBasedeDatos = Console.ReadLine();
           }
           string usuario = "";
           string contraseña = "";
           string logeo = "";
           while( logeo == "")
           {
            Console.WriteLine("Cúal es tu usuario:");
            usuario = Console.ReadLine();
            Console.WriteLine("Cúal es tu contraseña:");
            contraseña = Console.ReadLine();
            logeo = Logeo(usuario, contraseña);
            Console.Clear();
           };

        #region InputData
        //    List<user> users = new List<user>();

        //    users.Add(new user() {
        //     UniqueId = Guid.NewGuid ().ToString (),
        //     Usuario = usuario,
        //     contraseña = Hash(contraseña)
        //     });

        //     Console.WriteLine("Guardamos los datos");
        //             string LinksJson = JsonConvert.SerializeObject(users.ToArray(), Formatting.Indented);
        //             string _path =  Directory.GetCurrentDirectory()+ @"\Usuarios\users.json";
        //             System.IO.File.WriteAllText(_path, LinksJson);


        //    if(File.Exists(Directory.GetCurrentDirectory()+ @"\BaseDeDatos\"+ logeo + ".json"))
        //    {
        //        Console.WriteLine(File.Exists("SI"));

        //    }
        //    else
        //    {
        //        Console.WriteLine(File.Exists("NO"));
        //        System.IO.File.WriteAllText(Directory.GetCurrentDirectory()+ @"\BaseDeDatos", contraseña);

        //    }
        #endregion

           string target = CarpetaBasedeDatos+ @"\"+ logeo + ".json";
           bool flg_nuevo = false;
           int paginado = 1;

           do{
               string cadena;
                string title = "",url = "",tags = "";
                //Escribimos una cadena de caracteres.
                Console.WriteLine("Por favor, introduzca el comando:");
                //Capturamos el dato introducido por el usuario
                cadena = Console.ReadLine();

                if(cadena == "q"){
                    break;
                }

                if(cadena.Length < 8)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Ingrese bien el comando");
                    Console.ForegroundColor = ConsoleColor.White;
                    continue;
                }
                if(cadena.Substring(0,9) == "-per-page")
                {
                    try
                    {
                        paginado = Int32.Parse(cadena.Substring(9,cadena.Length - 9));
                        Console.WriteLine(paginado);
                        if(paginado<1)
                        {
                            Console.WriteLine("El paginado debe ser mayor a 1");
                            paginado = 1;
                        }
                        continue;
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Error de Formato");
                    }
                    
                }
                if(cadena.Substring(0,8) != "mdplinks"){
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Ingrese bien el comando");
                    Console.ForegroundColor = ConsoleColor.White;
                    continue;
                }

                List<Link> Links = new List<Link>();
                List<Etiqueta> etiquetas = new List<Etiqueta>();

                if(!File.Exists(target)){
                    flg_nuevo = true;

                }
                else
                {
                    flg_nuevo = false;
                    string archivo = CarpetaBasedeDatos+ @"\"+ logeo + ".json";
                    var Lectura = GetLinksJson(archivo);
                            Links = DeserializeFile(Lectura);
                    
                }

                if(cadena.Length > 11 ){

                    string s5 = cadena;
                    string s6 = "--title";
                    int index3 = s5.IndexOf(s6);
                        if (index3 >= 0){
                            title = cadena.Substring(index3 + 9, cadena.Length - index3 - 10);
                            cadena = cadena.Substring(0,index3-1);
                        }
                    
                    string s3 = cadena;
                    string s4 = "--tags";
                    int index2 = s3.IndexOf(s4);
                        if (index2 >= 0){
                            if(cadena == "mdplinks --tags"  || cadena == "mdplinks --tags ")
                            {
                                tags = "Alltags";
                            }
                            else{
                                tags = cadena.Substring(index2 + 7, cadena.Length - index2 - 7);
                                tags = tags.ToLower();
                                cadena = cadena.Substring(0,index2-1);
                            }
                            
                        }
                    if(cadena.Length > 10 & tags != "Alltags")
                    {
                        string s1 = cadena;
                        url = cadena.Substring(10, cadena.Length - 11);
                    }
                    
                    
                }
                

                int contador = 0;
                List<string> tagslist = new List<string>();
                tagslist = tags.Split(',').ToList();
                int flgURL = 0;

                if(url.Length >= 1){
                    if(!flg_nuevo)
                    {
                        //Verificamos URL
                        foreach(Link link in Links)
                        {
                            if(url == link.URL)
                            {
                                flgURL = 1;
                                if(title.Length > 1)
                                {
                                    Links[contador].Title = title;
                                    //Console.WriteLine("Agregamos titulo");
                                }
                                if(tags.Length > 1)
                                {
                                    bool flg_tags;
                                    foreach(var i in tagslist)
                                    {
                                        flg_tags = true;
                                        foreach(Etiqueta j in link.Etiquetas)
                                        {
                                            if(j.NombEtiqueta == i)
                                            {
                                                //Console.WriteLine(j.NombEtiqueta + i);
                                                flg_tags = false;
                                            }
                                        }
                                        if(flg_tags)
                                        {
                                            Links[contador].Etiquetas.Add(new Etiqueta() {NombEtiqueta=i});
                                        }
                                        
                                    }
                                }
                            }
                            contador = contador+1;
                        }
                    }
                        if(flgURL == 0)
                        {
                            if(tags.Length < 1 & url.Length > 1){
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                                Console.WriteLine("Ingrese mínimo una etiqueta");
                                Console.ForegroundColor = ConsoleColor.White;
                                continue;
                            }
                                //Consulta GET
                                if(title == "")
                                    {
                                        HttpClient client = new HttpClient();
                                        var httpResponse  =  await client.GetAsync(url);
                                        if(httpResponse.IsSuccessStatusCode)
                                        {
                                            var content = await httpResponse.Content.ReadAsStringAsync();
                                            int indexTitle = content.IndexOf("<title>");
                                            int indexTitleFin = content.IndexOf("</title>");
                                                if (indexTitle >= 0){
                                                    title = content.Substring(indexTitle+7,indexTitleFin-indexTitle-7);
                                                }
                                                else
                                                {
                                                    int indexH1 = content.IndexOf("<h1>");
                                                    int indexH1Fin = content.IndexOf("</h1>");
                                                        if (indexH1 >= 0){
                                                            title = content.Substring(indexH1+4,indexH1Fin-indexH1-4);
                                                        }
                                                }
                                        }
                                        
                                    };
                                //Agregamos un nuevo Link
                                foreach(var l in tagslist)
                                {
                                    etiquetas.Add(new Etiqueta() {NombEtiqueta=l});
                                }
                                
                                Links.Add(new Link() {
                                    UniqueId = Guid.NewGuid ().ToString (),
                                    Title = title,
                                    URL = url,
                                    rfc = DateTime.Now,
                                    Etiquetas = etiquetas
                                });
                        }

                        flg_nuevo = false;

                    string LinksJson = JsonConvert.SerializeObject(Links.ToArray(), Formatting.Indented);
                    string _path =  CarpetaBasedeDatos+ @"\"+ logeo + ".json";
                    System.IO.File.WriteAllText(_path, LinksJson);

                    //var CurrentDirectory = Directory.GetCurrentDirectory();
                    
                    Console.WriteLine("Ingreso datos redis");
                    var redisDB = ConsoleApplication2.RedisDB.Connection.GetDatabase();
                    redisDB.StringSet(logeo,LinksJson);

                    Console.WriteLine("Salida datos redis");

                    var valor  = redisDB.StringGet(logeo);
                    Links = DeserializeFile(valor);
                    foreach(Link link in Links)
                    {
                        Console.WriteLine(link.Title);
                    }
                    



                }else{
                    //Lectura de Links
                    if(tags.Length > 2)
                    {
                        string flg_etiqueta = "No";
                        string acum = "";
                        int contPaginado = 0;
                        Console.Clear();
                        foreach(Link link in Links)
                        {
                            foreach(Etiqueta etiqueta in link.Etiquetas)
                            {
                                if(etiqueta.NombEtiqueta == tags || tags == "Alltags")
                                {
                                    flg_etiqueta = "yes";
                                    contPaginado = contPaginado + 1;
                                }
                            }
                            if(flg_etiqueta == "yes") 
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine(string.Format("{0}",link.Title));
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.WriteLine(string.Format("URL: {0}",link.URL));
                                acum = "";
                                foreach(Etiqueta etiqueta in link.Etiquetas)
                                {
                                    acum = acum + string.Format(etiqueta.NombEtiqueta)+", ";
                                    
                                }
                                acum = acum.Remove(acum.Length - 2);
                                Console.WriteLine("Etiquetas: {0}",acum);
                                string diasemana, numdia, nommes, año, hora, min, seg;
                                diasemana = link.rfc.ToString("dddd",new CultureInfo("es-ES"));
                                numdia = link.rfc.ToString("dd",new CultureInfo("es-ES")); 
                                nommes = link.rfc.ToString("MMMM",new CultureInfo("es-ES"));
                                año = link.rfc.ToString("yyyy",new CultureInfo("es-ES"));
                                hora = link.rfc.ToString("hh",new CultureInfo("es-ES"));
                                min = link.rfc.ToString("mm",new CultureInfo("es-ES"));
                                seg = link.rfc.ToString("ss",new CultureInfo("es-ES"));
                                Console.WriteLine(string.Format("Fecha y Hora de Creación: {0} {1} de {2} de {3} {4}:{5}:{6} ",diasemana, numdia, nommes, año, hora, min, seg));
                            }
                            if(contPaginado == paginado){
                                if(Console.ReadKey(true).Key != ConsoleKey.Q)
                                {
                                    contPaginado = 0;
                                    Console.Clear();
                                    continue;
                                }
                                else
                                {
                                    break;
                                };
                            }
                        }
                    }

                }

            } while (true);

       }
       

    }

    public class RedisDB
    {
        private static Lazy<ConnectionMultiplexer> _LazyConnection;

        public static ConnectionMultiplexer Connection
        {
            get
            {
                return _LazyConnection.Value;
            }
        }
        static RedisDB()
        {
            _LazyConnection = new Lazy<ConnectionMultiplexer>(() => 
                ConnectionMultiplexer.Connect("localhost")
            );
        }
    }
    
}