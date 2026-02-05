using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaBancario1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Sistema Bancario";
            Banco banco = new Banco("BANCO del TECNM");
            bool salir = false;

            MostrarBienvenida();

            while (!salir)
            {
                MostrarMenuPrincipal();
                string opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        MenuClientes(banco);
                        break;
                    case "2":
                        MenuCuentas(banco);
                        break;
                    case "3":
                        MenuOperaciones(banco);
                        break;
                    case "4":
                        MenuReportes(banco);
                        break;
                    case "5":
                        salir = true;
                        MostrarDespedida();
                        break;
                    default:
                        Console.WriteLine("\n Opción inválida. Intente nuevamente.");
                        break;
                }

                if (!salir)
                {
                    Console.WriteLine("\nPresione cualquier tecla para continuar...");
                    Console.ReadKey();
                }
            }
        }


        // Muestra la pantalla de bienvenida

        static void MostrarBienvenida()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔══════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                                                              ║");
            Console.WriteLine("║                     BANCO DEL TECNM                          ║");
            Console.WriteLine("║                Sistema de Gestión Bancario                   ║");
            Console.WriteLine("║                                                              ║");
            Console.WriteLine("║           \"Tu confianza es nuestro compromiso\"               ║");
            Console.WriteLine("║                                                              ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════════╝");
            Console.ResetColor();

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        // Muestra el menú principal

        static void MostrarMenuPrincipal()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("╔══════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                       MENÚ PRINCIPAL                         ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════════╝");
            Console.ResetColor();

            Console.WriteLine("\n  1.  Gestión de Clientes");
            Console.WriteLine("  2.  Gestión de Cuentas");
            Console.WriteLine("  3.  Operaciones Bancarias");
            Console.WriteLine("  4.  Reportes y Consultas");
            Console.WriteLine("  5.  Salir del Sistema");

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("\n══════════════════════════════════════════════════════════════");
            Console.ResetColor();
            Console.Write("Seleccione una opción: ");
        }

        // Muestra el mensaje de despedida

        static void MostrarDespedida()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n╔══════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                                                              ║");
            Console.WriteLine("║               Gracias por usar este Banco                    ║");
            Console.WriteLine("║                                                              ║");
            Console.WriteLine("║              ¡Que tenga un excelente día!                    ║");
            Console.WriteLine("║                                                              ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════════╝\n");
            Console.ResetColor();
        }

        // Menú de gestión de clientes

        static void MenuClientes(Banco banco)
        {
            bool volver = false;

            while (!volver)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("╔══════════════════════════════════════════════════════════════╗");
                Console.WriteLine("║                   GESTIÓN DE CLIENTES                        ║");
                Console.WriteLine("╚══════════════════════════════════════════════════════════════╝");
                Console.ResetColor();

                Console.WriteLine("\n  1.  Registrar nuevo cliente");
                Console.WriteLine("  2.  Listar todos los clientes");
                Console.WriteLine("  3.  Buscar cliente");
                Console.WriteLine("  4.  Consultar datos de cliente");
                Console.WriteLine("  5.  Volver al menú principal");

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n══════════════════════════════════════════════════════════════");
                Console.ResetColor();
                Console.Write("Seleccione una opción: ");
                string opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        RegistrarNuevoCliente(banco);
                        break;
                    case "2":
                        banco.ListarClientes();
                        break;
                    case "3":
                        BuscarCliente(banco);
                        break;
                    case "4":
                        ConsultarDatosCliente(banco);
                        break;
                    case "5":
                        volver = true;
                        break;
                    default:
                        Console.WriteLine("\n Opción inválida.");
                        break;
                }

                if (!volver)
                {
                    Console.WriteLine("\nPresione cualquier tecla para continuar...");
                    Console.ReadKey();
                }
            }
        }

        // Registra un nuevo cliente

        static void RegistrarNuevoCliente(Banco banco)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔══════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                 REGISTRO DE NUEVO CLIENTE                    ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════════╝");
            Console.ResetColor();

            try
            {
                Console.Write("\nNombre(s): ");
                string nombre = Console.ReadLine();

                Console.Write("Apellido Paterno: ");
                string apellidoPaterno = Console.ReadLine();

                Console.Write("Apellido Materno: ");
                string apellidoMaterno = Console.ReadLine();

                Console.Write("CURP (18 caracteres): ");
                string curp = Console.ReadLine();

                // SOLUCIÓN MEJORADA PARA FECHA
                Console.Write("Fecha de Nacimiento (dd/mm/aaaa): ");
                string fechaInput = Console.ReadLine();

                DateTime fechaNacimiento;

                // Intentar parsear con cultura específica mexicana
                System.Globalization.CultureInfo culturaES = new System.Globalization.CultureInfo("es-MX");

                if (!DateTime.TryParse(fechaInput, culturaES, System.Globalization.DateTimeStyles.None, out fechaNacimiento))
                {
                    // Si falla, intentar con formato exacto dd/MM/yyyy
                    string[] formatosAceptados = { "dd/MM/yyyy", "dd-MM-yyyy", "dd/mm/yyyy", "dd-mm-yyyy" };

                    if (!DateTime.TryParseExact(fechaInput, formatosAceptados,
                        System.Globalization.CultureInfo.InvariantCulture,
                        System.Globalization.DateTimeStyles.None,
                        out fechaNacimiento))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\n Error: Formato de fecha inválido.");
                        Console.WriteLine("   Ejemplos válidos: 15/03/1990  o  15-03-1990");
                        Console.ResetColor();
                        return;
                    }
                }

                Console.Write("Teléfono: ");
                string telefono = Console.ReadLine();

                Console.Write("Email: ");
                string email = Console.ReadLine();

                Console.Write("Dirección: ");
                string direccion = Console.ReadLine();

                banco.RegistrarCliente(nombre, apellidoPaterno, apellidoMaterno, curp,
                                      fechaNacimiento, telefono, email, direccion);
            }
            catch (ArgumentException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n Error de validación: {0}", ex.Message);
                Console.ResetColor();
            }
            catch (InvalidOperationException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n Error: {0}", ex.Message);
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n Error inesperado: {0}", ex.Message);
                Console.ResetColor();
            }
        }
        /// Se busca clientes por criterio

        static void BuscarCliente(Banco banco)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("╔══════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                      BUSCAR CLIENTE                          ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════════╝");
            Console.ResetColor();

            Console.Write("\nIngrese nombre o CURP: ");
            string criterio = Console.ReadLine();

            var resultados = banco.BuscarClientes(criterio);

            if (resultados.Count == 0)
            {
                Console.WriteLine("\n No se encontraron clientes.");
            }
            else
            {
                Console.WriteLine($"\n✓ Se encontraron {resultados.Count} cliente(s):");
                foreach (var cliente in resultados)
                {
                    cliente.MostrarInformacion();
                }
            }
        }

        // Consulta los datos completos de un cliente

        static void ConsultarDatosCliente(Banco banco)
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                 CONSULTAR DATOS DE CLIENTE                   ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════════╝");

            try
            {
                Console.Write("\nIngrese el ID del cliente: ");
                int idCliente = int.Parse(Console.ReadLine());

                var cliente = banco.BuscarClientePorId(idCliente);
                cliente.MostrarInformacion();

                // Mostrar cuentas del cliente
                var cuentas = banco.ObtenerCuentasCliente(idCliente);
                if (cuentas.Count > 0)
                {
                    Console.WriteLine("\n CUENTAS ASOCIADAS:");
                    foreach (var cuenta in cuentas)
                    {
                        Console.WriteLine($"\n  Cuenta: {cuenta.NumeroCuenta}");
                        Console.WriteLine($"    Tipo: {cuenta.Tipo}");
                        Console.WriteLine($"    Saldo: ${cuenta.Saldo:N2}");
                    }
                }
                else
                {
                    Console.WriteLine("\n El cliente no tiene cuentas registradas.");
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("\n Debe ingresar un ID válido.");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"\n {ex.Message}");
            }
        }


        // Menú de gestión de cuentas

        static void MenuCuentas(Banco banco)
        {
            bool volver = false;

            while (!volver)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("╔══════════════════════════════════════════════════════════════╗");
                Console.WriteLine("║                   GESTIÓN DE CUENTAS                         ║");
                Console.WriteLine("╚══════════════════════════════════════════════════════════════╝");
                Console.ResetColor();

                Console.WriteLine("\n  1.  Abrir nueva cuenta");
                Console.WriteLine("  2.   Consultar saldo");
                Console.WriteLine("  3.   Ver estado de cuenta completo");
                Console.WriteLine("  4.   Ver todas las cuentas de un cliente");
                Console.WriteLine("  5.   Volver al menú principal");

                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("\n══════════════════════════════════════════════════════════════");
                Console.ResetColor();
                Console.Write("Seleccione una opción: ");
                string opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        AbrirNuevaCuenta(banco);
                        break;
                    case "2":
                        ConsultarSaldo(banco);
                        break;
                    case "3":
                        VerEstadoCuenta(banco);
                        break;
                    case "4":
                        VerCuentasCliente(banco);
                        break;
                    case "5":
                        volver = true;
                        break;
                    default:
                        Console.WriteLine("\n Opción inválida.");
                        break;
                }

                if (!volver)
                {
                    Console.WriteLine("\nPresione cualquier tecla para continuar...");
                    Console.ReadKey();
                }
            }
        }

        // Abre una nueva cuenta bancaria

        static void AbrirNuevaCuenta(Banco banco)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔══════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                    APERTURA DE CUENTA                        ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════════╝");
            Console.ResetColor();

            try
            {
                Console.Write("\nID del cliente: ");
                int idCliente = int.Parse(Console.ReadLine());

                // Verificar que el cliente existe
                var cliente = banco.BuscarClientePorId(idCliente);
                Console.WriteLine($"\nCliente: {cliente.NombreCompleto}");

                Console.WriteLine("\nSeleccione el tipo de cuenta:");
                Console.WriteLine("  1. Ahorro (Saldo mínimo: $100)");
                Console.WriteLine("  2. Cheques (Saldo mínimo: $500)");
                Console.WriteLine("  3. Inversión (Saldo mínimo: $5,000)");
                Console.Write("Opción: ");

                // Se captura la opción del usuario
                int tipoOpcion = int.Parse(Console.ReadLine());

                // Se declara la variable que guardará el Enum
                TipoCuenta tipo;

                switch (tipoOpcion)
                {
                    case 1:
                        tipo = TipoCuenta.Ahorro;
                        break;
                    case 2:
                        tipo = TipoCuenta.Cheques;
                        break;
                    case 3:
                        tipo = TipoCuenta.Inversion;
                        break;
                    default:
                        throw new ArgumentException("Tipo de cuenta inválido");
                }

                Console.Write("\nMonto de apertura: $");
                decimal saldoInicial = decimal.Parse(Console.ReadLine());

                banco.AbrirCuenta(idCliente, tipo, saldoInicial);
                Console.WriteLine("\n✓ Cuenta abierta exitosamente.");
            }
            catch (FormatException)
            {
                Console.WriteLine("\n Debe ingresar valores numéricos válidos.");
            }
            catch (ArgumentException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n {ex.Message}");
                Console.ResetColor();
            }
            catch (InvalidOperationException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n {ex.Message}");
                Console.ResetColor();
            }
        }

        // Consulta el saldo de una cuenta

        static void ConsultarSaldo(Banco banco)
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                    CONSULTA DE SALDO                         ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════════╝");

            try
            {
                Console.Write("\nNúmero de cuenta: ");
                string numeroCuenta = Console.ReadLine();

                banco.ConsultarSaldo(numeroCuenta);
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"\n {ex.Message}");
            }
        }


        /// Muestra el estado de cuenta completo

        static void VerEstadoCuenta(Banco banco)
        {
            try
            {
                Console.Write("\nNúmero de cuenta: ");
                string numeroCuenta = Console.ReadLine();

                Console.Write("¿Cuántos movimientos desea ver? (Enter para 10): ");
                string cantStr = Console.ReadLine();
                int cantidad = string.IsNullOrWhiteSpace(cantStr) ? 10 : int.Parse(cantStr);

                banco.MostrarEstadoCuenta(numeroCuenta, cantidad);
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"\n {ex.Message}");
            }
            catch (FormatException)
            {
                Console.WriteLine("\n Cantidad inválida.");
            }
        }

        // Ver todas las cuentas de un cliente

        static void VerCuentasCliente(Banco banco)
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║              CUENTAS DEL CLIENTE                             ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════════╝");

            try
            {
                Console.Write("\nID del cliente: ");
                int idCliente = int.Parse(Console.ReadLine());

                var cliente = banco.BuscarClientePorId(idCliente);
                Console.WriteLine($"\nCliente: {cliente.NombreCompleto}");

                var cuentas = banco.ObtenerCuentasCliente(idCliente);

                if (cuentas.Count == 0)
                {
                    Console.WriteLine("\n El cliente no tiene cuentas registradas.");
                }
                else
                {
                    decimal saldoTotal = 0;
                    foreach (var cuenta in cuentas)
                    {
                        cuenta.MostrarEstadoCuenta();
                        saldoTotal += cuenta.Saldo;
                    }

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\n SALDO TOTAL: ${saldoTotal:N2}");
                    Console.ResetColor();
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("\n ID inválido.");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"\n {ex.Message}");
            }
        }

        // Menú de operaciones bancarias

        static void MenuOperaciones(Banco banco)
        {
            bool volver = false;

            while (!volver)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("╔══════════════════════════════════════════════════════════════╗");
                Console.WriteLine("║                  OPERACIONES BANCARIAS                       ║");
                Console.WriteLine("╚══════════════════════════════════════════════════════════════╝");
                Console.ResetColor();

                Console.WriteLine("\n  1.  Realizar depósito");
                Console.WriteLine("  2.  Realizar retiro");
                Console.WriteLine("  3.  Realizar transferencia");
                Console.WriteLine("  4.  Volver al menú principal");

                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("\n══════════════════════════════════════════════════════════════");
                Console.ResetColor();
                Console.Write("Seleccione una opción: ");
                string opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        RealizarDeposito(banco);
                        break;
                    case "2":
                        RealizarRetiro(banco);
                        break;
                    case "3":
                        RealizarTransferencia(banco);
                        break;
                    case "4":
                        volver = true;
                        break;
                    default:
                        Console.WriteLine("\n Opción inválida.");
                        break;
                }

                if (!volver)
                {
                    Console.WriteLine("\nPresione cualquier tecla para continuar...");
                    Console.ReadKey();
                }
            }
        }


        // Realiza un depósito

        static void RealizarDeposito(Banco banco)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("╔══════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                       DEPÓSITO                               ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════════╝");
            Console.ResetColor();

            try
            {
                Console.Write("\nNúmero de cuenta: ");
                string numeroCuenta = Console.ReadLine();

                Console.Write("Monto a depositar: $");
                decimal monto = decimal.Parse(Console.ReadLine());

                Console.Write("Concepto (opcional): ");
                string concepto = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(concepto))
                    concepto = "Depósito en efectivo";

                banco.RealizarDeposito(numeroCuenta, monto, concepto);
            }
            catch (FormatException)
            {
                Console.WriteLine("\n Monto inválido.");
            }
            catch (ArgumentException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n {ex.Message}");
                Console.ResetColor();
            }
            catch (InvalidOperationException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n {ex.Message}");
                Console.ResetColor();
            }
        }

        // Realizar un retiro

        static void RealizarRetiro(Banco banco)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("╔══════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                        RETIRO                                ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════════╝");
            Console.ResetColor();

            try
            {
                Console.Write("\nNúmero de cuenta: ");
                string numeroCuenta = Console.ReadLine();

                // Mostrar saldo actual
                var cuenta = banco.BuscarCuenta(numeroCuenta);
                Console.WriteLine($"\nSaldo disponible: ${cuenta.Saldo:N2}");

                Console.Write("\nMonto a retirar: $");
                decimal monto = decimal.Parse(Console.ReadLine());

                Console.Write("Concepto (opcional): ");
                string concepto = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(concepto))
                    concepto = "Retiro en efectivo";

                banco.RealizarRetiro(numeroCuenta, monto, concepto);
            }
            catch (FormatException)
            {
                Console.WriteLine("\n Monto inválido.");
            }
            catch (ArgumentException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n {ex.Message}");
                Console.ResetColor();
            }
            catch (InvalidOperationException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n {ex.Message}");
                Console.ResetColor();
            }
        }

        // Realizar una transferencia
        static void RealizarTransferencia(Banco banco)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔══════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                     TRANSFERENCIA                            ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════════╝");
            Console.ResetColor();

            try
            {
                Console.Write("\nCuenta origen: ");
                string cuentaOrigen = Console.ReadLine();

                // Mostrar saldo de origen
                var cuenta = banco.BuscarCuenta(cuentaOrigen);
                Console.WriteLine($"Saldo disponible: ${cuenta.Saldo:N2}");

                Console.Write("\nCuenta destino: ");
                string cuentaDestino = Console.ReadLine();

                Console.Write("\nMonto a transferir: $");
                decimal monto = decimal.Parse(Console.ReadLine());

                Console.Write("Concepto (opcional): ");
                string concepto = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(concepto))
                    concepto = "Transferencia entre cuentas";

                banco.RealizarTransferencia(cuentaOrigen, cuentaDestino, monto, concepto);
            }
            catch (FormatException)
            {
                Console.WriteLine("\n Monto inválido.");
            }
            catch (ArgumentException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n {ex.Message}");
                Console.ResetColor();
            }
            catch (InvalidOperationException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n {ex.Message}");
                Console.ResetColor();
            }
        }



        // Menú de reportes

        static void MenuReportes(Banco banco)
        {
            bool volver = false;

            while (!volver)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("╔══════════════════════════════════════════════════════════════╗");
                Console.WriteLine("║                  REPORTES Y CONSULTAS                        ║");
                Console.WriteLine("╚══════════════════════════════════════════════════════════════╝");
                Console.ResetColor();

                Console.WriteLine("\n  1.  Reporte general del banco");
                Console.WriteLine("  2.  Estado de cuenta detallado");
                Console.WriteLine("  3.   Volver al menú principal");

                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("\n══════════════════════════════════════════════════════════════");
                Console.ResetColor();
                Console.Write("Seleccione una opción: ");
                string opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        banco.GenerarReporteGeneral();
                        break;
                    case "2":
                        VerEstadoCuenta(banco);
                        break;
                    case "3":
                        volver = true;
                        break;
                    default:
                        Console.WriteLine("\n Opción inválida.");
                        break;
                }

                if (!volver)
                {
                    Console.WriteLine("\nPresione cualquier tecla para continuar...");
                    Console.ReadKey();
                }
            }
        }
    }
    }

