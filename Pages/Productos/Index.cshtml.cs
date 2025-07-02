using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Papeleria.Data;
using Papeleria.Models;

namespace Papeleria.Pages_Productos
{
    public class IndexModel : PageModel
    {
        private readonly Papeleria.Data.AppDbContext _context;

        public IndexModel(Papeleria.Data.AppDbContext context)
        {
            _context = context;
        }

        public IList<Producto> Producto { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Producto = await _context.Producto
                .Include(p => p.Proveedor) // Asegúrate de que esta propiedad de navegación exista
                .ToListAsync();
        }
    }
}
