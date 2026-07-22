using System;
using System.Collections.Generic;
using System.Linq;
using SistemaGestionPedidos;

namespace SistemaGestionPedidos.Services
{
    public static class PedidoStateService
    {
        public static void CambiarEstado(ref List<Pedido> pedidos)
        {
            if (pedidos.Count == 0)
            {
                Console.WriteLine("\nNo hay pedidos registrados.");
                return;
            }

            Console.Write("\nIngrese el código del pedido cuyo estado desea cambiar: ");
            string codigo = (Console.ReadLine() ?? string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(codigo))
            {
                Console.WriteLine("\nDebe ingresar un código de pedido.");
                return;
            }

            var pedido = pedidos.FirstOrDefault(p => string.Equals(p.codigoPedido, codigo, StringComparison.OrdinalIgnoreCase));

            if (pedido == null)
            {
                Console.WriteLine("\nPedido no encontrado.");
                return;
            }

            EstadoPedido estadoActual = pedido.estadoPedido;
            var permitidos = ObtenerTransicionesPermitidas(estadoActual);

            if (permitidos.Count == 0)
            {
                Console.WriteLine($"\nNo es posible cambiar el estado desde '{FormatearEstado(estadoActual)}'.");
                return;
            }

            Console.WriteLine($"\nEstado actual: {FormatearEstado(estadoActual)}");
            Console.WriteLine("Estados permitidos para la transición:");
            for (int i = 0; i < permitidos.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {FormatearEstado(permitidos[i])}");
            }

            Console.Write("\nEscoja el nuevo estado (número): ");
            string entrada = Console.ReadLine() ?? string.Empty;
            if (!int.TryParse(entrada.Trim(), out int seleccion) || seleccion < 1 || seleccion > permitidos.Count)
            {
                Console.WriteLine("\nSelección inválida.");
                return;
            }

            EstadoPedido nuevoEstado = permitidos[seleccion - 1];
            pedido.estadoPedido = nuevoEstado;
            Console.WriteLine($"\nEstado del pedido {pedido.codigoPedido} cambiado a '{FormatearEstado(nuevoEstado)}'.");
        }

        public static List<EstadoPedido> ObtenerTransicionesPermitidas(EstadoPedido estadoActual)
        {
            return estadoActual switch
            {
                EstadoPedido.Pendiente => new List<EstadoPedido> { EstadoPedido.EnPreparacion, EstadoPedido.Cancelado },
                EstadoPedido.EnPreparacion => new List<EstadoPedido> { EstadoPedido.Enviado, EstadoPedido.Cancelado },
                EstadoPedido.Enviado => new List<EstadoPedido> { EstadoPedido.Entregado },
                EstadoPedido.Entregado => new List<EstadoPedido>(),
                EstadoPedido.Cancelado => new List<EstadoPedido>(),
                _ => new List<EstadoPedido>()
            };
        }

        public static string FormatearEstado(EstadoPedido estado)
        {
            return estado switch
            {
                EstadoPedido.Pendiente => "Pendiente",
                EstadoPedido.EnPreparacion => "En preparación",
                EstadoPedido.Enviado => "Enviado",
                EstadoPedido.Entregado => "Entregado",
                EstadoPedido.Cancelado => "Cancelado",
                _ => "Desconocido"
            };
        }
    }
}
