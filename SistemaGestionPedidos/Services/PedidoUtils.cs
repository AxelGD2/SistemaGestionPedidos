using System;
using SistemaGestionPedidos;

namespace SistemaGestionPedidos.Services
{
    public static class PedidoUtils
    {
        public static double CalcularTotalPedido(Pedido pedido)
        {
            double subtotal = pedido.cantidad * pedido.precioUnitario;
            double costoEntrega = pedido.tipoEntrega switch
            {
                tipoEntrega.RetiroEnTienda => 0,
                tipoEntrega.EntregaEstandar => 2.50,
                tipoEntrega.EntregaRapida => 5.00,
            };

            return subtotal + costoEntrega;
        }

        public static void MostrarDetallePedido(Pedido pedido)
        {
            double subtotal = pedido.cantidad * pedido.precioUnitario;
            double costoEntrega = pedido.tipoEntrega switch
            {
                tipoEntrega.RetiroEnTienda => 0,
                tipoEntrega.EntregaEstandar => 2.50,
                tipoEntrega.EntregaRapida => 5.00,
            };

            Console.WriteLine($"\nCódigo: {pedido.codigoPedido}");
            Console.WriteLine($"Cliente: {pedido.nombreCliente}");
            Console.WriteLine($"Producto: {pedido.productoSolicitado}");
            Console.WriteLine($"Cantidad: {pedido.cantidad}");
            Console.WriteLine($"Precio unitario: ${pedido.precioUnitario:F2}");
            Console.WriteLine($"Tipo de entrega: {pedido.tipoEntrega}");
            Console.WriteLine($"Estado: {pedido.estadoPedido}");
            Console.WriteLine($"Fecha: {pedido.fechaPedido:dd/MM/yyyy}");
            Console.WriteLine($"Subtotal: ${subtotal:F2}");
            Console.WriteLine($"Costo de entrega: ${costoEntrega:F2}");
            Console.WriteLine($"Total: ${subtotal + costoEntrega:F2}");
            Console.WriteLine("----------------------------------------");
        }
    }
}
