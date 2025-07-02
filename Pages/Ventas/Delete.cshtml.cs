using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Papeleria.Data;
using Papeleria.Models;

namespace Papeleria.Pages_Ventas
{
    public class DeleteModel : PageModel
    {
        private readonly Papeleria.Data.AppDbContext _context;

        public DeleteModel(Papeleria.Data.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Transaccion Transaccion { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venta = await _context.Transaccion.FirstOrDefaultAsync(m => m.Id == id);

            if (venta == null)
            {
                return NotFound();
            }
            else
            {
                Transaccion = venta;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null) return NotFound();

            var venta = await _context.Transaccion
                .Include(t => t.Detalles) // Incluye los detalles
                .ThenInclude(d => d.Producto) // Incluye el producto
                .FirstOrDefaultAsync(t => t.Id == id);

            if (venta == null) return NotFound();

            // Devolver el stock de cada producto
            foreach (var detalle in venta.Detalles)
            {
                detalle.Producto.Stock += detalle.Cantidad;
                _context.Producto.Update(detalle.Producto); // Marca el producto como modificado
            }

            _context.Transaccion.Remove(venta);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
