using System;
using static System.Console;
using System.IO;

namespace DataBits
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
                Environment.Exit(0);

            while (true)
            {
                FileExists(args[0]);

                WriteLine("\n[1] = Capturar\n[2] = Listar\n[3] = Buscar\n[4] = Editar\n[5] = Eliminar\n[6] = Salir");
                int opt = int.Parse(ReadLine());

                switch (opt)
                {
                    case 1:
                        Consultar(args[0]);
                        break;
                    case 2:
                        Listar(args[0]);
                        break;
                    case 3:
                        string finded = Busqueda(args[0], out string ced);
                        break;
                    case 4:
                        Editar(args[0]);
                        break;
                    case 5:
                        Eliminar(args[0]);
                        break;
                    case 6:
                        Environment.Exit(0);
                        break;
                }
            }
        }

        #region Data en Bits
        static int ToBits(int edad, char genero, char estado, char grado)
        {
            int datos = edad << 4;

            if (genero == 'F') datos = datos | 8;

            if (estado == 'C') datos = datos | 4;

            if (grado == 'M') datos = datos | 1;
            else if (grado == 'G') datos = datos | 2;
            else if (grado == 'P') datos = datos | 3;

            return datos;
        }

        static void ReadBits(int datos, out int edad, out char genero, out char estado, out char grado)
        {
            edad = datos >> 4;

            datos = datos & 15;
            if ((datos >> 3) == 1) genero = 'F';
            else genero = 'M';

            datos = datos & 7;
            if ((datos >> 2) == 1) estado = 'C';
            else estado = 'S';

            datos = datos & 3;
            if (datos == 0) grado = 'I';
            else if (datos == 1) grado = 'M';
            else if (datos == 2) grado = 'G';
            else grado = 'P';
        }

        static char ReadChar(string text)
        {
            Write(text);
            string value = "";
            ConsoleKey key;

            do
            {
                var keyInfo = ReadKey(intercept: true);
                key = keyInfo.Key;

                if (key == ConsoleKey.Backspace && value.Length > 0)
                {
                    Write("\b \b");
                    value = value.Remove(value.Length - 1);
                }
                else if (!char.IsControl(keyInfo.KeyChar) && value.Length < 1)
                {
                    Write(keyInfo.KeyChar);
                    value += keyInfo.KeyChar;
                }
            } while (key != ConsoleKey.Enter);

            return Convert.ToChar(value);
        }
        #endregion

        #region Registro de Datos v1
        static void FileExists(string path)
        {
            if (File.Exists(path) == false)
            {
                var creator = File.Create(path);
                creator.Close();

                StreamWriter builder = File.AppendText(path);
                builder.WriteLine("Cedula;Nombres;Apellidos;Datos;Ahorros;Contraseña");
                builder.Close();
            }
        }

        static bool CedExists(string path, string ced)
        {
            StreamReader reader = new StreamReader(path);
            string[] data = reader.ReadToEnd().Split(Environment.NewLine);
            reader.Close();

            foreach (var i in data)
            {
                if (i.Contains(ced + ";"))
                {
                    return true;
                }
            }
            return false;
        }

        static void Consultar(string path)
        {
            while (true)
            {
                string ced = ReadCedula("\nCedula: ");
                Write("\nNombre: ");
                string name = ReadLine();
                Write("Apellidos: ");
                string ape = ReadLine();

                if (name == "" && ape == "")
                    break;

                int age = ReadAge("Edad (7 - 120): "); ;
                while (age < 7 || age > 120)
                {
                    age = ReadAge("\nEdad (7 - 120): ");
                }

                char gender, state, grade;
                do
                {
                    gender = ReadChar("\nGénero (M/F): ");
                } while (gender != 'M' && gender != 'F');

                do
                {
                    state = ReadChar("\nEstado Civil (S/C): ");
                } while (state != 'S' && state != 'C');

                do
                {
                    grade = ReadChar("\nGrado Académico (I/M/G/P): ");
                } while (grade != 'I' && grade != 'M' && grade != 'G' && grade != 'P');

                decimal ahorros = ReadMoney("\nAhorros: ");
                string password = ReadPassword("\nContraseña: ");

                bool success = password == ReadPassword("\nConfirme contraseña: ");

                int datos = ToBits(age, gender, state, grade);

                if (CedExists(path, ced))
                {
                    WriteLine("\nLa cédula ya existe, intente otra vez!!");
                    continue;
                }
                else if (success == false)
                {
                    WriteLine("\nLas contraseñas no son idénticas!!");
                }
                else
                {
                    while (true)
                    {
                        WriteLine("\n\nGuardar(G) | Rehacer(R) | Salir(S)");
                        string opt = ReadLine();

                        if (opt == "G" || opt == "g")
                        {
                            StreamWriter writer = File.AppendText(path);
                            writer.WriteLine($"{ced};{name};{ape};{datos};{ahorros};{password}");
                            writer.Close();
                            break;
                        }
                        else if (opt == "R" || opt == "r")
                            break;
                        else if (opt == "S" || opt == "s")
                            Environment.Exit(0);
                        else continue;
                    }
                }
            }
        }
        #endregion

        #region Registro de Datos v2
        static void Listar(string path)
        {
            StreamReader reader = new StreamReader(path);
            string[] data = reader.ReadToEnd().Split(Environment.NewLine);
            reader.Close();

            WriteLine();

            foreach (var i in data)
            {
                if (i.Contains("Cedula;"))
                {
                    WriteLine("Cedula;Nombre;Apellido;Edad;Género;Estado;Grado;Ahorros;Contraseña");
                    continue;
                }

                if (i != "")
                {
                    string[] tokens = i.Split(";");
                    ReadBits(int.Parse(tokens[3]), out int edad, out char genero, out char estado, out char grado);
                    Console.WriteLine($"{tokens[0]};{tokens[1]};{tokens[2]};{edad};{genero};{estado};{grado};{tokens[4]};{tokens[5]}");
                }
            }
        }

        static string Busqueda(string path, out string sCed)
        {
            string finded = "";

            Write("\nIntroduzca la cédula: ");
            sCed = ReadLine();

            StreamReader reader = new StreamReader(path);
            string[] data = reader.ReadToEnd().Split(Environment.NewLine);
            reader.Close();

            foreach (var i in data)
            {
                string value = i.Substring(0, i.IndexOf(";") + 1);
                if (value == sCed + ";" && value != "Cedula;")
                {
                    finded = i;
                    string[] tokens = i.Split(";");
                    ReadBits(int.Parse(tokens[3]), out int edad, out char genero, out char estado, out char grado);
                    WriteLine($"{tokens[0]};{tokens[1]};{tokens[2]};{edad};{genero};{estado};{grado};{tokens[4]};{tokens[5]}");
                }
            }

            if (finded == "")
                WriteLine("Cédula inexistente!!");

            return finded;
        }
        #endregion

        #region Registro de Datos v3
        static void Editar(string path)
        {
            string finded = Busqueda(path, out string sCed);

            while (true)
            {
                string ced = ReadCedula("\nCedula: ");
                Write("\nNombre: ");
                string name = ReadLine();
                Write("Apellidos: ");
                string ape = ReadLine();

                if (name == "" && ape == "")
                    break;

                int age = ReadAge("Edad (7 - 120): "); ;
                while (age < 7 || age > 120)
                {
                    age = ReadAge("\nEdad (7 - 120): ");
                }

                char gender, state, grade;
                do
                {
                    gender = ReadChar("\nGénero (M/F): ");
                } while (gender != 'M' && gender != 'F');

                do
                {
                    state = ReadChar("\nEstado Civil (S/C): ");
                } while (state != 'S' && state != 'C');

                do
                {
                    grade = ReadChar("\nGrado Académico (I/M/G/P): ");
                } while (grade != 'I' && grade != 'M' && grade != 'G' && grade != 'P');

                decimal ahorros = ReadMoney("\nAhorros: ");
                string password = ReadPassword("\nContraseña: ");

                bool success = password == ReadPassword("\nConfirme contraseña: ");

                if (!success)
                {
                    WriteLine("\nLas contraseñas no coinciden!!");
                    continue;
                }

                int datos = ToBits(age, gender, state, grade);

                if (ced == sCed)
                {
                    StreamReader reader = new StreamReader(path);
                    string[] data = reader.ReadToEnd().Split(Environment.NewLine);
                    reader.Close();

                    File.Delete(path);

                    foreach (var i in data)
                    {
                        string line = i;
                        if (i == finded)
                            line = $"{ced};{name};{ape};{datos};{ahorros};{password}";
                        else if (i == "")
                            continue;
                        StreamWriter writer = File.AppendText(path);
                        writer.WriteLine(line);
                        writer.Close();
                    }
                    break;
                }
                else if (CedExists(path, ced))
                    WriteLine("\nLa cédula ya existe, intente otra vez!!");
                else
                {
                    StreamReader reader = new StreamReader(path);
                    string[] data = reader.ReadToEnd().Split(Environment.NewLine);
                    reader.Close();

                    File.Delete(path);

                    foreach (var i in data)
                    {
                        string line = i;

                        if (i == finded)
                            line = $"{ced};{name};{ape};{datos};{ahorros};{password}";
                        else if (i == "")
                            continue;

                        StreamWriter writer = File.AppendText(path);
                        writer.WriteLine(line);
                        writer.Close();
                    }
                    break;
                }
            }
        }

        static void Eliminar(string path)
        {
            while (true)
            {
                string finded = Busqueda(path, out string sCed);

                if (sCed == "")
                    break;

                while (true)
                {
                    WriteLine("\n¿Desea eliminarlo? (Y/N)");
                    string res = ReadLine();

                    if (res == "Y")
                    {
                        StreamReader reader = new StreamReader(path);
                        string[] data = reader.ReadToEnd().Split(Environment.NewLine);
                        reader.Close();

                        File.Delete(path);

                        foreach (var i in data)
                        {
                            if (!i.Contains(finded) && i != "")
                            {
                                StreamWriter writer = File.AppendText(path);
                                writer.WriteLine(i);
                                writer.Close();
                            }
                        }
                        break;
                    }
                    else if (res == "N")
                        break;
                    else
                        WriteLine("No ha seleccionado opción alguna!!");
                    continue;
                }
            }
        }
        #endregion

        static string ReadPassword(string text)
        {
            Write(text);

            while (true)
            {
                string password = "";
                ConsoleKey key;

                do
                {
                    var keyInfo = ReadKey(intercept: true);
                    key = keyInfo.Key;

                    if (key == ConsoleKey.Backspace && password.Length > 0)
                    {
                        Write("\b \b");
                        password = password.Remove(password.Length - 1);
                    }
                    else if (!char.IsControl(keyInfo.KeyChar))
                    {
                        Write("*");
                        password += keyInfo.KeyChar;
                    }
                } while (key != ConsoleKey.Enter);

                if (password == "")
                    continue;

                return password;
            }
        }

        static string ReadCedula(string text)
        {
            Write(text);
            while (true)
            {
                string data = "";
                ConsoleKey key;

                do
                {
                    var keyInfo = ReadKey(intercept: true);
                    key = keyInfo.Key;

                    int value;
                    bool success = int.TryParse(keyInfo.KeyChar.ToString(), out value);

                    if (key == ConsoleKey.Backspace && data.Length > 0)
                    {
                        Write("\b \b");
                        data = data.Remove(data.Length - 1);
                    }
                    else if (!char.IsControl(keyInfo.KeyChar) && success)
                    {
                        Write(keyInfo.KeyChar);
                        data += keyInfo.KeyChar;
                    }

                } while (key != ConsoleKey.Enter);

                if (data == "")
                    continue;

                return data;
            }
        }

        static int ReadAge(string text)
        {
            Write(text);
            while (true)
            {
                string data = "";
                ConsoleKey key;

                do
                {
                    var keyInfo = ReadKey(intercept: true);
                    key = keyInfo.Key;

                    int value;
                    bool success = int.TryParse(keyInfo.KeyChar.ToString(), out value);

                    if (key == ConsoleKey.Backspace && data.Length > 0)
                    {
                        Write("\b \b");
                        data = data.Remove(data.Length - 1);
                    }
                    else if (!char.IsControl(keyInfo.KeyChar) && success)
                    {
                        Write(keyInfo.KeyChar);
                        data += keyInfo.KeyChar;
                    }
                } while (key != ConsoleKey.Enter);

                if (data == "")
                    continue;

                return int.Parse(data);
            }
        }

        static decimal ReadMoney(string text)
        {
            Write(text);
            while (true)
            {
                string data = "";
                ConsoleKey key;
                int c = 0;

                do
                {
                    var keyInfo = ReadKey(intercept: true);
                    key = keyInfo.Key;

                    int value;
                    bool success = int.TryParse(keyInfo.KeyChar.ToString(), out value) || (keyInfo.KeyChar == '.' && c == 0);

                    if (keyInfo.KeyChar == '.')
                        c++;

                    if (key == ConsoleKey.Backspace && data.Length > 0)
                    {
                        Write("\b \b");
                        data = data.Remove(data.Length - 1);
                    }
                    else if (!char.IsControl(keyInfo.KeyChar) && success)
                    {
                        Write(keyInfo.KeyChar);
                        data += keyInfo.KeyChar;
                    }
                } while (key != ConsoleKey.Enter);

                if (data == "")
                    continue;

                return Math.Round(decimal.Parse(data), 2);
            }
        }
    }
}
