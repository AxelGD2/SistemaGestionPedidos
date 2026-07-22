
using System;
using System.Collections.Generic;
using System.Linq;

namespace SistemaGestionPedidos
{
    public class Program
    {
        public static void Main(string[] args)
        {
            bool menuCorriendo = true;
            List<Pedido> pedidos = new List<Pedido>();
            double total = 0;

            while (menuCorriendo)
            {
                Console.WriteLine("\n-------------------------------------------------------------------------------");
                Console.WriteLine("SISTEMA DE GESTIÓN DE PEDIDOS");
                Console.WriteLine("-------------------------------------------------------------------------------");
                Console.WriteLine("1.Agregar pedido");
                Console.WriteLine("2.Mostrar todos los pedidos");
                Console.WriteLine("3.Buscar pedido");
                Console.WriteLine("4.Modificar pedido");
                Console.WriteLine("5.Cambiar estado");
                Console.WriteLine("6.Eliminar pedido");
                Console.WriteLine("7.Filtrar pedidos");
                Console.WriteLine("8.Mostrar estadísticas");
                Console.WriteLine("9.Mostrar ranking de pedidos");
                Console.WriteLine("10.Salir\n");

                try
                {
                    Console.Write("Escoja una opción: ");
                    int opcion = int.Parse(Console.ReadLine());

                    switch (opcion)
                    {
                        case 1:
                            AgregarPedido();
                            break;

                        case 2:
                            MostrarPedidos(pedidos);
                            break;

                        case 3:
                            BuscarPedido(pedidos);
                            break;

                        case 4:
                            Console.WriteLine("\nOpción aún no implementada.");
                            break;

                        case 5:
                            CambiarEstado(ref pedidos);
                            break;

                        case 6:
                            EliminarPedido(ref pedidos, ref total);
                            break;

                        case 7:
                            FiltrarPedidos(pedidos);
                            break;

                        case 8:
                            Console.WriteLine("\nOpción aún no implementada.");
                            break;

                        case 9:
                            Console.WriteLine("\nOpción aún no implementada.");
                            break;

                        case 10:
                            Console.WriteLine("\nGRACIAS VUELVA PRONTO");
                            menuCorriendo = false;
                            break;

                        default:
                            Console.WriteLine("\nOpción no válida");
                            break;
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("\nFormato no válido");
                }
            }
        }

        public static void AgregarPedido()
        {
            Console.WriteLine("\nLa parte de agregar pedidos la realizará tu compañero.");
            Console.WriteLine("Esta opción queda pendiente para la otra parte del proyecto.");
        }

        public static void MostrarPedidos(List<Pedido> pedidos)
        {
            if (pedidos.Count == 0)
            {
                Console.WriteLine("\nNo hay pedidos registrados.");
                return;
            }

            Console.WriteLine("\n-------------------------------------------------------------------------------");
            Console.WriteLine("LISTA DE PEDIDOS");
            Console.WriteLine("-------------------------------------------------------------------------------");

            foreach (var pedido in pedidos)
            {
                MostrarDetallePedido(pedido);
            }
        }

        public static void BuscarPedido(List<Pedido> pedidos)
        {
            if (pedidos.Count == 0)
            {
                Console.WriteLine("\nNo hay pedidos registrados.");
                return;
            }

            Console.Write("\nIngrese el código del pedido a buscar: ");
            string codigo = (Console.ReadLine() ?? string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(codigo))
            {
                Console.WriteLine("\nDebe ingresar un código de pedido.");
                return;
            }

            var pedidoEncontrado = pedidos.FirstOrDefault(pedido =>
                string.Equals(pedido.codigoPedido, codigo, StringComparison.OrdinalIgnoreCase));

            if (pedidoEncontrado != null)
            {
                MostrarDetallePedido(pedidoEncontrado);
            }
            else
            {
                Console.WriteLine("\nPedido no encontrado.");
            }
        }

        public static void EliminarPedido(ref List<Pedido> pedidos, ref double total)
        {
            if (pedidos.Count == 0)
            {
                Console.WriteLine("\nNo hay pedidos registrados.");
                return;
            }

            Console.Write("\nIngrese el código del pedido a eliminar: ");
            string codigo = (Console.ReadLine() ?? string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(codigo))
            {
                Console.WriteLine("\nDebe ingresar un código de pedido.");
                return;
            }

            Pedido? pedidoAEliminar = pedidos.FirstOrDefault(pedido =>
                string.Equals(pedido.codigoPedido, codigo, StringComparison.OrdinalIgnoreCase));

            if (pedidoAEliminar == null)
            {
                Console.WriteLine("\nPedido no encontrado.");
                return;
            }

            total -= CalcularTotalPedido(pedidoAEliminar);
            pedidos.Remove(pedidoAEliminar);
            Console.WriteLine($"\nPedido {codigo} eliminado correctamente.");
        }

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

            string estadoActual = pedido.estadoPedido ?? "";
            var permitidos = ObtenerTransicionesPermitidas(estadoActual);

            if (permitidos.Count == 0)
            {
                Console.WriteLine($"\nNo es posible cambiar el estado desde '{estadoActual}'.");
                return;
            }

            Console.WriteLine($"\nEstado actual: {estadoActual}");
            Console.WriteLine("Estados permitidos para la transición:");
            for (int i = 0; i < permitidos.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {permitidos[i]}");
            }

            Console.Write("\nEscoja el nuevo estado (número): ");
            string entrada = Console.ReadLine() ?? string.Empty;
            if (!int.TryParse(entrada.Trim(), out int seleccion) || seleccion < 1 || seleccion > permitidos.Count)
            {
                Console.WriteLine("\nSelección inválida.");
                return;
            }

            string nuevoEstado = permitidos[seleccion - 1];
            pedido.estadoPedido = nuevoEstado;
            Console.WriteLine($"\nEstado del pedido {pedido.codigoPedido} cambiado a '{nuevoEstado}'.");
        }

