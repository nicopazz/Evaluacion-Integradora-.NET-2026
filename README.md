# Evaluaci-n-Integradora-.NET-2026


EVALUACIÓN INTEGRADORA
Desarrollo .NET 2026 — C# con .NET 10
Programación Orientada a Objetos · LINQ · Records
Alumno/a	Paz Malizia, Christian Nicolas
Dominio asignado	Clínica Veterinaria
Contenido	Problema 1 + Problema 2
Modalidad	Domiciliario individual — plazo: 1 semana

⚠  INSTRUCCIONES GENERALES
• El código debe compilar sin errores. Código que no compila equivale a 0 puntos en ese problema.
• Respetar las convenciones de nombres de C# en clases, propiedades y métodos.
• Aplicar correctamente los modificadores de acceso (private, protected, public).
• Los problemas se entregan mediante repositorio GitHub o archivo .zip.
 
PROBLEMA 1  Clínica Veterinaria — Sistema de Gestión
POO · Colecciones · LINQ   |   25 puntos   |   Domiciliario individual

Contexto del problema
Situación real a resolver
La la clínica veterinaria "Huella Feliz" necesita un sistema para gestionar sus registros y operaciones diarias. Actualmente todo se maneja en papel y el equipo pierde tiempo buscando información y calculando totales. Tu tarea es construir ese sistema en C# aplicando programación orientada a objetos.
El programa se ejecuta desde el método Main sin menú interactivo. Los datos están definidos exactamente en este enunciado y deben cargarse tal como se indica. Antes de cada bloque imprimí su encabezado:
Console.WriteLine("=== PASO 1: CARGA DE REGISTROS ===");

Parte A — Modelo de clases  (10 puntos)
Clase abstracta Animal
•	Campos privados: _nombre (string) e _idAnimal (string).
•	Propiedad pública Nombre: solo lectura (get).
•	Propiedad pública IdAnimal: solo lectura (get).
•	Propiedad pública Categoria con getter y setter. El setter lanza ArgumentException con "La categoría no puede estar vacía." si el valor es nulo o vacío.
•	Propiedad pública int Anio con getter y setter.
•	Constructor que reciba id, nombre, categoria y anio.
•	Método abstracto decimal CalcularCostoBase().
•	Método virtual string ObtenerFicha(): retorna "[Id] | Nombre | Categoria | Año: Anio".

Clase Perro : Animal
•	Propiedades encapsuladas: Raza y PesoKg (string, double).
•	Constructor que llame a base y asigne las propiedades propias.
•	Override de CalcularCostoBase(): retorna 3500 + (decimal)(PesoKg * 100). Justificá en un comentario: "Costo base: tarifa fija $3500 + $100 por kg de peso".
•	Override de ObtenerFicha(): llama a base.ObtenerFicha() y agrega los datos propios.
•	Método void AccionPropia(): imprime un mensaje representativo de este tipo.

Clase Gato : Animal
•	Propiedades encapsuladas: EsCastrado y ColorPelaje (bool, string).
•	Constructor que llame a base y asigne las propiedades propias.
•	Override de CalcularCostoBase(): retorna EsCastrado ? 2800m : 3200m. Justificá en un comentario: "Castrado: menor riesgo → menor tarifa base".
•	Override de ObtenerFicha(): llama a base.ObtenerFicha() y agrega los datos propios.

Interfaz IVacunable
•	Propiedad de solo lectura: bool EstaVacunado { get; }.
•	Método: void AplicarVacuna(string detalle).
•	Método: void RegistrarRefuerzo().
•	Método por defecto: string ObtenerEstado() → retorna "Al día" si EstaVacunado es true, "Pendiente" si es false.
Implementá IVacunable en la clase Perro.

Record Consulta
public record Consulta(
    string IdConsulta,
    string IdPaciente,
    string NombreVeterinario,
    string Motivo,
    decimal Costo,
    DateTime Fecha
);

Parte B — Carga y operaciones  (9 puntos)
Ejecutá cada paso en el Main en el orden indicado, con su encabezado antes de cada uno.

Paso 1 — Cargar los siguientes 6 registros en una List<Animal> llamada registros
ID	Nombre	Tipo	Dato 1	Dato 2	Dato 3
P001	Rocky	Perro	4 años	Labrador	32 kg
P002	Luna	Perra	2 años	Beagle	10 kg
P003	Michi	Gato	6 años	Pelaje gris	Castrado: Sí
P004	Simba	Gato	3 años	Pelaje naranja	Castrado: No
P005	Rex	Perro	7 años	Pastor Alemán	40 kg
P006	Manchita	Perra	1 año	Dálmata	8 kg
Después de cargarlos, imprimí la ficha de cada registro llamando a ObtenerFicha().

