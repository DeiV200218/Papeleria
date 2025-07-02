using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Papeleria.Data;
using Papeleria.Models;

namespace Papeleria.Pages_DetalleVentas
{
    public class EditModel : PageModel
    {
        private readonly Papeleria.Data.AppDbContext _context;

        public EditModel(Papeleria.Data.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public DetalleTransaccion DetalleTransaccion { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detalltransaccion =  await _context.DetalleTransaccion.FirstOrDefaultAsync(m => m.Id == id);
            if (detalltransaccion == null)
            {
                return NotFound();
            }
            DetalleTransaccion = detalltransaccion;
           ViewData["ProductoId"] = new SelectList(_context.Producto, "Id", "Nombre");
           ViewData["VentaId"] = new SelectList(_context.Transaccion, "Id", "Id");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(DetalleTransaccion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DetalleVentaExists(DetalleTransaccion.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool DetalleVentaExists(int id)
        {
            return _context.DetalleTransaccion.Any(e => e.Id == id);
        }
    }
}
