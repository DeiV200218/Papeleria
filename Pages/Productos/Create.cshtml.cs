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

namespace Papeleria.Pages_Productos
{
    public class CreateModel : PageModel
    {
        private readonly Papeleria.Data.AppDbContext _context;

        public CreateModel(Papeleria.Data.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Producto Producto { get; set; } = default!;

        public SelectList Proveedores { get; set; } = default!;
        public async Task<IActionResult> OnGetAsync()
        {
            Proveedores = new SelectList(await _context.Proveedor.ToListAsync(), "Id", "Nombre");
            return Page();
        }

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Remove("Producto.Proveedor");
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Producto.Add(Producto);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
