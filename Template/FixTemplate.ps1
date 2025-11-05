# Script para corrigir os arquivos do template substituindo backticks por $ corretamente

$templatePath = "."

# Função para corrigir referências nos arquivos
function Fix-TemplateFile {
    param([string]$filePath)
    
    if (-not (Test-Path $filePath)) {
        return
    }
    
    $content = Get-Content $filePath -Raw -Encoding UTF8
    
    # Corrigir backticks em referências de projeto
    $content = $content -replace '`\$safeprojectname`\$', '$$safeprojectname$$'
    
    Set-Content $filePath -Value $content -Encoding UTF8 -NoNewline
}

# Projetos para processar
$projects = @("Common.Results", "Common.Exceptions", "Common.Extensions", "Common.Logging", "Common.Email")

foreach ($project in $projects) {
    $projectPath = Join-Path $templatePath $project
    
    Write-Host "Corrigindo $project..."
    
    # Corrigir arquivo .csproj
    $csprojFiles = Get-ChildItem -Path $projectPath -Filter "*.csproj" -File
    foreach ($file in $csprojFiles) {
        Fix-TemplateFile $file.FullName
        Write-Host "  Corrigido: $($file.Name)"
    }
}

Write-Host "`nCorreções aplicadas!"
