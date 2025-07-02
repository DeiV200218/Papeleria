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
    public class DetailsModel : PageModel
    {
        private readonly Papeleria.Data.AppDbContext _context;

        public DetailsModel(Papeleria.Data.AppDbContext context)
        {
            _context = context;
        }

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
    }
}
