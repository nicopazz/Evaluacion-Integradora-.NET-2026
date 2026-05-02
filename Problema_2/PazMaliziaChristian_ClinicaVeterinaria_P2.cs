using System;
using System.Collections.Generic;


/* ==== MODELO DE CLASES ===== */


public interface IVacunable
{
    bool EstaVacunado { get; }
    void AplicarVacuna(string detalle);
    void RegistrarRefuerzo();
    string ObtenerEstado() => EstaVacunado ? "Al día" : "Pendiente";
}

public abstract class Animal
{
    private string _nombre;
    private string _idAnimal;
    private string _categoria;

    public string Nombre   => _nombre;
    public string IdAnimal => _idAnimal;
    public int Anio { get; set; }

    public string Categoria
    {
        get => _categoria;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("La categoría no puede estar vacía.");
            _categoria = value;
        }
    }

    protected Animal(string id, string nombre, string categoria, int anio)
    {
        _idAnimal = id;
        _nombre = nombre;
        Categoria = categoria;
        Anio = anio;
    }

    public abstract decimal CalcularCostoBase();

    public virtual string ObtenerFicha()
        => $"[{IdAnimal}] | {Nombre} | {Categoria} | Año: {Anio}";
}

public class Perro : Animal, IVacunable
{
    public string Raza { get; private set; }
    public double PesoKg { get; private set; }
    private bool _estaVacunado = false;
    public bool EstaVacunado => _estaVacunado;

    public Perro(string id, string nombre, string categoria, int anio, string raza, double pesoKg)
        : base(id, nombre, categoria, anio)
    {
        Raza = raza;
        PesoKg = pesoKg;
    }

    public override decimal CalcularCostoBase() => 3500m + (decimal)(PesoKg * 100);
    public override string ObtenerFicha() => $"{base.ObtenerFicha()} | Raza: {Raza} | Peso: {PesoKg} kg";
    
    public void AccionPropia() => Console.WriteLine(" Moviendo la cola :D ");
    public void AplicarVacuna(string detalle) => _estaVacunado = true;
    public void RegistrarRefuerzo() => _estaVacunado = true;
}

public class Gato : Animal
{
    public bool EsCastrado { get; private set; }
    public string ColorPelaje { get; private set; }

    public Gato(string id, string nombre, string categoria, int anio, bool esCastrado, string colorPelaje)
        : base(id, nombre, categoria, anio)
    {
        EsCastrado = esCastrado;
        ColorPelaje = colorPelaje;
    }

    public override decimal CalcularCostoBase() => EsCastrado ? 2800m : 3200m;
    public override string ObtenerFicha() => $"{base.ObtenerFicha()} | Color: {ColorPelaje} | Castrado: {(EsCastrado ? "Sí" : "No")}";
}

public record Consulta(string IdConsulta, string IdPaciente, string NombreVeterinario, string Motivo, decimal Costo, DateTime Fecha);


/* EJERCICIO 1: CLASE ESTÁTICA PARA EL MÉTODO RECURSIVO */

public static class VeterinariaUtils
{
    // Método recursivo — sin usar bucles
    public static Animal BuscarAnimalPorId(List<Animal> lista, string id, int indice = 0)
    {
        if (indice >= lista.Count) return null;
        if (lista[indice].IdAnimal == id) return lista[indice];
        return BuscarAnimalPorId(lista, id, indice + 1);
    }
}


