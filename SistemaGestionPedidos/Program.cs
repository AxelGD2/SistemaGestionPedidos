using System.IO;
using System.Security.Cryptography;

namespace SistemaGestionPedidos
{
    public class Program
    {
        public static void Main(string[] args)
        {
            bool menuCorriendo = true;
            List<Pedido> pedidos = new List<Pedido>();

            while (menuCorriendo)
            {
                Console.WriteLine("\n-------------------------------------------------------------------------------");
                Console.WriteLine("SISTEMA DE GESTIÓN DE PEDIDOS");
                Console.WriteLine("-------------------------------------------------------------------------------");
                Console.WriteLine("1.Registrar pedido");
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
                            RegistrarPedidos(ref pedidos);
                            break;

                        case 2:
                            MostrarPedidos(pedidos);
                            break;

                        case 3:
                            BuscarPedido(pedidos);
                            break;

                        case 4:
                            ModificarPedido(ref pedidos);
                            break;

                        case 5:
                            break;

                        case 6:
                            break;

                        case 7:
                            break;

                        case 8:
                            break;

                        case 9:
                            MostrarRankingPedidos(pedidos);
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
            if (pedidos.Count == 0)
            {
                Console.WriteLine("\nNo hay pedidos registrados.");
                return;
            }

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
    }
}
