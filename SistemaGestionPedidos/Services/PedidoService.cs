using System;
using System.Collections.Generic;
using System.Linq;
using SistemaGestionPedidos;

namespace SistemaGestionPedidos.Services
{
    public static class PedidoService
    {
        public static void RegistrarPedidos(ref List<Pedido> pedidos)
        {
            Pedido pedido = new Pedido();    

            Console.Write("\nIngrese el código del pedido: ");
            string codigo = Console.ReadLine().Trim();

            while (String.IsNullOrEmpty(codigo) || pedidos.Any(pedido => pedido.codigoPedido == codigo))
            {
                Console.WriteLine("\nEl código no puede estar vacío o repetido.");
                Console.Write("\nIngrese el código del pedido: ");
                codigo = Console.ReadLine().Trim();
            }

            pedido.codigoPedido = codigo;

            Console.Write("\nIngrese el nombre del cliente: ");
            string nombreCliente = Console.ReadLine().Trim();

            while (String.IsNullOrEmpty(nombreCliente))
            {
                Console.WriteLine("\nEl nombre no puede estar vacío.");
                Console.Write("\nIngrese el nombre del cliente: ");
                nombreCliente = Console.ReadLine().Trim();
            }

            pedido.nombreCliente = nombreCliente;

            Console.Write("\nIngrese el producto solicitado: ");
            string productoSolicitado = Console.ReadLine().Trim();

            while (String.IsNullOrEmpty(productoSolicitado))
            {
                Console.WriteLine("\nEl producto no puede estar vacío.");
                Console.Write("\nIngrese el producto solicitado: ");
                productoSolicitado = Console.ReadLine().Trim();
            }

            pedido.productoSolicitado = productoSolicitado;

            try
            {
                Console.Write("\nIngrese la cantidad: ");
                int cantidad = int.Parse(Console.ReadLine().Trim());

                while (cantidad <= 0)
                {
                    Console.Write("\nLa cantidad debe ser mayor a cero.");
                    Console.Write("\nIngrese la cantidad: ");
                    cantidad = int.Parse(Console.ReadLine().Trim());
                }

                Console.Write("\nIngrese el precio unitario: ");
                double precioUnitario = double.Parse(Console.ReadLine().Trim());

                while (precioUnitario <= 0)
                {
                    Console.Write("\nEl precio unitario debe ser mayor a cero.");
                    Console.Write("\nIngrese el precio unitario: ");
                    precioUnitario = double.Parse(Console.ReadLine().Trim());
                }

                pedido.cantidad = cantidad;
                pedido.precioUnitario = precioUnitario;

                Console.Write("\nIngrese el tipo de entrega: ");
                Console.WriteLine("\n1.Retiro en tienda");
                Console.WriteLine("2.Entrega estándar");
                Console.WriteLine("3.Entrega rápida");

                Console.WriteLine("\nEscoja un tipo de entrega: ");
                int opcionEntrega = int.Parse(Console.ReadLine().Trim());

                switch (opcionEntrega)
                {
                    case 1:
                        pedido.tipoEntrega = tipoEntrega.RetiroEnTienda;
                        break;

                    case 2:
                        pedido.tipoEntrega = tipoEntrega.EntregaEstandar;
                        break;

                    case 3:
                        pedido.tipoEntrega = tipoEntrega.EntregaRapida;
                        break;

                    default:
                        Console.WriteLine("\nTipo de entrega no válido");
                        break;
                }

            }
            catch (FormatException)
            {
                Console.WriteLine("\nFormato de cantidad no válido");
            }

            pedido.fechaPedido = DateTime.Now;
            pedido.estadoPedido = EstadoPedido.Pendiente;

            pedidos.Add(pedido);
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
                double subtotal = pedido.cantidad * pedido.precioUnitario;

                double costoEntrega = pedido.tipoEntrega switch
                {
                    tipoEntrega.RetiroEnTienda => 0,
                    tipoEntrega.EntregaEstandar => 2.50,
                    tipoEntrega.EntregaRapida => 5.00,
                };

                double total = subtotal + costoEntrega;

                Console.WriteLine($"Código: {pedido.codigoPedido}");
                Console.WriteLine($"Cliente: {pedido.nombreCliente}");
                Console.WriteLine($"Producto: {pedido.productoSolicitado}");
                Console.WriteLine($"Cantidad: {pedido.cantidad}");
                Console.WriteLine($"Precio unitario: ${pedido.precioUnitario:F2}");
                Console.WriteLine($"Tipo de entrega: {pedido.tipoEntrega}");
                Console.WriteLine($"Estado: {pedido.estadoPedido}");
                Console.WriteLine($"Fecha: {pedido.fechaPedido:dd/MM/yyyy}");
                Console.WriteLine($"Subtotal: ${subtotal:F2}");
                Console.WriteLine($"Costo de entrega: ${costoEntrega:F2}");
                Console.WriteLine($"Total: ${total:F2}");
                Console.WriteLine("----------------------------------------");
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
                double subtotal = pedidoEncontrado.cantidad * pedidoEncontrado.precioUnitario;
                double costoEntrega = pedidoEncontrado.tipoEntrega switch
                {
                    tipoEntrega.RetiroEnTienda => 0,
                    tipoEntrega.EntregaEstandar => 2.50,
                    tipoEntrega.EntregaRapida => 5.00,                    
                };

                Console.WriteLine($"\nCódigo: {pedidoEncontrado.codigoPedido}");
                Console.WriteLine($"Cliente: {pedidoEncontrado.nombreCliente}");
                Console.WriteLine($"Producto: {pedidoEncontrado.productoSolicitado}");
                Console.WriteLine($"Cantidad: {pedidoEncontrado.cantidad}");
                Console.WriteLine($"Precio unitario: ${pedidoEncontrado.precioUnitario:F2}");
                Console.WriteLine($"Tipo de entrega: {pedidoEncontrado.tipoEntrega}");
                Console.WriteLine($"Estado: {pedidoEncontrado.estadoPedido}");
                Console.WriteLine($"Fecha: {pedidoEncontrado.fechaPedido:dd/MM/yyyy}");
                Console.WriteLine($"Subtotal: ${subtotal:F2}");
                Console.WriteLine($"Costo de entrega: ${costoEntrega:F2}");
                Console.WriteLine($"Total: ${subtotal + costoEntrega:F2}");
            }
            else
            {
                Console.WriteLine("\nPedido no encontrado.");
            }
        }

