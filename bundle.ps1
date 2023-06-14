$currentPrincipal = New-Object Security.Principal.WindowsPrincipal([Security.Principal.WindowsIdentity]::GetCurrent())
if (-not ($currentPrincipal.IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator))) {
    write-host "run as admin"
    exit
}

if ($args.Count -lt 1) {
    write-host "Provide password as first arg"
    exit
}

$msix = gci -R *.msix | where-object { $_.Name -like "PathManager.UI*" }
write-host "Found:"
foreach ($item in $msix) {
    write-host $item.Name
}

if (Test-Path ./msix -PathType Container) {
    rm -recurse msix
}
$dir = mkdir msix
cd $dir
mv $msix .

$makeAppXDir = ls "C:\Program Files (x86)\Windows Kits\10\bin" | where-object { $_.Name -match "^[0-9].*" } | select-object -last 1
$makeAppX = "$makeAppXDir\x64\makeappx.exe"

if (-not (Test-Path $makeAppX)) {
    $makeAppX = "$makeAppXDir\x86\makeappx.exe"
}

if (-not (Test-Path $makeAppX)) {
    $makeAppX = "$makeAppXDir\arm64\makeappx.exe"
}
write-host "Using makeappx executable: $makeAppX"

$name = $msix | sort-object | select-object -last 1
$name = $name.Name 
$name = $name -replace ".{9}$"
$name = "$name.msixbundle"

& $makeAppX bundle /d $dir.FullName /p $name >$null

write-host "Created msixbundle"

$signTool = Split-Path $makeAppX -Parent
$signTool = "$signTool\SignTool.exe"

write-host "Using SignTool executable: $signTool"

$pass = $args[0]
$cert = ../PathManager.UI/PathManager.UI_TemporaryKey.pfx
if ($args.Count -gt 1) {
    $cert = $args[1]
}
& $signTool sign /fd SHA256 /a /f $cert /p $pass $name >$null

write-host "Signed bundle"
cd ../
