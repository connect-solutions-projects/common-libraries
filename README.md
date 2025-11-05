# Common Libraries Template for Visual Studio 2022

[![.NET](https://img.shields.io/badge/.NET-9.0-purple.svg)](https://dotnet.microsoft.com/)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)
[![Visual Studio](https://img.shields.io/badge/Visual%20Studio-2022-blue.svg)](https://visualstudio.microsoft.com/)

Um template profissional para Visual Studio 2022 que permite criar rapidamente uma solução com bibliotecas Common reutilizáveis, incluindo Email, Exceptions, Extensions, Logging e Results.

## 🚀 Características

- ✅ **Template de Solução Completo** - Cria automaticamente 5 projetos interconectados
- ✅ **Nomenclatura Dinâmica** - O nome do seu projeto aparece antes de "Common"
- ✅ **Validações Robustas** - Todas as classes incluem validações de segurança
- ✅ **Documentação Completa** - Comentários XML profissionais em todos os métodos
- ✅ **.NET 9.0** - Usa a versão mais recente do .NET
- ✅ **Pronto para Produção** - Inclui tratamento de exceções e logging

## 📦 Projetos Incluídos

### Common.Results
Biblioteca para padronização de retornos de operações com suporte a:
- `Result` e `Result<T>` para operações com e sem dados
- Extensões fluent para configurar status HTTP, mensagens e erros
- Metadados tipados com validação de conversão
- Serialização JSON integrada

### Common.Exceptions
Biblioteca de exceções personalizadas com:
- `AppException` como exceção base com código, HTTP status e metadados
- Serialização compatível com .NET Framework
- Suporte a CorrelationId para rastreamento distribuído
- `DomainException` e `ExternalServiceException` para casos específicos

### Common.Extensions
Biblioteca de extensões úteis com:
- Extensões para `Result`, `String`, `List`, `Enum`, `Json`, etc.
- Validações automáticas de null usando `ArgumentNullException.ThrowIfNull`
- Tratamento robusto de exceções em todas as operações

### Common.Email
Biblioteca de envio de emails via SMTP com:
- Integração com MailKit
- Suporte a SSL/TLS e STARTTLS
- Validação de formato de email com regex compilado
- Suporte a anexos com validação completa
- Timeout configurável

### Common.Logging
Biblioteca de logging com:
- Integração com Serilog
- Middleware de CorrelationId para rastreamento
- Extensões para logging de resultados
- Suporte a múltiplos sinks (Console, File, SQL Server, etc.)

## 📋 Requisitos

- Visual Studio 2022
- .NET 9.0 SDK

## 🛠️ Instalação

### 1. Baixar o Template

Baixe o arquivo `CommonLibrariesTemplate.zip` da seção [Releases](../../releases) ou crie você mesmo seguindo as instruções de desenvolvimento.

### 2. Instalar no Visual Studio

Copie o arquivo `CommonLibrariesTemplate.zip` para a pasta de templates do Visual Studio:

```
%USERPROFILE%\Documents\Visual Studio 2022\Templates\ProjectTemplates\
```

### 3. Reiniciar Visual Studio

Reinicie o Visual Studio 2022 (se estiver aberto) para carregar o novo template.

## 🎯 Como Usar

1. Abra o Visual Studio 2022
2. Clique em **"Criar um novo projeto"**
3. Procure por **"Common Libraries Solution Template"**
4. Digite o nome do seu projeto (ex: "MeuProjeto")
5. O Visual Studio criará uma solução com todos os projetos Common, onde o nome do seu projeto aparecerá antes de "Common"

### Exemplo

Se você criar um projeto chamado **"AuthCenter"**, os projetos serão:
- `AuthCenter.Common.Results`
- `AuthCenter.Common.Exceptions`
- `AuthCenter.Common.Extensions`
- `AuthCenter.Common.Logging`
- `AuthCenter.Common.Email`

## ✨ Melhorias da Versão 2.0

### 🔒 Segurança e Validações
- ✅ Validações de segurança em `SmtpEmailSender` (configurações, destinatários, anexos)
- ✅ Validação de formato de email em `EmailAddress` com regex compilado e timeout
- ✅ Validações de null em todos os métodos de extensão
- ✅ Melhorias em `GetMetadata` com validação de tipo e conversão segura
- ✅ Validação de chaves em `AddMetadata` para prevenir chaves vazias

### 🛡️ Tratamento de Exceções
- ✅ Melhor tratamento de exceções em `BuildMimeMessage` com mensagens específicas
- ✅ Tratamento melhorado em `StringExtensions.ToDateTime()` e `ToDateTimeWithHour()`
- ✅ Tratamento de erros em `JsonExtensions` com exceções específicas

### 📚 Documentação
- ✅ Comentários XML profissionais em todas as classes e métodos
- ✅ Documentação completa de parâmetros, retornos e exceções
- ✅ Exemplos e descrições detalhadas

### 💎 Qualidade de Código
- ✅ Proteção contra null em coleções de erros
- ✅ Validação de limites (ex: email com 254 caracteres conforme RFC 5321)
- ✅ Conversão de tipos segura com `Convert.ChangeType`

## 📖 Exemplos de Uso

### Result Pattern

```csharp
using MeuProjeto.Common.Results;
using MeuProjeto.Common.Extensions;

public Result<Usuario> ObterUsuario(int id)
{
    var result = new Result<Usuario>();
    
    if (id <= 0)
        return result.Failed("ID inválido");
    
    var usuario = _repository.Obter(id);
    
    if (usuario == null)
        return result.NotFound("Usuário não encontrado");
    
    return result.Successful(usuario);
}
```

### Envio de Email

```csharp
using MeuProjeto.Common.Email;
using MeuProjeto.Common.Email.Models;

var message = new EmailMessage
{
    From = new EmailAddress("Sistema", "noreply@exemplo.com"),
    To = new[] { new EmailAddress("Usuário", "usuario@exemplo.com") },
    Subject = "Bem-vindo!",
    HtmlBody = "<h1>Bem-vindo ao sistema!</h1>",
    TextBody = "Bem-vindo ao sistema!"
};

await _emailSender.SendAsync(message);
```

### Exceções Customizadas

```csharp
using MeuProjeto.Common.Exceptions;

throw new DomainException("CPF_INVALIDO", "CPF inválido")
    .WithCorrelation(correlationId)
    .WithMeta("Cpf", cpf);
```

## 🔧 Personalização

Se você quiser modificar o template:

1. Clone este repositório
2. Faça suas alterações nos arquivos fonte em `src/`
3. Execute o script `CreateTemplate.ps1` para regenerar os arquivos do template
4. Execute o script `FixTemplate.ps1` para corrigir referências (se necessário)
5. Compacte novamente em um arquivo `.zip`

## 📁 Estrutura do Projeto

```
common/
├── src/                    # Código fonte dos projetos
│   ├── Common.Results/
│   ├── Common.Exceptions/
│   ├── Common.Extensions/
│   ├── Common.Logging/
│   └── Common.Email/
├── Template/               # Arquivos do template
│   ├── Common.Results/
│   ├── Common.Exceptions/
│   ├── Common.Extensions/
│   ├── Common.Logging/
│   ├── Common.Email/
│   ├── CreateTemplate.ps1  # Script para gerar template
│   └── FixTemplate.ps1     # Script para corrigir referências
└── README.md
```

## 🤝 Contribuindo

Contribuições são bem-vindas! Sinta-se à vontade para:

1. Fazer fork do projeto
2. Criar uma branch para sua feature (`git checkout -b feature/AmazingFeature`)
3. Commit suas mudanças (`git commit -m 'Add some AmazingFeature'`)
4. Push para a branch (`git push origin feature/AmazingFeature`)
5. Abrir um Pull Request

## 📝 Licença

Este projeto está licenciado sob a licença MIT - veja o arquivo [LICENSE](LICENSE) para detalhes.

## 🙏 Agradecimentos

- MailKit por fornecer uma biblioteca robusta de SMTP
- Serilog por fornecer logging estruturado
- Microsoft por fornecer o .NET Framework

## 📧 Contato

Para dúvidas ou sugestões, abra uma [issue](../../issues) no repositório.

---

**Desenvolvido com ❤️ para a comunidade .NET**

