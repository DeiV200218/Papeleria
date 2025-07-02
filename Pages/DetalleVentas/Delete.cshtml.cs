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
    public class DeleteModel : PageModel
    {
        private readonly Papeleria.Data.AppDbContext _context;

        public DeleteModel(Papeleria.Data.AppDbContext context)
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

            var detalletransaccion = await _context.DetalleTransaccion.FirstOrDefaultAsync(m => m.Id == id);

            if (detalletransaccion == null)
            {
                return NotFound();
            }
            else
            {
                DetalleTransaccion = detalletransaccion;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detalleventa = await _context.DetalleTransaccion.FindAsync(id);
            if (detalleventa != null)
            {
                DetalleTransaccion = detalleventa;
                _context.DetalleTransaccion.Remove(DetalleTransaccion);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
