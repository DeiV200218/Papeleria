using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace Papeleria.Models
{
    public class Proveedor
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre del proveedor es obligatorio.")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El telefono del proveedor es obligatorio.")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Debe tener entre 10 digitos")]
        public string Telefono { get; set; }
        public List<Producto> Productos { get; set; } = new();
    }
}
