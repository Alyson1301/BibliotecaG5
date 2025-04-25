namespace ApiProyectoFinal.Models
{
    
    public class LibroInsertDTO
    {
        public string Titulo { get; set; }
        public int ID_Autor { get; set; }
        public int? ID_Categoria { get; set; }
        public string ISBN { get; set; }
        public int Año_Publicación { get; set; }
        public string EstadoDisponibilidad { get; set; } = "Disponible";
    }

   
    public class LibroUpdateDTO
    {
        public int ID_Libro { get; set; }
        public string Titulo { get; set; }
        public int ID_Autor { get; set; }
        public int? ID_Categoria { get; set; }
        public string ISBN { get; set; }
        public int Año_Publicación { get; set; }
        public string EstadoDisponibilidad { get; set; }
    }
}