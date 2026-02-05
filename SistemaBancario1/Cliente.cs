using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaBancario1
{
    
        public class Cliente
        {
            // Propiedades
            public int IdCliente { get; set; }
            public string Nombre { get; set; }
            public string ApellidoPaterno { get; set; }
            public string ApellidoMaterno { get; set; }
            public string CURP { get; set; }
            public DateTime FechaNacimiento { get; set; }
            public string Telefono { get; set; }
            public string Email { get; set; }
            public string Direccion { get; set; }
            public DateTime FechaRegistro { get; set; }
            public bool Activo { get; set; }

            // Constructor por defecto
            public Cliente()
            {
                FechaRegistro = DateTime.Now;
                Activo = true;
            }

            // Constructor con parámetros
            public Cliente(int id, string nombre, string apellidoPaterno, string apellidoMaterno,
                          string curp, DateTime fechaNacimiento, string telefono, string email, string direccion)
            {
                IdCliente = id;
                Nombre = nombre;
                ApellidoPaterno = apellidoPaterno;
                ApellidoMaterno = apellidoMaterno;
                CURP = curp;
                FechaNacimiento = fechaNacimiento;
                Telefono = telefono;
                Email = email;
                Direccion = direccion;
                FechaRegistro = DateTime.Now;
                Activo = true;
            }

            // Propiedad calculada: Nombre completo
            public string NombreCompleto
            {
                get { return string.Format("{0} {1} {2}", Nombre, ApellidoPaterno, ApellidoMaterno); }
            }

            // Propiedad calculada: Edad
            public int Edad
            {
                get
                {
                    var hoy = DateTime.Today;
                    var edad = hoy.Year - FechaNacimiento.Year;
                    if (FechaNacimiento.Date > hoy.AddYears(-edad)) edad--;
                    return edad;
                }
            }


            /// Muestra la información del cliente

            public void MostrarInformacion()
            {
                Console.WriteLine("\n╔══════════════════════════════════════════════════════════════╗");
                Console.WriteLine("  ID Cliente: {0}", IdCliente);
                Console.WriteLine("  Nombre: {0}", NombreCompleto);
                Console.WriteLine("  CURP: {0}", CURP);
                Console.WriteLine("  Edad: {0} años", Edad);
                Console.WriteLine("  Teléfono: {0}", Telefono);
                Console.WriteLine("  Email: {0}", Email);
                Console.WriteLine("  Dirección: {0}", Direccion);
                Console.WriteLine("  Fecha de Registro: {0:dd/MM/yyyy}", FechaRegistro);

                Console.ForegroundColor = Activo ? ConsoleColor.Green : ConsoleColor.Red;
                Console.WriteLine("  Estado: {0}", Activo ? "ACTIVO ✓" : "INACTIVO ✗");
                Console.ResetColor();

                Console.WriteLine("╚══════════════════════════════════════════════════════════════╝");
            }

            /// Serializa el cliente para guardar en archivo

            public override string ToString()
            {
                return string.Format("{0}|{1}|{2}|{3}|{4}|{5:yyyy-MM-dd}|{6}|{7}|{8}|{9:yyyy-MM-dd}|{10}",
                       IdCliente, Nombre, ApellidoPaterno, ApellidoMaterno, CURP,
                       FechaNacimiento, Telefono, Email, Direccion, FechaRegistro, Activo);
            }
        }
    }