/* PROGRAMA PRINCIPAL (Problema 2 - Estructuras y Recursividad) */
class Program
{
    static void Main()
    {
        // ── Datos actualizados (Simba eliminado, Thor agregado) ─────────────────
        var registros = new List<Animal>
        {
            new Perro("P001", "Rocky",    "Perro", 4, "Labrador",       32),
            new Perro("P002", "Luna",     "Perra", 2, "Beagle",         10),
            new Gato( "P003", "Michi",    "Gato",  6, true,             "Pelaje gris"),
            new Perro("P005", "Rex",      "Perro", 7, "Pastor Alemán",  40),
            new Perro("P006", "Manchita", "Perra", 1, "Dálmata",        8),
            new Perro("P007", "Thor",     "Perro", 0, "Golden Retriever",28)
        };

        var registros2 = new List<Consulta>
        {
            new("C001", "P001", "Dra. García",  "Control anual",       4500m, new DateTime(2026,4,1)),
            new("C002", "P001", "Dr. Pérez",    "Vacuna antirrábica",  3200m, new DateTime(2026,4,15)),
            new("C003", "P002", "Dra. García",  "Desparasitación",     2800m, new DateTime(2026,4,10)),
            new("C004", "P003", "Dr. Martínez", "Revisión dental",     6500m, new DateTime(2026,3,20)),
            new("C005", "P004", "Dr. Martínez", "Control de peso",     2100m, new DateTime(2026,4,5)),
            new("C006", "P005", "Dr. Pérez",    "Cirugía menor",      12000m, new DateTime(2026,4,22)),
            new("C007", "P006", "Dra. García",  "Primera consulta",    3500m, new DateTime(2026,4,25)),
            new("C008", "P003", "Dr. Pérez",    "Seguimiento post-op", 4000m, new DateTime(2026,2,18))
        };

        string[] responsables = { "Dra. García", "Dr. Pérez", "Dr. Martínez" };

        // ── PARTE A — TAREA 1: Generar historial con for + foreach ──────────────
        Console.WriteLine("=== TAREA 1: HISTORIAL COMPLETO DE ROCKY ===");
        var historialRocky = new List<Consulta>();
        for (int i = 0; i < registros2.Count; i++)
        {
            if (registros2[i].IdPaciente == "P001")
                historialRocky.Add(registros2[i]);
        }
        
        decimal sumaRocky = 0;
        foreach (var c in historialRocky)
        {
            Console.WriteLine($"[{c.IdConsulta}] | {c.Fecha:dd/MM/yyyy} | {c.Motivo} | Responsable: {c.NombreVeterinario} ${c.Costo}");
            sumaRocky += c.Costo;
        }
        Console.WriteLine($"Total acumulado de Rocky: ${sumaRocky}");

        // ── PARTE A — TAREA 2: Costo base con while ─────────────────────────────
        Console.WriteLine("\n=== TAREA 2: COSTOS BASE DE TODOS (while) ===");
        int idx = 0;
        while (idx < registros.Count)
        {
            var a = registros[idx];
            Console.WriteLine($"{a.Nombre} ({a.Categoria}) \t Costo base: ${a.CalcularCostoBase()}");
            idx++;
        }

        // ── PARTE A — TAREA 3: Reporte acumulado con do-while + for interno ─────
        Console.WriteLine("\n=== TAREA 3: REPORTE POR RESPONSABLE (do-while) ===");
        Console.WriteLine("=== REPORTE POR RESPONSABLE ===");
        decimal totalGeneral = 0;
        int v = 0;
        do
        {
            decimal totalResp = 0; 
            int cantRegistros = 0;
            
            for (int j = 0; j < registros2.Count; j++)
            {
                if (registros2[j].NombreVeterinario == responsables[v])
                {
                    totalResp += registros2[j].Costo;
                    cantRegistros++;
                }
            }
            
            Console.WriteLine($"{responsables[v]}");
            Console.WriteLine($"{cantRegistros} registros");
            Console.WriteLine($"Total: ${totalResp}\n");
            
            totalGeneral += totalResp;
            v++;
        } while (v < responsables.Length);
        Console.WriteLine($"TOTAL GENERAL: ${totalGeneral}");

        // ── PARTE B — EJERCICIO 1: Método Recursivo ─────────────────────────────
        Console.WriteLine("=== EJERCICIO 1: MÉTODO RECURSIVO ===");
        var rex = VeterinariaUtils.BuscarAnimalPorId(registros, "P005");
        if (rex != null) Console.WriteLine(rex.ObtenerFicha());
        
        var p999 = VeterinariaUtils.BuscarAnimalPorId(registros, "P999");
        if (p999 == null) Console.WriteLine("P999 no encontrado.");

        // ── PARTE B — EJERCICIO 2: Array de costos ──────────────────────────────
        Console.WriteLine("\n=== EJERCICIO 2: ARRAY DE COSTOS POR ELEMENTO ===");
        decimal[] costosPorAnimal = new decimal[registros.Count];
        
        for (int i = 0; i < registros.Count; i++)
        {
            for (int j = 0; j < registros2.Count; j++)
            {
                if (registros2[j].IdPaciente == registros[i].IdAnimal)
                    costosPorAnimal[i] += registros2[j].Costo;
            }
            Console.WriteLine($"{registros[i].Nombre}: ${costosPorAnimal[i]}");
        }

        int iMax = 0;
        for (int i = 1; i < costosPorAnimal.Length; i++)
            if (costosPorAnimal[i] > costosPorAnimal[iMax]) iMax = i;
        Console.WriteLine($"\nMayor gasto: {registros[iMax].Nombre} - ${costosPorAnimal[iMax]}");

        int iMin = -1;
        for (int i = 0; i < costosPorAnimal.Length; i++)
        {
            if (costosPorAnimal[i] > 0 && (iMin == -1 || costosPorAnimal[i] < costosPorAnimal[iMin]))
                iMin = i;
        }
        if (iMin >= 0)
            Console.WriteLine($"Menor gasto: {registros[iMin].Nombre} - ${costosPorAnimal[iMin]}");

        decimal sumaArray = 0; 
        int contArray = 0;
        for (int i = 0; i < costosPorAnimal.Length; i++)
        {
            if (costosPorAnimal[i] > 0) 
            { 
                sumaArray += costosPorAnimal[i]; 
                contArray++; 
            }
        }
        if (contArray > 0) Console.WriteLine($"Promedio: ${sumaArray / contArray:F2}");

        // ── PARTE B — EJERCICIO 3: Matriz ───────────────────────────────────────
        Console.WriteLine("\n=== EJERCICIO 3: MATRIZ (Pacientes x Responsables) ===");
        decimal[,] matriz = new decimal[registros.Count, responsables.Length];
        
        for (int i = 0; i < registros.Count; i++)
        {
            for (int j = 0; j < responsables.Length; j++)
            {
                for (int k = 0; k < registros2.Count; k++)
                {
                    if (registros2[k].IdPaciente == registros[i].IdAnimal &&
                        registros2[k].NombreVeterinario == responsables[j])
                    {
                        matriz[i, j] += registros2[k].Costo;
                    }
                }
            }
        }

        Console.Write($"{"Paciente",-15}");
        for (int j = 0; j < responsables.Length; j++) Console.Write($"{responsables[j],-15}");
        Console.WriteLine();
        
        for (int i = 0; i < registros.Count; i++)
        {
            Console.Write($"{registros[i].Nombre,-15}");
            for (int j = 0; j < responsables.Length; j++)
            {
                Console.Write($"${matriz[i, j],-14}");
            }
            Console.WriteLine();
        }

        Console.WriteLine("\nRecaudación por responsable:");
        int jMaxRecauda = 0; 
        decimal maxRecaudacion = 0;
        
        for (int j = 0; j < responsables.Length; j++)
        {
            decimal columnaSuma = 0;
            int cantConsultas = 0; // Recalculamos cantidad para formato idéntico al solicitado
            
            for (int k = 0; k < registros2.Count; k++)
            {
                if (registros2[k].NombreVeterinario == responsables[j]) cantConsultas++;
            }

            for (int i = 0; i < registros.Count; i++)
            {
                columnaSuma += matriz[i, j];
            }
            
            Console.WriteLine($"{responsables[j]} {cantConsultas} consultas ${columnaSuma}");
            
            if (columnaSuma > maxRecaudacion) 
            { 
                maxRecaudacion = columnaSuma; 
                jMaxRecauda = j; 
            }
        }
        Console.WriteLine($"\nEl responsable con mayor recaudación: {responsables[jMaxRecauda]}");
    }
}