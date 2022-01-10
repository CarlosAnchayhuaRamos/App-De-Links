using System;

namespace ConsoleApplication2.Models
{
    public class Link
    {
        public string UniqueId { get; set; }

        public string Title { get; set; }

        public string URL { get; set; }

        public DateTime rfc { get; set; }

        public List<Etiqueta> Etiquetas { get; set; }

    }
}