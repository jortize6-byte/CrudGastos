namespace Gimnasio
{
    public class Socio
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = "";
        public decimal Cuota { get; set; }
        public string Membresia { get; set; } = "";

        public override string ToString()
        {
            return $"[{Id}] {Nombre} | Q{Cuota:N2} | {Membresia}";
        }
    }
}