        public static void ModificarPedido(ref List<Pedido> pedidos)
        {
            Console.WriteLine("\n-------------------------------------------------------------------------------");
            Console.WriteLine("LISTA DE PEDIDOS");
            Console.WriteLine("-------------------------------------------------------------------------------");

            int indice = 0;

            foreach(var pedido in pedidos)
            {
                indice++;
                Console.WriteLine($"{indice} - {pedido.codigoPedido} - {pedido.nombreCliente} - {pedido.precioUnitario}");
            }

            try
            {
                Console.Write("\nEscoja un pedido para modificar: ");
                int opcionModificar = int.Parse(Console.ReadLine());

                Console.WriteLine("\n1.Código del pedido");
                Console.WriteLine("\n2.Nombre del cliente");
                Console.WriteLine("\n3.Producto solicitado");
                Console.WriteLine("\n4.Cantidad");
                Console.WriteLine("\n5.Precio unitario");
                Console.WriteLine("\n6.Tipo de entrega");
                Console.WriteLine("\n7.Fecha de pedido");

                Console.Write("\nQué propiedad desea modificar: ");
                int opcionPropiedad = int.Parse(Console.ReadLine());

                switch (opcionPropiedad)
                {
                    case 1:
                        Console.Write("\nIngrese el nuevo código del pedido: ");
                        string codigo = Console.ReadLine().Trim();

                        while (String.IsNullOrEmpty(codigo) || pedidos.Any(pedido => pedido.codigoPedido == codigo))
                        {
                            Console.WriteLine("\nEl código no puede estar vacío o repetido.");
                            Console.Write("\nIngrese el nuevo código del pedido: ");
                            codigo = Console.ReadLine().Trim();
                        }

                        pedidos[opcionModificar-1].codigoPedido = codigo;
                        break;

                    case 2:
                        Console.Write("\nIngrese el nuevo nombre del cliente: ");
                        string nombreCliente = Console.ReadLine().Trim();

                        while (String.IsNullOrEmpty(nombreCliente))
                        {
                            Console.WriteLine("\nEl nombre no puede estar vacío.");
                            Console.Write("\nIngrese el nuevo nombre del cliente: ");
                            nombreCliente = Console.ReadLine().Trim();
                        }

                        pedidos[opcionModificar-1].nombreCliente = nombreCliente;
                        break;

                    case 3:
                        Console.Write("\nIngrese el nuevo producto solicitado: ");
                        string productoSolicitado = Console.ReadLine().Trim();

                        while (String.IsNullOrEmpty(productoSolicitado))
                        {
                            Console.WriteLine("\nEl producto no puede estar vacío.");
                            Console.Write("\nIngrese el nuevo producto solicitado: ");
                            productoSolicitado = Console.ReadLine().Trim();
                        }

                        pedidos[opcionModificar-1].productoSolicitado = productoSolicitado;
                        break;

                    case 4:
                        Console.Write("\nIngrese la nueva cantidad: ");
                        int cantidad = int.Parse(Console.ReadLine().Trim());

                        while (cantidad <= 0)
                        {
                            Console.Write("\nLa cantidad debe ser mayor a cero.");
                            Console.Write("\nIngrese la nueva cantidad: ");
                            cantidad = int.Parse(Console.ReadLine().Trim());
                        }

                        pedidos[opcionModificar-1].cantidad = cantidad;
                        break;

                    case 5:
                        Console.Write("\nIngrese el precio unitario: ");
                        double precioUnitario = double.Parse(Console.ReadLine().Trim());

                        while (precioUnitario <= 0)
                        {
                            Console.Write("\nEl precio unitario debe ser mayor a cero.");
                            Console.Write("\nIngrese el precio unitario: ");
                            precioUnitario = double.Parse(Console.ReadLine().Trim());
                        }

                        pedidos[opcionModificar-1].precioUnitario = precioUnitario;
                        break;

                    case 6:
                        Console.Write("\nIngrese el nuevo tipo de entrega: ");
                        Console.WriteLine("\n1.Retiro en tienda");
                        Console.WriteLine("2.Entrega estándar");
                        Console.WriteLine("3.Entrega rápida");

                        Console.WriteLine("\nEscoja un tipo de entrega: ");
                        int opcionEntrega = int.Parse(Console.ReadLine().Trim());

                        switch (opcionEntrega)
                        {
                            case 1:
                                pedidos[opcionModificar-1].tipoEntrega = tipoEntrega.RetiroEnTienda;
                                break;

                            case 2:
                                pedidos[opcionModificar-1].tipoEntrega = tipoEntrega.EntregaEstandar;
                                break;

                            case 3:
                                pedidos[opcionModificar-1].tipoEntrega = tipoEntrega.EntregaRapida;
                                break;

                            default:
                                Console.WriteLine("\nTipo de entrega no válido");
                                break;
                        }
                        break;

                    case 7:
                        pedidos[opcionModificar-1].fechaPedido = DateTime.Now;
                        break;
                    
                    default:
                        Console.WriteLine("\nOpción no válida.");
                        break;
                }

            }
            catch (FormatException)
            {
                Console.WriteLine("\nFormato no válido");
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("\nOpción no válida o pedido inexistente");
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("\nOpción no válida o pedido inexistente");
            }
        }

