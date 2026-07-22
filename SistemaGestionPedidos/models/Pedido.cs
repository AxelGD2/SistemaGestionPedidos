using System.IO;

public class Pedido
{
    public string codigoPedido {get; set;} = string.Empty;
    public string nombreCliente {get; set;} = string.Empty;
    public string productoSolicitado {get; set;} = string.Empty;
    public int cantidad {get; set;}
    public double precioUnitario {get; set;}
    public tipoEntrega tipoEntrega {get; set;}
    public DateTime fechaPedido {get; set;} = DateTime.Now;
    public EstadoPedido estadoPedido {get; set;} = EstadoPedido.Pendiente;

}