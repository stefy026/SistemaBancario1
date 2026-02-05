using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SistemaBancario1
{
    public class Banco
    {
        // Propiedades
        public string NombreBanco { get; set; }
        private List<Cliente> clientes;
        private List<CuentaBancaria> cuentas;
        private int contadorIdCliente;
        private int contadorNumeroCuenta;

        // Archivos de datos
        private const string ARCHIVO_CLIENTES = "clientes.txt";
        private const string ARCHIVO_CUENTAS = "cuentas.txt";
        private const string ARCHIVO_MOVIMIENTOS = "movimientos.txt";

        // Constructor
        public Banco(string nombreBanco = "Banco del Pueblo")
        {
            NombreBanco = nombreBanco;
            clientes = new List<Cliente>();
            cuentas = new List<CuentaBancaria>();
            contadorIdCliente = 1000;
            contadorNumeroCuenta = 10000;

            CargarDatos();
        }


        /// Registra un nuevo cliente en el banco

        public Cliente RegistrarCliente(string nombre, string apellidoPaterno, string apellidoMaterno,
                                       string curp, DateTime fechaNacimiento, string telefono,
                                       string email, string direccion)
        {
            // Validaciones
            ValidarDatosCliente(nombre, apellidoPaterno, curp, fechaNacimiento, telefono, email);

            // Verificar que no exista CURP duplicado
            if (clientes.Any(c => c.CURP == curp.ToUpper()))
            {
                throw new InvalidOperationException("Ya existe un cliente registrado con ese CURP");
            }

            // Crear cliente
            Cliente nuevoCliente = new Cliente(
                contadorIdCliente++,
                nombre,
                apellidoPaterno,
                apellidoMaterno,
                curp.ToUpper(),
                fechaNacimiento,
                telefono,
                email,
                direccion
            );

            clientes.Add(nuevoCliente);
            GuardarClientes();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n✓ Cliente registrado exitosamente!");
            Console.WriteLine("  ID Cliente: {0}", nuevoCliente.IdCliente);
            Console.WriteLine("  Nombre: {0}", nuevoCliente.NombreCompleto);
            Console.ResetColor();

            return nuevoCliente;
        }

        /// Valida los datos del cliente

        private void ValidarDatosCliente(string nombre, string apellidoPaterno, string curp,
                                        DateTime fechaNacimiento, string telefono, string email)
        {
            // Validar nombre
            if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(apellidoPaterno))
            {
                throw new ArgumentException("El nombre y apellido paterno son obligatorios");
            }

            // Validar CURP (18 caracteres alfanuméricos)
            if (string.IsNullOrWhiteSpace(curp) || curp.Length != 18)
            {
                throw new ArgumentException("El CURP debe tener 18 caracteres");
            }

            // Validar edad (mínimo 18 años)
            int edad = DateTime.Today.Year - fechaNacimiento.Year;
            if (fechaNacimiento.Date > DateTime.Today.AddYears(-edad)) edad--;

            if (edad < 18)
            {
                throw new ArgumentException("El cliente debe ser mayor de 18 años");
            }

            // Validar teléfono (10 dígitos)
            string patronTelefono = @"^[\d\s\-()]{10,15}$";
            if (!Regex.IsMatch(telefono, patronTelefono))
            {
                throw new ArgumentException("El teléfono debe tener entre 10 y 15 dígitos");
            }

            // Validar email
            if (!string.IsNullOrWhiteSpace(email))
            {
                string patronEmail = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                if (!Regex.IsMatch(email, patronEmail))
                {
                    throw new ArgumentException("El formato del email no es válido");
                }
            }
        }

        /// Busca un cliente por ID

        public Cliente BuscarClientePorId(int idCliente)
        {
            var cliente = clientes.FirstOrDefault(c => c.IdCliente == idCliente);

            if (cliente == null)
            {
                throw new InvalidOperationException(string.Format("No se encontró el cliente con ID {0}", idCliente));
            }

            return cliente;
        }

        /// Busca clientes por nombre o CURP

        public List<Cliente> BuscarClientes(string criterio)
        {
            return clientes
                .Where(c => c.NombreCompleto.ToLower().Contains(criterio.ToLower()) ||
                           c.CURP.ToLower().Contains(criterio.ToLower()))
                .ToList();
        }


        /// Lista todos los clientes

        public void ListarClientes()
        {
            if (clientes.Count == 0)
            {
                Console.WriteLine("\n No hay clientes registrados.");
                return;
            }

            Console.WriteLine("\n╔══════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                    LISTA DE CLIENTES                         ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════════╝");

            foreach (var cliente in clientes.Where(c => c.Activo))
            {
                cliente.MostrarInformacion();
            }

            Console.WriteLine("\nTotal de clientes activos: {0}", clientes.Count(c => c.Activo));
        }


        /// Abre una nueva cuenta bancaria para un cliente

        public CuentaBancaria AbrirCuenta(int idCliente, TipoCuenta tipo, decimal saldoInicial)
        {
            // Verificar que el cliente existe
            var cliente = BuscarClientePorId(idCliente);

            if (!cliente.Activo)
            {
                throw new InvalidOperationException("El cliente no está activo");
            }

            // Generar número de cuenta
            string numeroCuenta = contadorNumeroCuenta.ToString("D10");
            contadorNumeroCuenta++;

            // Crear cuenta
            CuentaBancaria nuevaCuenta = new CuentaBancaria(
                numeroCuenta,
                idCliente,
                tipo,
                saldoInicial
            );

            cuentas.Add(nuevaCuenta);
            GuardarCuentas();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n✓ Cuenta abierta exitosamente!");
            Console.WriteLine("  Número de cuenta: {0}", numeroCuenta);
            Console.WriteLine("  Tipo: {0}", tipo);
            Console.WriteLine("  Saldo inicial: ${0:N2}", saldoInicial);
            Console.ResetColor();

            return nuevaCuenta;
        }


        /// Busca una cuenta por número

        public CuentaBancaria BuscarCuenta(string numeroCuenta)
        {
            var cuenta = cuentas.FirstOrDefault(c => c.NumeroCuenta == numeroCuenta);

            if (cuenta == null)
            {
                throw new InvalidOperationException(string.Format("No se encontró la cuenta {0}", numeroCuenta));
            }

            return cuenta;
        }

        /// Obtiene todas las cuentas de un cliente

        public List<CuentaBancaria> ObtenerCuentasCliente(int idCliente)
        {
            return cuentas.Where(c => c.IdCliente == idCliente && c.Activa).ToList();
        }


        /// Consulta el saldo de una cuenta

        public void ConsultarSaldo(string numeroCuenta)
        {
            var cuenta = BuscarCuenta(numeroCuenta);
            cuenta.MostrarEstadoCuenta();
        }

        /// Realiza un depósito

        public void RealizarDeposito(string numeroCuenta, decimal monto, string concepto = "Depósito")
        {
            var cuenta = BuscarCuenta(numeroCuenta);
            cuenta.Depositar(monto, concepto);
            GuardarCuentas();
            GuardarMovimientos();
        }


        /// Realiza un retiro

        public void RealizarRetiro(string numeroCuenta, decimal monto, string concepto = "Retiro")
        {
            var cuenta = BuscarCuenta(numeroCuenta);
            cuenta.Retirar(monto, concepto);
            GuardarCuentas();
            GuardarMovimientos();
        }


        /// Realiza una transferencia entre cuentas

        public void RealizarTransferencia(string numeroCuentaOrigen, string numeroCuentaDestino,
                                         decimal monto, string concepto = "Transferencia")
        {
            var cuentaOrigen = BuscarCuenta(numeroCuentaOrigen);
            var cuentaDestino = BuscarCuenta(numeroCuentaDestino);

            cuentaOrigen.Transferir(cuentaDestino, monto, concepto);

            GuardarCuentas();
            GuardarMovimientos();
        }

        /// Muestra el estado de cuenta con movimientos

        public void MostrarEstadoCuenta(string numeroCuenta, int cantidadMovimientos = 10)
        {
            var cuenta = BuscarCuenta(numeroCuenta);
            var cliente = BuscarClientePorId(cuenta.IdCliente);

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔══════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║  {0,-58}  ║", NombreBanco);
            Console.WriteLine("╚══════════════════════════════════════════════════════════════╝");
            Console.ResetColor();

            Console.WriteLine("\nCliente: {0}", cliente.NombreCompleto);
            cuenta.MostrarEstadoCuenta();
            cuenta.MostrarMovimientos(cantidadMovimientos);
        }


        /// Genera un reporte general del banco

        public void GenerarReporteGeneral()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔══════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                    REPORTE GENERAL DEL BANCO                 ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════════╝");
            Console.ResetColor();

            Console.WriteLine("\n ESTADÍSTICAS GENERALES");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("═══════════════════════════════════════════════════════════════");
            Console.ResetColor();

            Console.WriteLine("\n  Total de clientes: {0}", clientes.Count(c => c.Activo));
            Console.WriteLine("  Total de cuentas: {0}", cuentas.Count(c => c.Activa));

            decimal saldoTotal = cuentas.Where(c => c.Activa).Sum(c => c.Saldo);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n   Saldo total en el banco: ${0:N2}", saldoTotal);
            Console.ResetColor();

            // Estadísticas por tipo de cuenta
            Console.WriteLine("\n CUENTAS POR TIPO:");
            var cuentasPorTipo = cuentas.Where(c => c.Activa).GroupBy(c => c.Tipo);
            foreach (var grupo in cuentasPorTipo)
            {
                decimal saldoTipo = grupo.Sum(c => c.Saldo);
                Console.WriteLine("  {0,-12}: {1,3} cuentas - ${2,15:N2}", grupo.Key, grupo.Count(), saldoTipo);
            }

            // Cliente con más cuentas
            var clienteMasCuentas = cuentas
                .Where(c => c.Activa)
                .GroupBy(c => c.IdCliente)
                .OrderByDescending(g => g.Count())
                .FirstOrDefault();

            if (clienteMasCuentas != null)
            {
                var cliente = BuscarClientePorId(clienteMasCuentas.Key);
                Console.WriteLine("\n  Cliente con más cuentas: {0} ({1} cuentas)",
                                cliente.NombreCompleto, clienteMasCuentas.Count());
            }
        }


        /// Guarda los clientes en archivo

        private void GuardarClientes()
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(ARCHIVO_CLIENTES))
                {
                    foreach (var cliente in clientes)
                    {
                        sw.WriteLine(cliente.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(" Error al guardar clientes: {0}", ex.Message);
            }
        }

        /// Guarda las cuentas en archivo

        private void GuardarCuentas()
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(ARCHIVO_CUENTAS))
                {
                    foreach (var cuenta in cuentas)
                    {
                        sw.WriteLine(cuenta.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(" Error al guardar cuentas: {0}", ex.Message);
            }
        }


        /// Guarda los movimientos en archivo

        private void GuardarMovimientos()
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(ARCHIVO_MOVIMIENTOS))
                {
                    foreach (var cuenta in cuentas)
                    {
                        foreach (var mov in cuenta.Movimientos)
                        {
                            sw.WriteLine("{0}|{1}", cuenta.NumeroCuenta, mov.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(" Error al guardar movimientos: {0}", ex.Message);
            }
        }


        /// Carga todos los datos desde archivos

        private void CargarDatos()
        {
            CargarClientes();
            CargarCuentas();
            CargarMovimientos();
        }


        /// Carga los clientes desde archivo

        private void CargarClientes()
        {
            try
            {
                if (File.Exists(ARCHIVO_CLIENTES))
                {
                    string[] lineas = File.ReadAllLines(ARCHIVO_CLIENTES);
                    foreach (string linea in lineas)
                    {
                        string[] datos = linea.Split('|');
                        if (datos.Length >= 11)
                        {
                            Cliente cliente = new Cliente
                            {
                                IdCliente = int.Parse(datos[0]),
                                Nombre = datos[1],
                                ApellidoPaterno = datos[2],
                                ApellidoMaterno = datos[3],
                                CURP = datos[4],
                                FechaNacimiento = DateTime.Parse(datos[5]),
                                Telefono = datos[6],
                                Email = datos[7],
                                Direccion = datos[8],
                                FechaRegistro = DateTime.Parse(datos[9]),
                                Activo = bool.Parse(datos[10])
                            };
                            clientes.Add(cliente);

                            if (cliente.IdCliente >= contadorIdCliente)
                                contadorIdCliente = cliente.IdCliente + 1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(" Error al cargar clientes: {0}", ex.Message);
            }
        }

        /// Carga las cuentas desde archivo

        private void CargarCuentas()
        {
            try
            {
                if (File.Exists(ARCHIVO_CUENTAS))
                {
                    string[] lineas = File.ReadAllLines(ARCHIVO_CUENTAS);
                    foreach (string linea in lineas)
                    {
                        string[] datos = linea.Split('|');
                        if (datos.Length >= 6)
                        {
                            CuentaBancaria cuenta = new CuentaBancaria
                            {
                                NumeroCuenta = datos[0],
                                IdCliente = int.Parse(datos[1]),
                                Tipo = (TipoCuenta)Enum.Parse(typeof(TipoCuenta), datos[2]),
                                FechaApertura = DateTime.Parse(datos[4]),
                                Activa = bool.Parse(datos[5])
                            };

                            // El saldo se establece mediante reflexión
                            typeof(CuentaBancaria)
                                .GetProperty("Saldo")
                                .SetValue(cuenta, decimal.Parse(datos[3]), null);

                            cuentas.Add(cuenta);

                            int numCuenta = int.Parse(cuenta.NumeroCuenta);
                            if (numCuenta >= contadorNumeroCuenta)
                                contadorNumeroCuenta = numCuenta + 1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(" Error al cargar cuentas: {0}", ex.Message);
            }
        }

        /// Carga los movimientos desde archivo

        private void CargarMovimientos()
        {
            try
            {
                if (File.Exists(ARCHIVO_MOVIMIENTOS))
                {
                    string[] lineas = File.ReadAllLines(ARCHIVO_MOVIMIENTOS);
                    foreach (string linea in lineas)
                    {
                        string[] datos = linea.Split('|');
                        if (datos.Length >= 6)
                        {
                            string numeroCuenta = datos[0];
                            var cuenta = cuentas.FirstOrDefault(c => c.NumeroCuenta == numeroCuenta);

                            if (cuenta != null)
                            {
                                Movimiento mov = new Movimiento
                                {
                                    Fecha = DateTime.Parse(datos[1]),
                                    Concepto = datos[2],
                                    Monto = decimal.Parse(datos[3]),
                                    Tipo = datos[4],
                                    SaldoResultante = decimal.Parse(datos[5])
                                };
                                cuenta.Movimientos.Add(mov);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(" Error al cargar movimientos: {0}", ex.Message);
            }
        }
    }
}
