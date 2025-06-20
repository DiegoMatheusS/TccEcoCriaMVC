# EcoCriaMVC 🌱

Projeto ASP.NET MVC desenvolvido para promover práticas sustentáveis e facilitar o gerenciamento de pontos de coleta e ações ecológicas.

## 📁 Estrutura do Projeto

- `Program.cs` – Inicialização da aplicação.
- `appsettings.json` – Configurações da aplicação.
- `EcoCriaMVC.csproj` – Projeto principal em ASP.NET MVC.
- `Views/`, `Controllers/`, `Models/` – Estrutura padrão MVC.

## 🚀 Como Executar

### Pré-requisitos

- [.NET SDK 6.0 ou superior](https://dotnet.microsoft.com/download)
- Visual Studio 2022 ou superior (ou VS Code com extensão C#)

### Passos para rodar localmente

1. Clone o repositório ou extraia o `.zip`:
   ```bash
   git clone https://github.com/seu-usuario/EcoCriaMVC.git
   ```

2. Navegue até o diretório do projeto:
   ```bash
   cd EcoCriaMVC
   ```

3. Restaure os pacotes e compile:
   ```bash
   dotnet restore
   dotnet build
   ```

4. Execute a aplicação:
   ```bash
   dotnet run
   ```

5. Acesse no navegador:
   ```
   https://localhost:5001
   ```

## 🔧 Funcionalidades

- Cadastro e listagem de pontos de coleta
- Integração com banco de dados
- Visualizações dinâmicas com Razor
- Configurações por ambiente (`Development`, `Production`)
- Possibilidade de envio de alertas (e-mail / WhatsApp)

## 📦 Tecnologias Utilizadas

- ASP.NET Core MVC
- C#
- Entity Framework Core
- Razor Pages
- Bootstrap (UI)

## 📌 Observações

> Certifique-se de configurar corretamente a `connectionString` no arquivo `appsettings.json` para conectar ao seu banco de dados.

## 🤝 Contribuição

Pull Requests são bem-vindos! Para mudanças maiores, abra uma issue antes para discutirmos o que você gostaria de mudar.

## 📄 Licença

Este projeto está licenciado sob a licença MIT. Veja o arquivo [LICENSE](LICENSE) para mais detalhes.