        public static List<string> ObtenerTransicionesPermitidas(string estadoActual)
        {
            return estadoActual switch
            {
                "Pendiente" => new List<string> { "En preparación", "Cancelado" },
                "En preparación" => new List<string> { "Enviado", "Cancelado" },
                "Enviado" => new List<string> { "Entregado" },
                "Entregado" => new List<string>(),
                "Cancelado" => new List<string>(),
                _ => new List<string>()
            };
        }

        public static void FiltrarPedidos(List<Pedido> pedidos)
        {
            if (pedidos.Count == 0)
            {
                Console.WriteLine("\nNo hay pedidos registrados.");
                return;
            }

            Console.WriteLine("\nFiltrar pedidos por:");
            Console.WriteLine("1. Estado");
            Console.WriteLine("2. Cliente");
            Console.WriteLine("3. Producto");
            Console.WriteLine("4. Tipo de entrega");

            Console.Write("\nEscoja un criterio: ");
            int opcion = int.Parse((Console.ReadLine() ?? string.Empty).Trim());

            List<Pedido> resultado = new List<Pedido>();

            switch (opcion)
            {
                case 1:
                    Console.Write("Ingrese el estado: ");
                    string estado = (Console.ReadLine() ?? string.Empty).Trim();
                    resultado = pedidos.Where(p => string.Equals(p.estadoPedido, estado, StringComparison.OrdinalIgnoreCase)).ToList();
                    break;

                case 2:
                    Console.Write("Ingrese el nombre del cliente: ");
                    string cliente = (Console.ReadLine() ?? string.Empty).Trim();
                    resultado = pedidos.Where(p => string.Equals(p.nombreCliente, cliente, StringComparison.OrdinalIgnoreCase)).ToList();
                    break;

                case 3:
                    Console.Write("Ingrese el producto: ");
                    string producto = (Console.ReadLine() ?? string.Empty).Trim();
                    resultado = pedidos.Where(p => string.Equals(p.productoSolicitado, producto, StringComparison.OrdinalIgnoreCase)).ToList();
                    break;

                case 4:
                    Console.Write("Ingrese el tipo de entrega: ");
                    string tipoEntrega = (Console.ReadLine() ?? string.Empty).Trim();
                    resultado = pedidos.Where(p => string.Equals(p.tipoEntrega, tipoEntrega, StringComparison.OrdinalIgnoreCase)).ToList();
                    break;

                default:
                    Console.WriteLine("\nCriterio no válido.");
                    return;
            }

            if (resultado.Count == 0)
            {
                Console.WriteLine("\nNo se encontraron pedidos con ese criterio.");
                return;
            }

            Console.WriteLine("\nRESULTADO DEL FILTRO");
            foreach (var pedido in resultado)
            {
                MostrarDetallePedido(pedido);
            }
        }

        public static double CalcularTotalPedido(Pedido pedido)
        {
            double subtotal = pedido.cantidad * pedido.precioUnitario;
            double costoEntrega = pedido.tipoEntrega switch
            {
                "Retiro en tienda" => 0,
                "Entrega estándar" => 2.50,
                "Entrega rápida" => 5.00,
                _ => 0
            };

            return subtotal + costoEntrega;
        }

        public static void MostrarDetallePedido(Pedido pedido)
        {
            double subtotal = pedido.cantidad * pedido.precioUnitario;
            double costoEntrega = pedido.tipoEntrega switch
            {
                "Retiro en tienda" => 0,
                "Entrega estándar" => 2.50,
                "Entrega rápida" => 5.00,
                _ => 0
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
