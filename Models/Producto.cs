using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace Papeleria.Models
{
    public class Producto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El precio debe ser mayor a cero.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a cero.")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "Debe ingresar al menos 1 unidad.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe ingresar al menos 1 unidad.")]
        public int Stock { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un proveedor.")]
        public int ProveedorId { get; set; }

        [BindNever]
        public Proveedor Proveedor { get; set; } = null!;
    }
}
