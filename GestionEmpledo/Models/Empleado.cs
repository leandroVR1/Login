namespace GestionEmpledo.Models
{
    public class Empleado
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public string Contraseña { get; set; }
    }

    public class RegistrosEntrada_Salida
    {
        public int Id { get; set; }
        public DateTime FechaEntrada { get; set; }
        public DateTime FechaSalida { get; set; }
        public int IdEmpleado { get; set; }

        public Empleado? Empleado { get; set; }
    }
}
