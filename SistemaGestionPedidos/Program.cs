using System.IO;



namespace SistemaGestionPedidos
{
    public class Program
    {
        public static void Main(string[] args)
        {
            bool menuCorriendo = true;
            List<Pedido> pedidos = new List<Pedido>();
            double Total = 0;

            while(menuCorriendo)
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
                            RegistrarPedidos(ref pedidos, ref Total);
                            break;
                        
                        case 2:
                            break;

                        case 3:
                            break;

                        case 4:
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

        public static void RegistrarPedidos(ref List<Pedido> pedidos, ref double total)
        {
            Pedido pedido = new Pedido();
            double subtotal = 0;      

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

                while(cantidad <= 0)
                {
                    Console.Write("\nLa cantidad debe ser mayor a cero.");
                    Console.Write("\nIngrese la cantidad: ");
                    cantidad = int.Parse(Console.ReadLine().Trim());
                }

                Console.Write("\nIngrese el precio unitario: ");
                double precioUnitario = double.Parse(Console.ReadLine().Trim());

                while(precioUnitario <= 0)
                {
                    Console.Write("\nEl precio unitario debe ser mayor a cero.");
                    Console.Write("\nIngrese el precio unitario: ");
                    precioUnitario = double.Parse(Console.ReadLine().Trim());
                }

                subtotal = cantidad + precioUnitario;
                pedido.cantidad = cantidad;
                pedido.precioUnitario = precioUnitario;

                Console.Write("\nIngrese el tipo de entrega: ");
                Console.WriteLine("1.Retiro en tienda");
                Console.WriteLine("2.Entrega estándar");
                Console.WriteLine("3.Entrega rápida");

                Console.WriteLine("\nEscoja un tipo de entrega: ");
                int opcionEntrega = int.Parse(Console.ReadLine().Trim());

                switch (opcionEntrega)
                {
                    case 1:
                        pedido.tipoEntrega = "Retiro en tienda";
                        break;

                    case 2:
                        pedido.tipoEntrega = "Entrega estándar";
                        subtotal += 2.50;
                        break;

                    case 3:
                        pedido.tipoEntrega = "Entrega rápida";
                        subtotal += 5.0;
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
            pedido.estadoPedido = "Pendiente";
            total += subtotal;

            pedidos.Add(pedido);      

        }
    }
}