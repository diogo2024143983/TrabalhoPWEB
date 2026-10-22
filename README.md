# MyHotels

Base do trabalho prático de Programação Web 2026/2027.

## Estrutura

- `MyHotels.Domain`: entidades e regras de domínio partilhadas.
- `MyHotels.Infrastructure`: Entity Framework Core, SQL Server LocalDB e ASP.NET Core Identity.
- `MyHotels.Shared`: Razor Class Library para componentes e estilos reutilizáveis.
- `MyHotels.Frontend`: frontend público Blazor Web App (com projeto Client para o modo WebAssembly).
- `MyHotels.Admin`: aplicação de gestão para funcionários e administradores.
- `MyHotels.Api`: API REST com Swagger para consumo controlado pelo frontend.

## Requisitos locais

- Visual Studio 2022 com ASP.NET and web development e .NET 8.
- SQL Server LocalDB.

## Executar

1. Abrir `MyHotels.slnx` no Visual Studio.
2. Configurar os projetos `MyHotels.Api`, `MyHotels.Admin` e `MyHotels.Frontend` como projetos de arranque.
3. Criar a base de dados através das migrations do projeto `MyHotels.Infrastructure`:

```powershell
dotnet ef migrations add InitialCreate --project src\MyHotels.Infrastructure --startup-project src\MyHotels.Api
dotnet ef database update --project src\MyHotels.Infrastructure --startup-project src\MyHotels.Api
```

O trabalho usa a connection string `MyHotels` em `appsettings.json`. As credenciais, dados de demonstração, regras de negócio e o MER devem ser documentados no relatório final.
