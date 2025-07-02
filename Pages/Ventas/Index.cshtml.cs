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
    public class IndexModel : PageModel
    {
        private readonly Papeleria.Data.AppDbContext _context;

        public IndexModel(Papeleria.Data.AppDbContext context)
        {
            _context = context;
        }

        public IList<Transaccion> Transacciones { get;set; } = default!;
        public List<string> Fechas { get; set; } = new();
        public List<decimal> Totales { get; set; } = new();

        public async Task OnGetAsync()
        {
            Transacciones = await _context.Transaccion
                .Where(v => v.TipoTransaccionId == 1)
                .OrderBy(v => v.Fecha)
                .ToListAsync();

            Fechas = Transacciones.Select(v => v.Fecha.ToString("dd/MM")).ToList();
            Totales = Transacciones.Select(v => v.Total).ToList();
        }
    }
}
