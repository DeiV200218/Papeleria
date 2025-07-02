using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Papeleria.Data;
using Papeleria.Models;

namespace Papeleria.Pages_DetalleVentas
{
    public class IndexModel : PageModel
    {
        private readonly Papeleria.Data.AppDbContext _context;

        public IndexModel(Papeleria.Data.AppDbContext context)
        {
            _context = context;
        }

        public IList<DetalleTransaccion> DetalleTransaccion { get;set; } = default!;

        public async Task OnGetAsync()
        {
            DetalleTransaccion = await _context.DetalleTransaccion
                .Include(d => d.Producto)
                .Include(d => d.Venta)
                .Where(d => d.Venta.TipoTransaccionId == 1).ToListAsync();
        }
    }
}
