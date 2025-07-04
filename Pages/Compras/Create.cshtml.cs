using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Papeleria.Data;
using Papeleria.Models;
using Papeleria.ViewModels;

namespace Papeleria.Pages.Compras
{
    public class CreateModel : PageModel
    {
        private readonly AppDbContext _context;

        public CreateModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public CrearTransaccionViewModel CompraVM { get; set; } = new();

        public List<Producto> ProductosDisponibles { get; set; } = new();


        public async Task OnGetAsync()
        {
            ProductosDisponibles = await _context.Producto.ToListAsync();

            if (CompraVM.Detalles == null || !CompraVM.Detalles.Any())
            {
                CompraVM.Detalles.Add(new DetalleVentaInput());
            }
        }


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var transaccion = new Transaccion
            {
                Fecha = CompraVM.Fecha,
                Total = CompraVM.Total,
                Detalles = CompraVM.Detalles.Select(d => new DetalleTransaccion
                {
                    ProductoId = d.ProductoId,
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario
                }).ToList(),
                TipoTransaccionId = 2
            };

            foreach (var item in transaccion.Detalles)
            {
                var producto = await _context.Producto.FindAsync(item.ProductoId);

                if (transaccion.TipoTransaccionId == 2)
                {
                    producto.Stock += item.Cantidad;
                }
            }


            _context.Transaccion.Add(transaccion);
            await _context.SaveChangesAsync();

            return RedirectToPage("Index");
        }
    }
}
