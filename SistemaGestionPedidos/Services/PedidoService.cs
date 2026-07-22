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
            string codigo = (Console.ReadLine() ?? string.Empty).Trim();

            while (string.IsNullOrEmpty(codigo) || pedidos.Any(p => string.Equals(p.codigoPedido, codigo, StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine("\nEl código no puede estar vacío o repetido.");
                Console.Write("\nIngrese el código del pedido: ");
                codigo = (Console.ReadLine() ?? string.Empty).Trim();
            }

            pedido.codigoPedido = codigo;

            Console.Write("\nIngrese el nombre del cliente: ");
            string nombreCliente = (Console.ReadLine() ?? string.Empty).Trim();

            while (string.IsNullOrEmpty(nombreCliente))
            {
                Console.WriteLine("\nEl nombre no puede estar vacío.");
                Console.Write("\nIngrese el nombre del cliente: ");
                nombreCliente = (Console.ReadLine() ?? string.Empty).Trim();
            }

            pedido.nombreCliente = nombreCliente;

            Console.Write("\nIngrese el producto solicitado: ");
            string productoSolicitado = (Console.ReadLine() ?? string.Empty).Trim();

            while (string.IsNullOrEmpty(productoSolicitado))
            {
                Console.WriteLine("\nEl producto no puede estar vacío.");
                Console.Write("\nIngrese el producto solicitado: ");
                productoSolicitado = (Console.ReadLine() ?? string.Empty).Trim();
            }

            pedido.productoSolicitado = productoSolicitado;

            int cantidad;
            while (true)
            {
                Console.Write("\nIngrese la cantidad: ");
                string entradaCantidad = (Console.ReadLine() ?? string.Empty).Trim();
                if (!int.TryParse(entradaCantidad, out cantidad) || cantidad <= 0)
                {
                    Console.WriteLine("\nLa cantidad debe ser un número entero mayor a cero.");
                    continue;
                }
                break;
            }

            double precioUnitario;
            while (true)
            {
                Console.Write("\nIngrese el precio unitario: ");
                string entradaPrecio = (Console.ReadLine() ?? string.Empty).Trim();
                if (!double.TryParse(entradaPrecio, out precioUnitario) || precioUnitario <= 0)
                {
                    Console.WriteLine("\nEl precio unitario debe ser un número mayor a cero.");
                    continue;
                }
                break;
            }

            pedido.cantidad = cantidad;
            pedido.precioUnitario = precioUnitario;

            while (true)
            {
                Console.WriteLine("\nTipos de entrega disponibles:");
                Console.WriteLine("1. Retiro en tienda");
                Console.WriteLine("2. Entrega estándar");
                Console.WriteLine("3. Entrega rápida");
                Console.Write("Escoja un tipo de entrega: ");

                string entradaEntrega = (Console.ReadLine() ?? string.Empty).Trim();
                if (!int.TryParse(entradaEntrega, out int opcionEntrega))
                {
                    Console.WriteLine("\nTipo de entrega no válido.");
                    continue;
                }

                if (opcionEntrega == 1)
                {
                    pedido.tipoEntrega = tipoEntrega.RetiroEnTienda;
                    break;
                }
                if (opcionEntrega == 2)
                {
                    pedido.tipoEntrega = tipoEntrega.EntregaEstandar;
                    break;
                }
                if (opcionEntrega == 3)
                {
                    pedido.tipoEntrega = tipoEntrega.EntregaRapida;
                    break;
                }

                Console.WriteLine("\nTipo de entrega no válido.");
            }

            pedido.fechaPedido = DateTime.Now;
            pedido.estadoPedido = EstadoPedido.Pendiente;

            pedidos.Add(pedido);
            Console.WriteLine($"\nPedido {pedido.codigoPedido} registrado correctamente.");
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
                    _ => 0,
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

            Console.WriteLine("\nBuscar pedido por:");
            Console.WriteLine("1. Código");
            Console.WriteLine("2. Cliente");
            Console.WriteLine("3. Producto");
            Console.Write("Seleccione una opción: ");

            string entrada = (Console.ReadLine() ?? string.Empty).Trim();
            if (!int.TryParse(entrada, out int opcion) || opcion < 1 || opcion > 3)
            {
                Console.WriteLine("\nOpción inválida.");
                return;
            }

            List<Pedido> resultados = new List<Pedido>();

            switch (opcion)
            {
                case 1:
                    Console.Write("\nIngrese el código del pedido a buscar: ");
                    string codigo = (Console.ReadLine() ?? string.Empty).Trim();
                    if (string.IsNullOrWhiteSpace(codigo))
                    {
                        Console.WriteLine("\nDebe ingresar un código de pedido.");
                        return;
                    }
                    resultados = pedidos.Where(p => string.Equals(p.codigoPedido, codigo, StringComparison.OrdinalIgnoreCase)).ToList();
                    break;

                case 2:
                    Console.Write("\nIngrese el nombre del cliente: ");
                    string cliente = (Console.ReadLine() ?? string.Empty).Trim();
                    if (string.IsNullOrWhiteSpace(cliente))
                    {
                        Console.WriteLine("\nDebe ingresar un nombre de cliente.");
                        return;
                    }
                    resultados = pedidos.Where(p => p.nombreCliente.Contains(cliente, StringComparison.OrdinalIgnoreCase)).ToList();
                    break;

                case 3:
                    Console.Write("\nIngrese el producto solicitado: ");
                    string producto = (Console.ReadLine() ?? string.Empty).Trim();
                    if (string.IsNullOrWhiteSpace(producto))
                    {
                        Console.WriteLine("\nDebe ingresar un producto.");
                        return;
                    }
                    resultados = pedidos.Where(p => p.productoSolicitado.Contains(producto, StringComparison.OrdinalIgnoreCase)).ToList();
                    break;
            }

            if (resultados.Count == 0)
            {
                Console.WriteLine("\nNo se encontraron pedidos con esos datos.");
                return;
            }

            Console.WriteLine($"\nSe encontraron {resultados.Count} pedido(s):");
            foreach (var pedidoEncontrado in resultados)
            {
                PedidoUtils.MostrarDetallePedido(pedidoEncontrado);
            }
        }

        public static void ModificarPedido(ref List<Pedido> pedidos)
        {
            if (pedidos.Count == 0)
            {
                Console.WriteLine("\nNo hay pedidos registrados.");
                return;
            }

            Console.Write("\nIngrese el código del pedido a modificar: ");
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

            Console.WriteLine("\nQué propiedad desea modificar?");
            Console.WriteLine("1. Código del pedido");
            Console.WriteLine("2. Nombre del cliente");
            Console.WriteLine("3. Producto solicitado");
            Console.WriteLine("4. Cantidad");
            Console.WriteLine("5. Precio unitario");
            Console.WriteLine("6. Tipo de entrega");
            Console.WriteLine("7. Fecha de pedido");
            Console.Write("Seleccione una opción: ");

            string entradaPropiedad = (Console.ReadLine() ?? string.Empty).Trim();
            if (!int.TryParse(entradaPropiedad, out int opcionPropiedad))
            {
                Console.WriteLine("\nFormato no válido");
                return;
            }

            switch (opcionPropiedad)
            {
                case 1:
                    Console.Write("\nIngrese el nuevo código del pedido: ");
                    string nuevoCodigo = (Console.ReadLine() ?? string.Empty).Trim();
                    while (string.IsNullOrEmpty(nuevoCodigo) || pedidos.Any(p => string.Equals(p.codigoPedido, nuevoCodigo, StringComparison.OrdinalIgnoreCase) && !string.Equals(p.codigoPedido, pedido.codigoPedido, StringComparison.OrdinalIgnoreCase)))
                    {
                        Console.WriteLine("\nEl código no puede estar vacío o repetido.");
                        Console.Write("\nIngrese el nuevo código del pedido: ");
                        nuevoCodigo = (Console.ReadLine() ?? string.Empty).Trim();
                    }
                    pedido.codigoPedido = nuevoCodigo;
                    break;

                case 2:
                    Console.Write("\nIngrese el nuevo nombre del cliente: ");
                    string nuevoCliente = (Console.ReadLine() ?? string.Empty).Trim();
                    while (string.IsNullOrEmpty(nuevoCliente))
                    {
                        Console.WriteLine("\nEl nombre no puede estar vacío.");
                        Console.Write("\nIngrese el nuevo nombre del cliente: ");
                        nuevoCliente = (Console.ReadLine() ?? string.Empty).Trim();
                    }
                    pedido.nombreCliente = nuevoCliente;
                    break;

                case 3:
                    Console.Write("\nIngrese el nuevo producto solicitado: ");
                    string nuevoProducto = (Console.ReadLine() ?? string.Empty).Trim();
                    while (string.IsNullOrEmpty(nuevoProducto))
                    {
                        Console.WriteLine("\nEl producto no puede estar vacío.");
                        Console.Write("\nIngrese el nuevo producto solicitado: ");
                        nuevoProducto = (Console.ReadLine() ?? string.Empty).Trim();
                    }
                    pedido.productoSolicitado = nuevoProducto;
                    break;

                case 4:
                    Console.Write("\nIngrese la nueva cantidad: ");
                    string entradaCantidad = (Console.ReadLine() ?? string.Empty).Trim();
                    if (!int.TryParse(entradaCantidad, out int nuevaCantidad) || nuevaCantidad <= 0)
                    {
                        Console.WriteLine("\nCantidad no válida.");
                        return;
                    }
                    pedido.cantidad = nuevaCantidad;
                    break;

                case 5:
                    Console.Write("\nIngrese el nuevo precio unitario: ");
                    string entradaPrecio = (Console.ReadLine() ?? string.Empty).Trim();
                    if (!double.TryParse(entradaPrecio, out double nuevoPrecio) || nuevoPrecio <= 0)
                    {
                        Console.WriteLine("\nPrecio no válido.");
                        return;
                    }
                    pedido.precioUnitario = nuevoPrecio;
                    break;

                case 6:
                    Console.WriteLine("\nTipos de entrega disponibles:");
                    Console.WriteLine("1. Retiro en tienda");
                    Console.WriteLine("2. Entrega estándar");
                    Console.WriteLine("3. Entrega rápida");
                    Console.Write("Escoja un tipo de entrega: ");
                    string entradaEntrega = (Console.ReadLine() ?? string.Empty).Trim();
                    if (!int.TryParse(entradaEntrega, out int opcionEntrega))
                    {
                        Console.WriteLine("\nTipo de entrega no válido.");
                        return;
                    }
                    if (opcionEntrega == 1)
                    {
                        pedido.tipoEntrega = tipoEntrega.RetiroEnTienda;
                    }
                    else if (opcionEntrega == 2)
                    {
                        pedido.tipoEntrega = tipoEntrega.EntregaEstandar;
                    }
                    else if (opcionEntrega == 3)
                    {
                        pedido.tipoEntrega = tipoEntrega.EntregaRapida;
                    }
                    else
                    {
                        Console.WriteLine("\nTipo de entrega no válido.");
                        return;
                    }
                    break;

                case 7:
                    Console.Write("\nIngrese la nueva fecha de pedido (dd/MM/yyyy): ");
                    string entradaFecha = (Console.ReadLine() ?? string.Empty).Trim();
                    if (!DateTime.TryParse(entradaFecha, out DateTime nuevaFecha))
                    {
                        Console.WriteLine("\nFecha no válida.");
                        return;
                    }
                    pedido.fechaPedido = nuevaFecha;
                    break;

                default:
                    Console.WriteLine("\nOpción no válida.");
                    break;
            }

            Console.WriteLine($"\nPedido {pedido.codigoPedido} modificado correctamente.");
        }

        public static void MostrarRankingPedidos(List<Pedido> pedidos)
        {
            if (pedidos.Count == 0)
            {
                Console.WriteLine("\nNo hay pedidos registrados.");
                return;
            }

            var ranking = pedidos.OrderByDescending(PedidoUtils.CalcularTotalPedido).ToList();

            Console.WriteLine("\n-------------------------------------------------------------------------------");
            Console.WriteLine("RANKING DE PEDIDOS (total descendente)");
            Console.WriteLine("-------------------------------------------------------------------------------");

            foreach (var pedido in ranking)
            {
                double total = PedidoUtils.CalcularTotalPedido(pedido);
                Console.WriteLine($"Código: {pedido.codigoPedido} | Cliente: {pedido.nombreCliente} | Total: ${total:F2}");
            }
        }

        public static void EliminarPedido(ref List<Pedido> pedidos)
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

            var pedidoAEliminar = pedidos.FirstOrDefault(p => string.Equals(p.codigoPedido, codigo, StringComparison.OrdinalIgnoreCase));
            if (pedidoAEliminar == null)
            {
                Console.WriteLine("\nPedido no encontrado.");
                return;
            }

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
            Console.WriteLine("5. Total");
            Console.WriteLine("6. Fecha");

            Console.Write("\nEscoja un criterio: ");
            string entrada = (Console.ReadLine() ?? string.Empty).Trim();
            if (!int.TryParse(entrada, out int opcion))
            {
                Console.WriteLine("\nCriterio no válido.");
                return;
            }

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
                    if (string.IsNullOrWhiteSpace(cliente))
                    {
                        Console.WriteLine("\nDebe ingresar un nombre de cliente.");
                        return;
                    }
                    resultado = pedidos.Where(p => p.nombreCliente.Contains(cliente, StringComparison.OrdinalIgnoreCase)).ToList();
                    break;

                case 3:
                    Console.Write("Ingrese el producto: ");
                    string producto = (Console.ReadLine() ?? string.Empty).Trim();
                    if (string.IsNullOrWhiteSpace(producto))
                    {
                        Console.WriteLine("\nDebe ingresar un producto.");
                        return;
                    }
                    resultado = pedidos.Where(p => p.productoSolicitado.Contains(producto, StringComparison.OrdinalIgnoreCase)).ToList();
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

                case 5:
                    Console.Write("\nIngrese el monto mínimo total: ");
                    if (!double.TryParse((Console.ReadLine() ?? string.Empty).Trim(), out double minimo) || minimo < 0)
                    {
                        Console.WriteLine("\nMonto mínimo no válido.");
                        return;
                    }
                    Console.Write("Ingrese el monto máximo total: ");
                    if (!double.TryParse((Console.ReadLine() ?? string.Empty).Trim(), out double maximo) || maximo < minimo)
                    {
                        Console.WriteLine("\nMonto máximo no válido.");
                        return;
                    }
                    resultado = pedidos.Where(p =>
                    {
                        double totalPedido = PedidoUtils.CalcularTotalPedido(p);
                        return totalPedido >= minimo && totalPedido <= maximo;
                    }).ToList();
                    break;

                case 6:
                    Console.Write("\nIngrese la fecha inicial (dd/MM/yyyy): ");
                    if (!DateTime.TryParse((Console.ReadLine() ?? string.Empty).Trim(), out DateTime fechaInicio))
                    {
                        Console.WriteLine("\nFecha inicial no válida.");
                        return;
                    }
                    Console.Write("Ingrese la fecha final (dd/MM/yyyy): ");
                    if (!DateTime.TryParse((Console.ReadLine() ?? string.Empty).Trim(), out DateTime fechaFin) || fechaFin < fechaInicio)
                    {
                        Console.WriteLine("\nFecha final no válida.");
                        return;
                    }
                    resultado = pedidos.Where(p => p.fechaPedido.Date >= fechaInicio.Date && p.fechaPedido.Date <= fechaFin.Date).ToList();
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

        public static void MostrarEstadisticas(List<Pedido> pedidos)
        {
            if (pedidos.Count == 0)
            {
                Console.WriteLine("\nNo hay pedidos registrados.");
                return;
            }

            double totalFacturado = pedidos.Sum(PedidoUtils.CalcularTotalPedido);
            double promedio = totalFacturado / pedidos.Count;

            Console.WriteLine("\n-------------------------------------------------------------------------------");
            Console.WriteLine("ESTADÍSTICAS DE PEDIDOS");
            Console.WriteLine("-------------------------------------------------------------------------------");
            Console.WriteLine($"Total de pedidos: {pedidos.Count}");
            Console.WriteLine($"Total facturado: ${totalFacturado:F2}");
            Console.WriteLine($"Promedio por pedido: ${promedio:F2}");

            Console.WriteLine("\nPedidos por estado:");
            foreach (var grupoEstado in pedidos.GroupBy(p => p.estadoPedido))
            {
                Console.WriteLine($"- {grupoEstado.Key}: {grupoEstado.Count()}");
            }

            Console.WriteLine("\nPedidos por tipo de entrega:");
            foreach (var grupoEntrega in pedidos.GroupBy(p => p.tipoEntrega))
            {
                Console.WriteLine($"- {grupoEntrega.Key}: {grupoEntrega.Count()}");
            }

            var clienteMasPedidos = pedidos
                .GroupBy(p => p.nombreCliente)
                .Select(g => new { Cliente = g.Key, Cantidad = g.Count(), Total = g.Sum(PedidoUtils.CalcularTotalPedido) })
                .OrderByDescending(x => x.Cantidad)
                .ThenByDescending(x => x.Total)
                .FirstOrDefault();

            if (clienteMasPedidos != null)
            {
                Console.WriteLine($"\nCliente con más pedidos: {clienteMasPedidos.Cliente} ({clienteMasPedidos.Cantidad} pedido(s), total ${clienteMasPedidos.Total:F2})");
            }

            var clienteMasFacturo = pedidos
                .GroupBy(p => p.nombreCliente)
                .Select(g => new { Cliente = g.Key, Total = g.Sum(PedidoUtils.CalcularTotalPedido) })
                .OrderByDescending(x => x.Total)
                .FirstOrDefault();

            if (clienteMasFacturo != null)
            {
                Console.WriteLine($"Cliente que más facturó: {clienteMasFacturo.Cliente} (${clienteMasFacturo.Total:F2})");
            }
        }
    }
}
