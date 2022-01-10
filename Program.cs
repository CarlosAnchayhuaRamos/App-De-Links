using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using ConsoleApplication2.Models;
using Newtonsoft.Json;


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

                Console.WriteLine($"The SHA256 hash of {source} is: {hash}.");

                Console.WriteLine("Verifying the hash...");

                if (VerifyHash(sha256Hash, source, hash))
                {
                    Console.WriteLine("The hashes are the same.");
                }
                else
                {
                    Console.WriteLine("The hashes are not same.");
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
            Console.WriteLine("bucle logeo");
            foreach(user user in users)
            {
                if(user.Usuario == usuario & Hash(llave) == user.contraseña)
                {
                    dirección = user.contraseña;
                }
                break;
            };
            Console.WriteLine("Termino");

            return dirección;
        }

        

       [STAThread]
       static void Main(string[] args)
       {
           string usuario = "";
           string contraseña = "";
           string logeo = "";
           while( logeo == "")
           {
            Console.WriteLine("Cúal es tu usuario:");
            usuario = Console.ReadLine();
            Console.WriteLine("Cúal es tu contraseña:");
            contraseña = Console.ReadLine();
            Console.WriteLine("inicio logeo");
            logeo = Logeo(usuario, contraseña);
            Console.WriteLine(logeo);

           };
           Console.WriteLine("termino bucle logeo");


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
           string target = Directory.GetCurrentDirectory()+ @"\BaseDeDatos\"+ logeo + ".json";
           Console.WriteLine(File.Exists(target));
           bool flg_nuevo = false;

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
                if(cadena.Substring(0,8) != "mdplinks"){
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Ingrese bien el comando");
                    Console.ForegroundColor = ConsoleColor.White;
                    continue;
                }

                List<Link> Links = new List<Link>();
                List<Etiqueta> etiquetas = new List<Etiqueta>();

                Console.WriteLine("File.Exists(target");
                Console.WriteLine(File.Exists(target));
                if(!File.Exists(target)){
                    flg_nuevo = true;

                }
                else
                {
                    flg_nuevo = false;
                    string archivo = Directory.GetCurrentDirectory()+ @"\BaseDeDatos\"+logeo+".json";
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
                            tags = cadena.Substring(index2 + 7, cadena.Length - index2 - 7);
                            cadena = cadena.Substring(0,index2-1);
                        }
                    if(cadena.Length > 10)
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
                    Console.WriteLine(flg_nuevo);
                    if(!flg_nuevo)
                    {
                        //Verificamos URL
                        Console.WriteLine("Verificamos url");
                        foreach(Link link in Links)
                        {
                            Console.WriteLine(url+link.URL);
                            if(url == link.URL)
                            {
                                flgURL = 1;
                                if(title.Length > 1)
                                {
                                    Links[contador].Title = title;
                                    Console.WriteLine("Agregamos titulo");
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
                                                Console.WriteLine(j.NombEtiqueta + i);
                                                flg_tags = false;
                                            }
                                        }
                                        if(flg_tags)
                                        {
                                            Links[contador].Etiquetas.Add(new Etiqueta() {NombEtiqueta=i});
                                        }
                                        
                                    }
                                    Console.WriteLine("Agregamos etiqueta");
                                }
                            }
                            contador = contador+1;
                        }
                    }
                        if(flgURL == 0)
                        {
                                //Agregamos un nuevo Link
                                Console.WriteLine("Agregamos etiqueta nueva");

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

                    
                    
                    Console.WriteLine("Guardamos los datos");
                    string LinksJson = JsonConvert.SerializeObject(Links.ToArray(), Formatting.Indented);
                    string _path =  Directory.GetCurrentDirectory()+ @"\BaseDeDatos\"+ logeo + ".json";
                    System.IO.File.WriteAllText(_path, LinksJson);

                    var CurrentDirectory = Directory.GetCurrentDirectory();
                    Console.WriteLine(CurrentDirectory);


                }else{
                    //Lectura de Links
                    if(tags.Length > 2)
                    {
                        string flg_etiqueta = "No";
                        string acum = "";

                        foreach(Link link in Links)
                        {
                            foreach(Etiqueta etiqueta in link.Etiquetas)
                            {
                                if(etiqueta.NombEtiqueta == tags)
                                {
                                    flg_etiqueta = "yes";
                                }
                            }
                            if(flg_etiqueta == "yes")
                            {
                                Console.WriteLine(string.Format("{0}",link.Title));
                                Console.WriteLine(string.Format("URL: {0}",link.URL));
                                acum = "";
                                foreach(Etiqueta etiqueta in link.Etiquetas)
                                {
                                    acum = acum + string.Format(etiqueta.NombEtiqueta)+", ";
                                    
                                }
                                acum = acum.Remove(acum.Length - 2);
                                Console.WriteLine("Etiquetas: {0}",acum);
                                Console.WriteLine(string.Format("Fecha y Hora de Creación: {0}",link.rfc));
                            }
                        }
                    }

                }

            } while (true);

       }
       

    }
}