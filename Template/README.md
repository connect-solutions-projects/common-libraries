# Template de Bibliotecas Common para Visual Studio 2022

Este template permite criar uma solução com bibliotecas Common (Email, Exceptions, Extensions, Logging, Results) onde o nome do seu projeto aparecerá antes de "Common".

Por exemplo, se você criar um projeto chamado "MeuProjeto", os projetos serão:
- MeuProjeto.Common.Results
- MeuProjeto.Common.Exceptions
- MeuProjeto.Common.Extensions
- MeuProjeto.Common.Logging
- MeuProjeto.Common.Email

## Versão

**Versão atual**: 2.0 (com melhorias de segurança e validações)

**Data de atualização**: Novembro 2025

## Instalação

1. Copie o arquivo `CommonLibrariesTemplate.zip` para a pasta de templates do Visual Studio:
   ```
   %USERPROFILE%\Documents\Visual Studio 2022\Templates\ProjectTemplates\
   ```

2. Reinicie o Visual Studio 2022 (se estiver aberto)

3. Ao criar um novo projeto, procure por "Common Libraries Solution Template" na lista de templates

## Como Usar

1. Abra o Visual Studio 2022
2. Clique em "Criar um novo projeto"
3. Procure por "Common Libraries Solution Template"
4. Digite o nome do seu projeto (ex: "MeuProjeto")
5. O Visual Studio criará uma solução com todos os projetos Common, onde o nome do seu projeto aparecerá antes de "Common"

## Estrutura do Template

O template inclui:
- **Common.Results**: Biblioteca de resultados e erros com validações robustas e metadados tipados
- **Common.Exceptions**: Biblioteca de exceções personalizadas com serialização para .NET Framework
- **Common.Extensions**: Biblioteca de extensões úteis com validações de null e tratamento de exceções
- **Common.Logging**: Biblioteca de logging com Serilog e middlewares de correlação
- **Common.Email**: Biblioteca de envio de emails com validação de formato e validações de segurança

## Melhorias Implementadas (v2.0)

### Segurança e Validações
- ✅ Validações de segurança em `SmtpEmailSender` (validação de configurações, destinatários, anexos)
- ✅ Validação de formato de email em `EmailAddress` com regex compilado e timeout
- ✅ Validações de null em todos os métodos de extensão usando `ArgumentNullException.ThrowIfNull`
- ✅ Melhorias em `GetMetadata` com validação de tipo e conversão segura
- ✅ Validação de chaves em `AddMetadata` para prevenir chaves vazias

### Tratamento de Exceções
- ✅ Melhor tratamento de exceções em `BuildMimeMessage` com mensagens específicas
- ✅ Tratamento melhorado em `StringExtensions.ToDateTime()` e `ToDateTimeWithHour()`
- ✅ Tratamento de erros em `JsonExtensions` com exceções específicas

### Documentação
- ✅ Comentários XML profissionais em todas as classes e métodos
- ✅ Documentação completa de parâmetros, retornos e exceções
- ✅ Exemplos e descrições detalhadas

### Qualidade de Código
- ✅ Proteção contra null em coleções de erros
- ✅ Validação de limites (ex: email com 254 caracteres conforme RFC 5321)
- ✅ Conversão de tipos segura com `Convert.ChangeType`

## Observações

- Todos os namespaces e referências são automaticamente ajustados para usar o nome do seu projeto
- As dependências entre projetos são mantidas automaticamente
- O template usa .NET 9.0
- Todas as classes incluem validações robustas para prevenir erros em runtime

## Personalização

Se você quiser modificar o template:
1. Descompacte o arquivo .zip
2. Faça suas alterações nos arquivos fonte em `src/`
3. Execute o script `CreateTemplate.ps1` para regenerar os arquivos do template
4. Execute o script `FixTemplate.ps1` para corrigir referências (se necessário)
5. Compacte novamente em um arquivo .zip

## Recursos Principais

### Common.Results
- `Result` e `Result<T>` para padronização de retornos
- Extensões fluent para configurar status HTTP, mensagens e erros
- Suporte a metadados tipados
- Serialização JSON integrada

### Common.Exceptions
- `AppException` como exceção base com código, HTTP status e metadados
- Serialização compatível com .NET Framework
- Suporte a CorrelationId para rastreamento

### Common.Extensions
- Extensões para `Result`, `String`, `List`, `Enum`, `Json`, etc.
- Validações automáticas de null
- Tratamento robusto de exceções

### Common.Email
- Envio de emails via SMTP com MailKit
- Suporte a SSL/TLS e STARTTLS
- Validação de formato de email
- Suporte a anexos com validação

### Common.Logging
- Integração com Serilog
- Middleware de CorrelationId
- Extensões para logging de resultados
- Suporte a múltiplos sinks (Console, File, SQL Server, etc.)

## Licença

Este template é fornecido como está. Use livremente em seus projetos.
