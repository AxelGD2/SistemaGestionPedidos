using System;
using System.Collections.Generic;
using SistemaGestionPedidos.Services;

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
                    string entrada = Console.ReadLine() ?? string.Empty;

                    if (!int.TryParse(entrada.Trim(), out int opcion))
                    {
                        Console.WriteLine("\nFormato no válido");
                        continue;
                    }

                    switch (opcion)
                    {
                        case 1:
                            PedidoService.RegistrarPedidos(ref pedidos);
                            break;

                        case 2:
                            PedidoService.MostrarPedidos(pedidos);
                            break;

                        case 3:
                            PedidoService.BuscarPedido(pedidos);
                            break;

                        case 4:
                            PedidoService.ModificarPedido(ref pedidos);
                            break;

                        case 5:
                            PedidoStateService.CambiarEstado(ref pedidos);
                            break;

                        case 6:
                            PedidoService.EliminarPedido(ref pedidos);
                            break;

                        case 7:
                            PedidoService.FiltrarPedidos(pedidos);
                            break;

                        case 8:
                            PedidoService.MostrarEstadisticas(pedidos);
                            break;

                        case 9:
                            PedidoService.MostrarRankingPedidos(pedidos);
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
                catch (Exception ex)
                {
                    Console.WriteLine($"\nError: {ex.Message}");
                }
            }
        }
    }
}
