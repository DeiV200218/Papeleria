namespace Papeleria.Models
{
    public class DetalleTransaccion
    {
        public int Id { get; set; }

        public int VentaId { get; set; }
        public Transaccion Venta { get; set; }

        public int ProductoId { get; set; }
        public Producto Producto { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal => Cantidad * PrecioUnitario;
    }
}
