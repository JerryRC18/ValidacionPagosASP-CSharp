using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSP2.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string? UserName { get; set; }

    public string? Clave { get; set; }
    
    [NotMapped]

    public bool MantenerActivo { get; set; }
}
