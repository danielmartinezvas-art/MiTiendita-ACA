param(
    [string]$Exe,
    [string]$Salida,
    [string]$Repo
)

$ErrorActionPreference = "Stop"
$Exe = [System.IO.Path]::GetFullPath($Exe)
Add-Type -AssemblyName UIAutomationClient, UIAutomationTypes, System.Windows.Forms, System.Drawing

Add-Type @"
using System;
using System.Runtime.InteropServices;
public static class Raton {
    [DllImport("user32.dll")] public static extern bool SetCursorPos(int x, int y);
    [DllImport("user32.dll")] public static extern void mouse_event(uint f, uint x, uint y, uint d, UIntPtr e);
    [DllImport("user32.dll")] public static extern bool SetForegroundWindow(IntPtr h);
    [DllImport("user32.dll")] public static extern bool GetCursorPos(out POINT p);
    public struct POINT { public int X; public int Y; }
}
"@

New-Item -ItemType Directory -Force -Path $Salida | Out-Null
$UIA = [System.Windows.Automation.AutomationElement]
$TS = [System.Windows.Automation.TreeScope]
$PC = [System.Windows.Automation.PropertyCondition]

function Pausa([double]$s) { Start-Sleep -Milliseconds ([int]($s * 1000)) }

function Captura([string]$nombre) {
    $b = [System.Windows.Forms.Screen]::PrimaryScreen.Bounds
    $bmp = New-Object System.Drawing.Bitmap $b.Width, $b.Height
    $g = [System.Drawing.Graphics]::FromImage($bmp)
    $g.CopyFromScreen($b.Location, [System.Drawing.Point]::Empty, $b.Size)
    $bmp.Save((Join-Path $Salida "$nombre.png"), [System.Drawing.Imaging.ImageFormat]::Png)
    $g.Dispose(); $bmp.Dispose()
}

function Mover([int]$x, [int]$y) {
    $p = New-Object Raton+POINT
    [Raton]::GetCursorPos([ref]$p) | Out-Null
    $pasos = 18
    for ($i = 1; $i -le $pasos; $i++) {
        $nx = [int]($p.X + ($x - $p.X) * $i / $pasos)
        $ny = [int]($p.Y + ($y - $p.Y) * $i / $pasos)
        [Raton]::SetCursorPos($nx, $ny) | Out-Null
        Start-Sleep -Milliseconds 15
    }
}

function ClicXY([int]$x, [int]$y) {
    Mover $x $y
    Pausa 0.15
    [Raton]::mouse_event(0x02, 0, 0, 0, [UIntPtr]::Zero)
    [Raton]::mouse_event(0x04, 0, 0, 0, [UIntPtr]::Zero)
    Pausa 0.35
}

function Clic($el) {
    $r = $el.Current.BoundingRectangle
    ClicXY ([int]($r.X + $r.Width / 2)) ([int]($r.Y + $r.Height / 2))
}

function Escribir([string]$texto) {
    foreach ($c in $texto.ToCharArray()) {
        $k = [string]$c
        if ("+^%~(){}[]".Contains($k)) { $k = "{" + $k + "}" }
        [System.Windows.Forms.SendKeys]::SendWait($k)
        Start-Sleep -Milliseconds 55
    }
}

function Teclas([string]$t) { [System.Windows.Forms.SendKeys]::SendWait($t); Pausa 0.25 }

function Ventana([string]$titulo, [int]$seg = 20) {
    $fin = (Get-Date).AddSeconds($seg)
    while ((Get-Date) -lt $fin) {
        $w = $UIA::RootElement.FindFirst($TS::Children, (New-Object $PC($UIA::NameProperty, $titulo)))
        if ($w) { return $w }
        Start-Sleep -Milliseconds 250
    }
    throw "No aparecio la ventana '$titulo'"
}

function Dialogo([string]$titulo, [int]$seg = 20) {
    $fin = (Get-Date).AddSeconds($seg)
    while ((Get-Date) -lt $fin) {
        $todos = $UIA::RootElement.FindAll($TS::Descendants, (New-Object $PC($UIA::ControlTypeProperty, [System.Windows.Automation.ControlType]::Window)))
        foreach ($w in $todos) { if ($w.Current.Name -eq $titulo) { return $w } }
        Start-Sleep -Milliseconds 250
    }
    throw "No aparecio el dialogo '$titulo'"
}

function Boton($ventana, [string[]]$nombres) {
    foreach ($n in $nombres) {
        $b = $ventana.FindFirst($TS::Descendants, (New-Object $PC($UIA::NameProperty, $n)))
        if ($b) { return $b }
    }
    throw "No se encontro el boton $($nombres -join '/')"
}

function Control([string]$id) {
    $c = $script:app.FindFirst($TS::Descendants, (New-Object $PC($UIA::AutomationIdProperty, $id)))
    if (-not $c) { throw "No se encontro el control $id" }
    return $c
}

function Campo([string]$id, [string]$texto) {
    Clic (Control $id)
    Teclas "^a"
    Teclas "{DEL}"
    Escribir $texto
    Pausa 0.3
}

function Tipo([string]$tipo) {
    $cmb = Control "cmbTipo"
    Clic $cmb
    Pausa 0.4
    if ($tipo -eq "Perecedero") { Teclas "{DOWN}" } else { Teclas "{UP}" }
    Teclas "{ENTER}"
    Pausa 0.4
}

function Vence([int]$dias) {
    $dtp = Control "dtpVencimiento"
    Clic $dtp
    Teclas "{F4}"
    Pausa 0.6
    for ($i = 0; $i -lt $dias; $i++) { Teclas "{RIGHT}" }
    Pausa 0.4
    Teclas "{ENTER}"
    Pausa 0.4
}

