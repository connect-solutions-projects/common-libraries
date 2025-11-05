# Script para criar template do Visual Studio substituindo Common por $safeprojectname$.Common

$scriptPath = Split-Path -Parent $MyInvocation.MyCommand.Path
$sourcePath = Join-Path $scriptPath "..\src" | Resolve-Path
$templatePath = $scriptPath

# Função para substituir Common. por $safeprojectname$.Common. nos arquivos
function Replace-CommonNames {
    param([string]$filePath)
    
    $content = Get-Content $filePath -Raw -Encoding UTF8
    
    # Substituir namespaces: namespace Common.X -> namespace $safeprojectname$.Common.X
    $content = $content -replace 'namespace Common\.', 'namespace $safeprojectname$.Common.'
    
    # Substituir usings: using Common.X -> using $safeprojectname$.Common.X
    $content = $content -replace 'using Common\.', 'using $safeprojectname$.Common.'
    
    # Substituir referências de projeto: Common.X.csproj -> $safeprojectname$.Common.X.csproj
    $content = $content -replace 'Common\.([A-Za-z]+)\.csproj', '$$safeprojectname$$.Common.$1.csproj'
    
    # Substituir nomes de projeto: "Common.X" -> "$safeprojectname$.Common.X"
    $content = $content -replace '"(Common\.([A-Za-z\.]+))"', '"$$safeprojectname$$.Common.$2"'
    
    # Substituir referências de pasta: Common.X\ -> $safeprojectname$.Common.X\
    $content = $content -replace 'Common\.([A-Za-z]+)\\', '$$safeprojectname$$.Common.$1\'
    
    Set-Content $filePath -Value $content -Encoding UTF8 -NoNewline
}

# Projetos para processar
$projects = @("Common.Results", "Common.Exceptions", "Common.Extensions", "Common.Logging", "Common.Email")

foreach ($project in $projects) {
    $sourceProjectPath = Join-Path $sourcePath $project
    $targetProjectPath = Join-Path $templatePath $project
    
    Write-Host "Processando $project..."
    
    # Criar diretório de destino se não existir
    if (-not (Test-Path $targetProjectPath)) {
        New-Item -ItemType Directory -Path $targetProjectPath -Force | Out-Null
    }
    
    # Copiar e processar arquivos .cs
    $sourceProjectPathResolved = Resolve-Path $sourceProjectPath
    $csFiles = Get-ChildItem -Path $sourceProjectPath -Filter "*.cs" -Recurse -File | Where-Object { $_.FullName -notmatch "\\bin\\|\\obj\\" }
    
    foreach ($file in $csFiles) {
        $fullPath = $file.FullName
        $relativePath = $fullPath.Substring($sourceProjectPathResolved.Path.Length + 1)
        $targetFile = Join-Path $targetProjectPath $relativePath
        $targetDir = Split-Path $targetFile -Parent
        
        if (-not (Test-Path $targetDir)) {
            New-Item -ItemType Directory -Path $targetDir -Force | Out-Null
        }
        
        Copy-Item $file.FullName $targetFile -Force
        Replace-CommonNames $targetFile
        Write-Host "  Processado: $relativePath"
    }
    
    # Copiar e processar arquivo .csproj
    $csprojFile = Join-Path $sourceProjectPath "$project.csproj"
    if (Test-Path $csprojFile) {
        $projectName = $project -replace '^Common\.', ''
        $targetCsproj = Join-Path $targetProjectPath "`$safeprojectname`$.Common.$projectName.csproj"
        Copy-Item $csprojFile $targetCsproj -Force
        Replace-CommonNames $targetCsproj
        Write-Host "  Processado: $project.csproj"
    }
    
    # Copiar README.md se existir
    $readmeFile = Join-Path $sourceProjectPath "README.md"
    if (Test-Path $readmeFile) {
        $targetReadme = Join-Path $targetProjectPath "README.md"
        Copy-Item $readmeFile $targetReadme -Force
    }
}

Write-Host "`nTemplate criado com sucesso!"
Write-Host "Próximo passo: Compactar a pasta Template em um arquivo .zip e copiar para:"
Write-Host "%USERPROFILE%\Documents\Visual Studio 2022\Templates\ProjectTemplates\"
