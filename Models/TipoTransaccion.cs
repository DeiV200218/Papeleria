namespace Papeleria.Models
{
    public class TipoTransaccion
    {
        public int Id { get; set; }
        public String Nombre { get; set; }
        public ICollection<Transaccion> Transacciones { get; set; }
    }
}