function Cantidad([int]$n) {
    Clic (Control "numCantidad")
    Teclas "^a"
    Escribir ([string]$n)
    Teclas "{TAB}"
}

function Agregar([string]$cod, [string]$nom, [string]$precio, [int]$cant, [string]$tipo, [int]$dias = 0) {
    Campo "txtCodigo" $cod
    Campo "txtNombre" $nom
    Campo "txtPrecio" $precio
    Cantidad $cant
    Tipo $tipo
    if ($tipo -eq "Perecedero") { Vence $dias }
    Pausa 0.4
    Clic (Control "btnAgregar")
    Pausa 1.2
}

function Celda([string]$valor) {
    $dgv = Control "dgvProductos"
    $todos = $dgv.FindAll($TS::Descendants, [System.Windows.Automation.Condition]::TrueCondition)
    foreach ($e in $todos) {
        $vp = $null
        if ($e.TryGetCurrentPattern([System.Windows.Automation.ValuePattern]::Pattern, [ref]$vp)) {
            if ($vp.Current.Value -eq $valor) { return $e }
        }
    }
    throw "No se encontro la celda $valor"
}

function Abrir {
    $p = Start-Process -FilePath $Exe -WorkingDirectory (Split-Path $Exe) -PassThru
    $script:app = Ventana "Mi Tiendita - Gestor de productos" 60
    [Raton]::SetForegroundWindow([IntPtr]$script:app.Current.NativeWindowHandle) | Out-Null
    Pausa 1.5
    return $p
}

# ---------------- grabacion ----------------
$json = Join-Path (Split-Path $Exe) "productos.json"
if (Test-Path $json) { Remove-Item $json }

$psi = New-Object System.Diagnostics.ProcessStartInfo
$psi.FileName = "ffmpeg"
$video = Join-Path $Salida "demo_crudo.mp4"
$psi.Arguments = "-y -loglevel error -f gdigrab -framerate 30 -draw_mouse 1 -i desktop -c:v libx264 -preset veryfast -crf 20 -pix_fmt yuv420p " + '"' + $video + '"'
$psi.UseShellExecute = $false
$psi.RedirectStandardInput = $true
$rec = [System.Diagnostics.Process]::Start($psi)
Pausa 2

try {
    $proc = Abrir
    Captura "01_ventana_vacia"

    Agregar "ARR01" "Arroz Diana 500 g" "3500" 10 "Normal"
    Agregar "PAN01" "Pan tajado" "6500" 4 "Normal"

    Agregar "X1" "Prueba" "-500" 1 "Normal"
    $d = Dialogo "No se pudo agregar"
    Pausa 1.2
    Captura "02_error_precio_negativo"
    Clic (Boton $d @("OK", "Aceptar"))
    Pausa 0.8

    Agregar "LEC01" "Leche Alqueria 1 L" "5000" 6 "Perecedero" 1
    Agregar "QUE01" "Queso campesino" "12000" 3 "Perecedero" 10
    Tipo "Normal"
    Pausa 1
    Captura "03_perecedero_con_descuento"

    Clic (Celda "PAN01")
    Pausa 0.6
    Clic (Control "btnEliminar")
    $d = Dialogo "Eliminar producto"
    Pausa 1.2
    Captura "04_confirmar_eliminar"
    Clic (Boton $d @("Yes", "Si"))
    Pausa 1.2

    Clic (Control "btnGuardar")
    $d = Dialogo "Guardar"
    Pausa 1.2
    Captura "05_guardado"
    Clic (Boton $d @("OK", "Aceptar"))
    Pausa 0.8

    $cerrar = $script:app.FindFirst($TS::Descendants, (New-Object $PC($UIA::NameProperty, "Close")))
    if (-not $cerrar) { $cerrar = $script:app.FindFirst($TS::Descendants, (New-Object $PC($UIA::NameProperty, "Cerrar"))) }
    if ($cerrar) { Clic $cerrar } else { Teclas "%{F4}" }
    $proc.WaitForExit(15000) | Out-Null
    Pausa 2

    $proc = Abrir
    Pausa 1
    Captura "06_productos_al_reabrir"

    Clic (Control "btnTrm")
    $fin = (Get-Date).AddSeconds(25)
    while ((Get-Date) -lt $fin -and (Control "lblTrm").Current.Name -eq "Consultando...") { Start-Sleep -Milliseconds 300 }
    Pausa 1.5
    Captura "07_trm_consultada"

    New-NetFirewallRule -DisplayName "SinInternetMiTiendita" -Direction Outbound -Program $Exe -Action Block | Out-Null
    Pausa 1
    Clic (Control "btnTrm")
    $d = Dialogo "TRM" 30
    Pausa 1.2
    Captura "08_trm_sin_internet"
    Clic (Boton $d @("OK", "Aceptar"))
    Remove-NetFirewallRule -DisplayName "SinInternetMiTiendita"
    Pausa 1

    Campo "txtBuscar" "lec01"
    Clic (Control "btnBuscar")
    Pausa 1.5
    Captura "09_buscar_codigo"
    Pausa 1
}
finally {
    Pausa 1
    if ($proc -and -not $proc.HasExited) { $proc.CloseMainWindow() | Out-Null; $proc.WaitForExit(10000) | Out-Null }
    Pausa 1
    $rec.StandardInput.Write("q")
    $rec.WaitForExit(30000) | Out-Null
}

$linea = "/k cd /d " + '"' + $Repo + '"' + " && dotnet test MiTiendita.sln --no-build"
$cmd = Start-Process -FilePath "cmd.exe" -ArgumentList $linea -PassThru
Start-Sleep -Seconds 50
Captura "00_dotnet_test"
Stop-Process -Id $cmd.Id -Force -ErrorAction SilentlyContinue
