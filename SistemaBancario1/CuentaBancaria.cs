using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaBancario1
{
    /// Enumeración de tipos de cuenta

    public enum TipoCuenta
    {
        Ahorro,
        Cheques,
        Inversion
    }

    public class CuentaBancaria
    {
        // Propiedades
        public string NumeroCuenta { get; set; }
        public int IdCliente { get; set; }
        public TipoCuenta Tipo { get; set; }
        public decimal Saldo { get; private set; }
        public DateTime FechaApertura { get; set; }
        public bool Activa { get; set; }
        public List<Movimiento> Movimientos { get; set; }

        // Constantes
        private const decimal SALDO_MINIMO_AHORRO = 100m;
        private const decimal SALDO_MINIMO_CHEQUES = 500m;
        private const decimal SALDO_MINIMO_INVERSION = 5000m;
        private const decimal COMISION_RETIRO = 5m;

        // Constructor
        public CuentaBancaria()
        {
            FechaApertura = DateTime.Now;
            Activa = true;
            Movimientos = new List<Movimiento>();
            Saldo = 0m;
        }

        public CuentaBancaria(string numeroCuenta, int idCliente, TipoCuenta tipo, decimal saldoInicial)
        {
            NumeroCuenta = numeroCuenta;
            IdCliente = idCliente;
            Tipo = tipo;
            FechaApertura = DateTime.Now;
            Activa = true;
            Movimientos = new List<Movimiento>();

            // Validar saldo mínimo según el tipo de cuenta
            ValidarSaldoMinimo(saldoInicial, tipo);
            Saldo = saldoInicial;

            // Registrar apertura
            RegistrarMovimiento("Apertura de cuenta", saldoInicial, "DEPOSITO");
        }


        /// Valida que el saldo inicial cumpla con el mínimo requerido

        private void ValidarSaldoMinimo(decimal monto, TipoCuenta tipo)
        {
            decimal minimoRequerido = 0m;

            switch (tipo)
            {
                case TipoCuenta.Ahorro:
                    minimoRequerido = SALDO_MINIMO_AHORRO;
                    break;
                case TipoCuenta.Cheques:
                    minimoRequerido = SALDO_MINIMO_CHEQUES;
                    break;
                case TipoCuenta.Inversion:
                    minimoRequerido = SALDO_MINIMO_INVERSION;
                    break;
            }

            if (monto < minimoRequerido)
            {
                throw new InvalidOperationException(
                    string.Format("El saldo mínimo para cuenta de {0} es ${1:N2}", tipo, minimoRequerido));
            }
        }


        /// Realiza un depósito en la cuenta

        public bool Depositar(decimal monto, string concepto = "Depósito")
        {
            if (monto <= 0)
            {
                throw new ArgumentException("El monto debe ser mayor a cero");
            }

            if (!Activa)
            {
                throw new InvalidOperationException("La cuenta está inactiva");
            }

            Saldo += monto;
            RegistrarMovimiento(concepto, monto, "DEPOSITO");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n✓ Depósito exitoso: ${0:N2}", monto);
            Console.WriteLine("  Nuevo saldo: ${0:N2}", Saldo);
            Console.ResetColor();

            return true;
        }

        /// Realiza un retiro de la cuenta

        public bool Retirar(decimal monto, string concepto = "Retiro")
        {
            if (monto <= 0)
            {
                throw new ArgumentException("El monto debe ser mayor a cero");
            }

            if (!Activa)
            {
                throw new InvalidOperationException("La cuenta está inactiva");
            }

            // Calcular monto total (incluyendo comisión para cuentas de ahorro)
            decimal montoTotal = monto;
            if (Tipo == TipoCuenta.Ahorro)
            {
                montoTotal += COMISION_RETIRO;
            }

            if (Saldo < montoTotal)
            {
                throw new InvalidOperationException(
                    string.Format("Saldo insuficiente. Disponible: ${0:N2}, Requerido: ${1:N2}", Saldo, montoTotal));
            }

            Saldo -= montoTotal;
            RegistrarMovimiento(concepto, monto, "RETIRO");

            if (Tipo == TipoCuenta.Ahorro && COMISION_RETIRO > 0)
            {
                RegistrarMovimiento("Comisión por retiro", COMISION_RETIRO, "COMISION");
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n✓ Retiro exitoso: ${0:N2}", monto);
            if (Tipo == TipoCuenta.Ahorro)
            {
                Console.WriteLine("  Comisión aplicada: ${0:N2}", COMISION_RETIRO);
            }
            Console.WriteLine("  Nuevo saldo: ${0:N2}", Saldo);
            Console.ResetColor();

            return true;
        }


        /// Realiza una transferencia a otra cuenta

        public bool Transferir(CuentaBancaria cuentaDestino, decimal monto, string concepto = "Transferencia")
        {
            if (cuentaDestino == null)
            {
                throw new ArgumentNullException("La cuenta destino no existe");
            }

            if (NumeroCuenta == cuentaDestino.NumeroCuenta)
            {
                throw new InvalidOperationException("No puede transferir a la misma cuenta");
            }

            // Retirar de cuenta origen
            Retirar(monto, string.Format("Transferencia enviada - {0}", concepto));

            // Depositar en cuenta destino
            cuentaDestino.Depositar(monto, string.Format("Transferencia recibida - {0}", concepto));

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n✓ Transferencia exitosa");
            Console.WriteLine("  De: {0}", NumeroCuenta);
            Console.WriteLine("  Para: {0}", cuentaDestino.NumeroCuenta);
            Console.WriteLine("  Monto: ${0:N2}", monto);
            Console.ResetColor();

            return true;
        }

        /// Registra un movimiento en el historial

        private void RegistrarMovimiento(string concepto, decimal monto, string tipo)
        {
            Movimientos.Add(new Movimiento
            {
                Fecha = DateTime.Now,
                Concepto = concepto,
                Monto = monto,
                Tipo = tipo,
                SaldoResultante = Saldo
            });
        }


        /// Muestra el estado de cuenta

        public void MostrarEstadoCuenta()
        {
            Console.WriteLine("\n╔══════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                      ESTADO DE CUENTA                        ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════════╝");
            Console.WriteLine("\n  Número de Cuenta: {0}", NumeroCuenta);
            Console.WriteLine("  Tipo: {0}", Tipo);
            Console.WriteLine("  Fecha de Apertura: {0:dd/MM/yyyy}", FechaApertura);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n  SALDO ACTUAL: ${0:N2}", Saldo);
            Console.ResetColor();

            Console.ForegroundColor = Activa ? ConsoleColor.Green : ConsoleColor.Red;
            Console.WriteLine("  Estado: {0}", Activa ? "ACTIVA ✓" : "INACTIVA ✗");
            Console.ResetColor();
        }

        /// Muestra el historial de movimientos

        public void MostrarMovimientos(int cantidad = 10)
        {
            if (Movimientos.Count == 0)
            {
                Console.WriteLine("\n⚠ No hay movimientos registrados.");
                return;
            }

            Console.WriteLine("\n╔══════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                  HISTORIAL DE MOVIMIENTOS                    ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════════╝");

            var movimientosRecientes = Movimientos
                .OrderByDescending(m => m.Fecha)
                .Take(cantidad)
                .ToList();

            Console.WriteLine("\n{0,-20} {1,-30} {2,-12} {3,12} {4,12}",
                             "Fecha", "Concepto", "Tipo", "Monto", "Saldo");
            Console.WriteLine(new string('─', 88));

            foreach (var mov in movimientosRecientes)
            {
                ConsoleColor color = ConsoleColor.White;

                if (mov.Tipo == "DEPOSITO")
                    color = ConsoleColor.Green;
                else if (mov.Tipo == "RETIRO")
                    color = ConsoleColor.Yellow;
                else if (mov.Tipo == "COMISION")
                    color = ConsoleColor.Red;

                Console.ForegroundColor = color;

                string signo = (mov.Tipo == "RETIRO" || mov.Tipo == "COMISION") ? "-" : "+";

                Console.WriteLine("{0,-20:dd/MM/yyyy HH:mm} {1,-30} {2,-12} {3}{4,11:N2} ${5,11:N2}",
                                mov.Fecha,
                                mov.Concepto.Length > 30 ? mov.Concepto.Substring(0, 27) + "..." : mov.Concepto,
                                mov.Tipo,
                                signo,
                                mov.Monto,
                                mov.SaldoResultante);
                Console.ResetColor();
            }

            Console.WriteLine("\nTotal de movimientos: {0}", Movimientos.Count);
        }

    /// Serializa la cuenta para guardar en archivo

        public override string ToString()
        {
            return string.Format("{0}|{1}|{2}|{3}|{4:yyyy-MM-dd}|{5}",
                   NumeroCuenta, IdCliente, Tipo, Saldo, FechaApertura, Activa);
        }
    }

    /// Clase que representa un movimiento bancario

    public class Movimiento
    {
        public DateTime Fecha { get; set; }
        public string Concepto { get; set; }
        public decimal Monto { get; set; }
        public string Tipo { get; set; }  // DEPOSITO, RETIRO, COMISION
        public decimal SaldoResultante { get; set; }

        public override string ToString()
        {
            return string.Format("{0:yyyy-MM-dd HH:mm:ss}|{1}|{2}|{3}|{4}",
                   Fecha, Concepto, Monto, Tipo, SaldoResultante);
        }
    }
}
