using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Papeleria.Data;
using Papeleria.Models;

namespace Papeleria.Pages_Proveedores
{
    public class DeleteModel : PageModel
    {
        private readonly Papeleria.Data.AppDbContext _context;

        public DeleteModel(Papeleria.Data.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Proveedor Proveedor { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proveedor = await _context.Proveedor.FirstOrDefaultAsync(m => m.Id == id);

            if (proveedor == null)
            {
                return NotFound();
            }
            else
            {
                Proveedor = proveedor;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proveedor = await _context.Proveedor.FindAsync(id);
            if (proveedor != null)
            {
                Proveedor = proveedor;
                _context.Proveedor.Remove(Proveedor);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