        public static void MostrarRankingPedidos(List<Pedido> pedidos)
        {
            List<Pedido> pedidosRetirar = new List<Pedido>();

            for(int i = 0; i < pedidos.Count; i++)
            {
                pedidosRetirar.Add(pedidos[i]);
            }

            List<Pedido> pedidosOrdenar = new List<Pedido>();            

            while(pedidosOrdenar.Count != pedidos.Count)
            {
                double costoEntrega = pedidosRetirar[0].tipoEntrega switch
                {
                    tipoEntrega.RetiroEnTienda => 0,
                    tipoEntrega.EntregaEstandar => 2.50,
                    tipoEntrega.EntregaRapida => 5.00,                    
                };

                double Total = (pedidosRetirar[0].cantidad * pedidosRetirar[0].precioUnitario) + costoEntrega;

                int j = 0;

                for(int i = 0; i < pedidosRetirar.Count; i++)
                {
                    double costoEntregaTemporal = pedidosRetirar[i].tipoEntrega switch
                    {
                        tipoEntrega.RetiroEnTienda => 0,
                        tipoEntrega.EntregaEstandar => 2.50,
                        tipoEntrega.EntregaRapida => 5.00,                    
                    };

                    double TotalTemporal = (pedidosRetirar[i].cantidad * pedidosRetirar[i].precioUnitario) + costoEntregaTemporal;

                    if(TotalTemporal > Total)
                    {
                        Total = TotalTemporal;
                        j = i;
                    }                    
                }

                pedidosOrdenar.Add(pedidosRetirar[j]);
                pedidosRetirar.RemoveAt(j);
            }

             Console.WriteLine("\n-------------------------------------------------------------------------------");
            Console.WriteLine("RANKING DE PEDIDOS");
            Console.WriteLine("-------------------------------------------------------------------------------");
            
            foreach (var pedido in pedidosOrdenar)
            {
                double subtotal = pedido.cantidad * pedido.precioUnitario;

                double costoEntregaMostrar = pedido.tipoEntrega switch
                {
                    tipoEntrega.RetiroEnTienda => 0,
                    tipoEntrega.EntregaEstandar => 2.50,
                    tipoEntrega.EntregaRapida => 5.00,
                };

                double total = subtotal + costoEntregaMostrar;

                Console.WriteLine($"{pedido.codigoPedido} - {pedido.nombreCliente} - {total}");
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

            total -= PedidoUtils.CalcularTotalPedido(pedidoAEliminar);
            pedidos.Remove(pedidoAEliminar);
            Console.WriteLine($"\nPedido {codigo} eliminado correctamente.");
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
                    Console.WriteLine("\nEstados disponibles:");
                    foreach (var estadoValor in Enum.GetValues<EstadoPedido>())
                    {
                        Console.WriteLine($"- {estadoValor}");
                    }
                    Console.Write("Ingrese el estado: ");
                    string estadoEntrada = (Console.ReadLine() ?? string.Empty).Trim();

                    if (!Enum.TryParse<EstadoPedido>(estadoEntrada, ignoreCase: true, out var estadoEnum))
                    {
                        Console.WriteLine("\nEstado no válido.");
                        return;
                    }

                    resultado = pedidos.Where(p => p.estadoPedido == estadoEnum).ToList();
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
                    Console.WriteLine("\nTipos de entrega disponibles:");
                    foreach (var entregaValor in Enum.GetValues<tipoEntrega>())
                    {
                        Console.WriteLine($"- {entregaValor}");
                    }
                    Console.Write("Ingrese el tipo de entrega: ");
                    string entregaEntrada = (Console.ReadLine() ?? string.Empty).Trim();

                    if (!Enum.TryParse<tipoEntrega>(entregaEntrada, ignoreCase: true, out var entregaEnum))
                    {
                        Console.WriteLine("\nTipo de entrega no válido.");
                        return;
                    }

                    resultado = pedidos.Where(p => p.tipoEntrega == entregaEnum).ToList();
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
                PedidoUtils.MostrarDetallePedido(pedido);
            }
        }
    }
}