Paso 2 — Cargar los siguientes 8 registros en una List<Consulta> llamada registros2
IdConsulta	IdPaciente	NombreVeterinario	Motivo	Costo	Fecha
C001	P001	Dra. García	Control anual	4500	01/04/2026
C002	P001	Dr. Pérez	Vacuna antirrábica	3200	15/04/2026
C003	P002	Dra. García	Desparasitación	2800	10/04/2026
C004	P003	Dr. Martínez	Revisión dental	6500	20/03/2026
C005	P004	Dr. Martínez	Control de peso	2100	05/04/2026
C006	P005	Dr. Pérez	Cirugía menor	12000	22/04/2026
C007	P006	Dra. García	Primera consulta	3500	25/04/2026
C008	P003	Dr. Pérez	Seguimiento post-op	4000	18/02/2026

Paso 3 — Agregar un nuevo registro
Creá y agregá a la lista registros el siguiente elemento:
ID: P007 | Nombre: Thor | Tipo: Perro | Golden Retriever | 28 kg
Imprimí exactamente: "✔ Thor agregado exitosamente." y a continuación su ficha usando ObtenerFicha().

Paso 4 — Eliminar un registro
Eliminá de la lista registros al elemento con Nombre exactamente igual a "Simba". Imprimí "✔ Simba eliminado del sistema."
Luego intentá eliminar a "Kira" (que no existe). Imprimí "✘ No se encontró ningún registro con el nombre Kira."

Paso 5 — Recorrido polimórfico
Recorré la lista registros con un foreach. Antes escribí exactamente este comentario:
// POLIMORFISMO: la lista es de tipo Animal, pero en tiempo de ejecución
// .NET invoca el ObtenerFicha() real de cada objeto (Perro o Gato).
Dentro del foreach: llamá a ObtenerFicha() en cada elemento. Si el registro es de tipo Perro (usá is con pattern matching), llamá también a su AccionPropia().

Paso 6 — Usar IVacunable
Buscá en la lista al elemento con Nombre "Rocky" y referencilo como IVacunable. Ejecutá en orden:
•	AplicarVacuna("prueba") → imprimí "✔ AplicarVacuna aplicado a Rocky."
•	RegistrarRefuerzo() → imprimí "✔ RegistrarRefuerzo ejecutado para Rocky."
•	ObtenerEstado() → imprimí "Estado de Rocky: [resultado]"

Parte C — Consultas LINQ  (6 puntos)
Resolvé exactamente las siguientes 4 consultas con LINQ. Cada una debe tener su encabezado en consola.

Consulta 1 — pacientes de mayor a menor edad
Usá OrderByDescending. Para cada elemento imprimí: "[Nombre] — [dato]". Verificá el orden esperado según los datos cargados.

Consulta 2 — consultas de "Dra. García" en abril de 2026
Filtrá por NombreVeterinario == "Dra. García" && Fecha.Month == 4 && Fecha.Year == 2026. Para cada resultado imprimí: "[Motivo/Tipo] | ID: [id] | Importe: $[valor]". Deben aparecer exactamente C001, C003 y C007.

Consulta 3 — total gastado por paciente
Usá GroupBy por Id del elemento principal. Para cada grupo calculá la suma del importe y cruzá con la lista registros para obtener el Nombre. Ordená de mayor a menor e imprimí: "[Nombre]: $[Total]".

Consulta 4 — Estadísticas generales
Mostrá exactamente los siguientes 5 datos calculados con LINQ:
•	"Total de pacientes registrados: [valor]"
•	"Cantidad de perros: [valor]"
•	"Cantidad de gatos: [valor]"
•	"Costo promedio de consultas: [valor]"
•	"Consulta más cara: [valor]"
 
PROBLEMA 2  Clínica Veterinaria — Estructuras repetitivas y recursividad
Extiende el Problema 1   |   25 puntos   |   Domiciliario individual

Contexto del problema
Situación real a resolver
El sistema del Problema 1 necesita procesar datos con estrategias avanzadas: recorrer colecciones con distintos bucles, generar reportes acumulados y buscar elementos usando algoritmos recursivos. Partís del mismo modelo de clases y los mismos datos del Problema 1.
Importante: usá exactamente las mismas clases y datos del Problema 1. Simba/el elemento eliminado ya no está en la lista.

