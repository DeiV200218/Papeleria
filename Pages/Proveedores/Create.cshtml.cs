using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Papeleria.Data;
using Papeleria.Models;

namespace Papeleria.Pages_Proveedores
{
    public class CreateModel : PageModel
    {
        private readonly Papeleria.Data.AppDbContext _context;

        public CreateModel(Papeleria.Data.AppDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Proveedor Proveedor { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Proveedor.Add(Proveedor);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
