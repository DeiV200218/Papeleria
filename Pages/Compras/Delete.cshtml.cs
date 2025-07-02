using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Papeleria.Data;
using Papeleria.Models;

namespace Papeleria.Pages_Compras
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
            if (id == null)
            {
                return NotFound();
            }

            var venta = await _context.Transaccion.FindAsync(id);
            if (venta != null)
            {
                Transaccion = venta;
                _context.Transaccion.Remove(Transaccion);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
