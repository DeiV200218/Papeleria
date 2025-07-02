using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;

namespace Papeleria.Models
{
    public class Transaccion
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public decimal Total { get; set; }
        public int TipoTransaccionId { get; set; }
        [BindNever]
        public TipoTransaccion? TipoTransaccion { get; set; }
        public ICollection<DetalleTransaccion> Detalles { get; set; } = new List<DetalleTransaccion>();    
    }
}
