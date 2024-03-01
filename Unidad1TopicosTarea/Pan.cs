using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unidad1TopicosTarea
{
    public class Pan
    {
            public string Nombre { get; set; } = null!;
            public decimal Precio { get; set; }
            public byte CantidadDisp { get; set; } = 30;
            public string UrlImagen { get; set; } = "";
            public byte CantidadVenta { get; set; } = 1;

    }
}
