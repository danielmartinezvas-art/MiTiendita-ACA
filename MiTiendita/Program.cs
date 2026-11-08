namespace MiTiendita;

internal static class Program
{
    /// <summary>
    /// Punto de entrada de la aplicacion. No necesitas modificar este archivo.
    /// </summary>
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new FormPrincipal());
    }
}
