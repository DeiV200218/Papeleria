using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Papeleria.Data;
using Papeleria.Models;

namespace Papeleria.Pages.Productos
{
    public class CreateModel : PageModel
    {
        private readonly AppDbContext _context;

        public CreateModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Producto Producto { get; set; }

        public List<SelectListItem> Proveedores { get; set; }

        public async Task OnGetAsync()
        {
            Proveedores = await _context.Proveedor
                .Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Nombre })
                .ToListAsync();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Remove("Producto.Proveedor");
            if (!ModelState.IsValid)
                return Page();

            _context.Producto.Add(Producto);
            await _context.SaveChangesAsync();

            // Podrías usar TempData para devolver feedback
            TempData["ProductoCreado"] = Producto.Nombre;

            return Page();
        }
    }
}