Parte A — Estructuras repetitivas  (10 puntos)
Tarea 1 — Generar el historial completo de "Rocky"  (3 puntos)
Recorré la lista registros2 con un bucle for (no foreach) y acumulá en una nueva lista todos los registros cuyo IdPrincipal sea "P001". Luego recorré esa lista con un foreach e imprimí cada registro con el formato:
"[Id] | [Fecha dd/MM/yyyy] | [Tipo/Motivo] | Responsable: [nombre] | $[importe]"
Al final imprimí: "Total acumulado de Rocky: $[suma]". El resultado esperado es $7.700 (C001 $4.500 + C002 $3.200).

Tarea 2 — Tabla de costos base de todos los registros  (3 puntos)
Recorré la lista registros con un bucle while usando un índice entero. Para cada elemento llamá a CalcularCostoBase() e imprimí:
"[Nombre] ([Categoria]) → Costo base: $[resultado]"
Resultados esperados:
Nombre	Costo base esperado
Rocky	$6.700
Luna	$4.500
Michi	$2.800
Rex	$7.500
Manchita	$4.300
Thor	$6.300

Tarea 3 — Reporte acumulado por responsable  (4 puntos)
Usando do-while, recorré los responsables en el orden indicado. Para cada uno, usá un for interno que acumule el total de sus registros. Imprimí:
"=== REPORTE POR RESPONSABLE ==="
"Dra. García → [cantidad] registros | Total: $[suma]"
"Dr. Pérez → [cantidad] registros | Total: $[suma]"
"Dr. Martínez → [cantidad] registros | Total: $[suma]"
"─────────────────────────────"
"TOTAL GENERAL: $[suma de todos]"
Resultado esperado: Dra. García 3 consultas $10.800 | Dr. Pérez 3 consultas $19.200 | Dr. Martínez 2 consultas $8.600 | Total general: $38.600.

Parte B — Recursividad, Arrays y Matrices  (15 puntos)
Esta parte tiene tres ejercicios independientes. No se permite usar LINQ en ninguno. Todos usan los datos del Problema 1 (lista registros y lista registros2).

Ejercicio 1 — Método recursivo: BuscarAnimalPorId  (5 puntos)
Implementá el siguiente método estático recursivo en una clase estática. No está permitido usar bucles dentro del método.
Firma:
static Animal BuscarAnimalPorId(List<Animal> lista, string id, int indice = 0)
Comportamiento: si indice >= lista.Count retorna null. Si lista[indice].IdAnimal == id retorna ese elemento. Si no, llama recursivamente con indice + 1.
Casos de prueba en el Main:
•	BuscarAnimalPorId(registros, "P005") → debe encontrar a Rex e imprimir su ficha.
•	BuscarAnimalPorId(registros, "P999") → debe retornar null e imprimir "P999 no encontrado."

Ejercicio 2 — Array: costos totales por elemento  (5 puntos)
Construí un array decimal[] llamado costosPorAnimal donde cada posición i contiene el total acumulado de consultas del elemento registros[i]. Para construirlo usá bucles anidados, sin LINQ.
Luego, también sin LINQ, calculá e imprimí:
•	El elemento con mayor gasto total. Imprimí: "Mayor gasto: [Nombre] — $[monto]"
•	El elemento con menor gasto (solo los que tienen al menos una consulta). Imprimí: "Menor gasto: [Nombre] — $[monto]"
•	El promedio de gasto por elemento con consultas. Imprimí: "Promedio: $[promedio]"
Resultados esperados con los datos del enunciado:
Rocky: $6.700
Luna: $4.500
Michi: $2.800
Rex: $7.500
Manchita: $4.300
Thor: $6.300

Ejercicio 3 — Matriz: animals × responsables  (5 puntos)
Construí una matriz decimal[,] de dimensiones [cantidadRegistros × cantidadResponsables] donde:
•	Las filas representan los elementos de la lista registros (en orden).
•	Las columnas representan los 3 responsables: columna 0 = Dra. García, columna 1 = Dr. Pérez, columna 2 = Dr. Martínez.
•	Cada celda [i, j] contiene el total acumulado del elemento i con el responsable j. Celdas sin registros valen 0.
Para llenar la matriz usá bucles anidados (triple for), sin LINQ. Luego imprimí la tabla con encabezados y calculá:
•	Total recaudado por cada responsable (suma de su columna). Resultado esperado: Dra. García 3 consultas $10.800 | Dr. Pérez 3 consultas $19.200 | Dr. Martínez 2 consultas $8.600.
•	El responsable con mayor recaudación. Resultado esperado: Dra. García.
