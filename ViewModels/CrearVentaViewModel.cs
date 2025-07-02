using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Papeleria.ViewModels
{
    public class CrearTransaccionViewModel
    {
        public DateTime Fecha { get; set; } = DateTime.Now;

        public List<DetalleVentaInput> Detalles { get; set; } = new List<DetalleVentaInput>();

        public decimal Total => Detalles.Sum(d => d.Cantidad * d.PrecioUnitario);
    }

    public class DetalleVentaInput
    {
        [Required(ErrorMessage = "Debe seleccionar un producto.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un producto.")]
        public int ProductoId { get; set; }

        [Required(ErrorMessage = "La cantidad es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe ser al menos 1")]
        public int Cantidad { get; set; }

        [Required(ErrorMessage = "El campo Precio es obligatorio.")]
        [Range(1, double.MaxValue, ErrorMessage = "No puede ser 0")]
        [DataType(DataType.Currency)]
        public decimal PrecioUnitario { get; set; }
    }
}
