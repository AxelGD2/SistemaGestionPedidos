using System.IO;

public class Pedido
{
    public string codigoPedido {get; set;}
    public string nombreCliente {get; set;}
    public string productoSolicitado {get; set;}
    public int cantidad {get; set;}
    public double precioUnitario {get; set;}
    public string tipoEntrega {get; set;}
    public DateTime fechaPedido {get; set;}
    public string estadoPedido {get; set;}

}