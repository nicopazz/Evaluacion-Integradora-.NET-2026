using System;
using System.Collections.Generic;
using System.Linq;


/* ==== PARTE A - MODELO DE CLASES ===== */

public interface IVacunable
{
    bool EstaVacunado { get; }
    void AplicarVacuna(string detalle);
    void RegistrarRefuerzo();

    // Método por defecto
    string ObtenerEstado()
        => EstaVacunado ? "Al día" : "Pendiente";
}

public abstract class Animal
{
    private string _nombre;
    private string _idAnimal;
    private string _categoria;

    public string Nombre   => _nombre;
    public string IdAnimal => _idAnimal;

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

    public int Anio { get; set; }

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

    // Costo base: tarifa fija $3500 + $100 por kg de peso
    public override decimal CalcularCostoBase()
        => 3500m + (decimal)(PesoKg * 100);

    public override string ObtenerFicha()
        => $"{base.ObtenerFicha()} | Raza: {Raza} | Peso: {PesoKg} kg";

    public void AccionPropia()
    {
        Console.WriteLine(" Moviendo la cola :D ");
    }

    public void AplicarVacuna(string detalle)
    {
        _estaVacunado = true;
    }

    public void RegistrarRefuerzo()
    {
        _estaVacunado = true;
    }
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

    // Castrado: menor riesgo --> menor tarifa base
    public override decimal CalcularCostoBase()
        => EsCastrado ? 2800m : 3200m;

    public override string ObtenerFicha()
        => $"{base.ObtenerFicha()} | Color: {ColorPelaje} | Castrado: {(EsCastrado ? "Sí" : "No")}";
}

public record Consulta(
    string   IdConsulta,
    string   IdPaciente,
    string   NombreVeterinario,
    string   Motivo,
    decimal  Costo,
    DateTime Fecha
);

/* ==== PROGRAMA PRINCIPAL (Partes B y C)   ===== */

class Program
{
    static void Main()
    {
        /* ==== PARTE B - CARGA Y OPERACIONES ===== */

        Console.WriteLine(" PASO 1: CARGA DE REGISTROS  ");
        var registros = new List<Animal>
        {
            new Perro("P001", "Rocky",    "Perro", 4, "Labrador",       32),
            new Perro("P002", "Luna",     "Perra", 2, "Beagle",         10),
            new Gato( "P003", "Michi",    "Gato",  6, true,             "Pelaje gris"),
            new Gato( "P004", "Simba",    "Gato",  3, false,            "Pelaje naranja"),
            new Perro("P005", "Rex",      "Perro", 7, "Pastor Alemán",  40),
            new Perro("P006", "Manchita", "Perra", 1, "Dálmata",        8)
        };

        foreach (var r in registros)
            Console.WriteLine(r.ObtenerFicha());

        Console.WriteLine("\n PASO 2: CARGA DE CONSULTAS  ");
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

        Console.WriteLine("\n PASO 3: AGREGAR UN NUEVO REGISTRO  ");
        var thor = new Perro("P007", "Thor", "Perro", 0, "Golden Retriever", 28);
        registros.Add(thor);
        Console.WriteLine(" Thor agregado exitosamente.");
        Console.WriteLine(thor.ObtenerFicha());

        Console.WriteLine("\n PASO 4: ELIMINAR UN REGISTRO  ");
        var simba = registros.FirstOrDefault(r => r.Nombre == "Simba");
        if (simba != null)
        {
            registros.Remove(simba);
            Console.WriteLine("✓ Simba eliminado del sistema.");
        }

        var kira = registros.FirstOrDefault(r => r.Nombre == "Kira");
        if (kira != null) registros.Remove(kira);
        else Console.WriteLine("X No se encontró ningún registro con el nombre Kira.");

        Console.WriteLine("\n PASO 5: RECORRIDO POLIMÓRFICO  ");
        // POLIMORFISMO: la lista es de tipo Animal, pero en tiempo de ejecución
        // .NET invoca el ObtenerFicha() real de cada objeto (Perro o Gato).
        foreach (var r in registros)
        {
            Console.WriteLine(r.ObtenerFicha());
            if (r is Perro p)
                p.AccionPropia();
        }

        Console.WriteLine("\n PASO 6: USAR IVACUNABLE  ");
        var rocky = registros.FirstOrDefault(r => r.Nombre == "Rocky");
        if (rocky is IVacunable vac)
        {
            vac.AplicarVacuna("prueba");
            Console.WriteLine(" AplicarVacuna aplicado a Rocky.");
            vac.RegistrarRefuerzo();
            Console.WriteLine(" RegistrarRefuerzo ejecutado para Rocky.");
            Console.WriteLine($"Estado de Rocky: {vac.ObtenerEstado()}");
        }

        /* ==== PARTE C - CONSULTAS LINQ ===== */

        Console.WriteLine("\n Consulta 1 ");
        foreach (var p in registros.OrderByDescending(r => r.Anio))
            Console.WriteLine($"{p.Nombre} - {p.Anio} años");

        Console.WriteLine("\n Consulta 2 ");
        var q2 = registros2.Where(c => c.NombreVeterinario == "Dra. García" 
                                    && c.Fecha.Month == 4 
                                    && c.Fecha.Year == 2026);
        foreach (var c in q2)
            Console.WriteLine($"{c.Motivo} | ID: {c.IdConsulta} | Importe: ${c.Costo}");

        Console.WriteLine("\n Consulta 3 ");
        foreach (var x in registros2
            .GroupBy(c => c.IdPaciente)
            .Select(g => new { 
                Id = g.Key, 
                Total = g.Sum(c => c.Costo),
                Nombre = registros.FirstOrDefault(r => r.IdAnimal == g.Key)?.Nombre ?? "Desconocido"
            })
            .OrderByDescending(x => x.Total))
            Console.WriteLine($"{x.Nombre}: ${x.Total}");

        Console.WriteLine("\n Consulta 4 ");
        Console.WriteLine($"Total de pacientes registrados: {registros.Count}");
        Console.WriteLine($"Cantidad de perros: {registros.OfType<Perro>().Count()}");
        Console.WriteLine($"Cantidad de gatos: {registros.OfType<Gato>().Count()}");
        Console.WriteLine($"Costo promedio de consultas: {registros2.Average(c => c.Costo)}");
        Console.WriteLine($"Consulta más cara: {registros2.Max(c => c.Costo)}");
    }
}