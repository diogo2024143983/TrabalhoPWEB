# MyHotels

Trabalho prático de Programação Web 2026/2027 — cadeia de hotéis e aparthotéis.

## Arquitetura

| Projeto | Função |
|---------|--------|
| `MyHotels.Domain` | Entidades e roles de domínio |
| `MyHotels.Infrastructure` | EF Core, Identity, LocalDB, seed (`Inicializacao`) |
| `MyHotels.Shared` | RCL partilhada (UI + DTOs + serviços HTTP) |
| `MyHotels.Frontend` | Frontend Web Blazor (consome a API) |
| `MyHotels.Maui` | Frontend Hybrid (mesmo código RCL; Windows/Android/iOS/…) |
| `MyHotels.Admin` | Gestão de alojamentos (acesso direto à BD) |
| `MyHotels.Api` | API REST + Swagger + JWT |

Admin e API usam a **mesma** base de dados LocalDB `MyHotels`. O frontend público fala **só** com a API (JWT).

## Requisitos

- Visual Studio 2022 + workloads ASP.NET, .NET MAUI
- .NET 8 (Admin/API/Frontend/Shared/Domain/Infrastructure)
- SQL Server LocalDB
- O projeto MAUI usa `net8.0-*`, conforme as fichas laboratoriais

## Credenciais de demonstração (seed)

| Perfil | Email | Password |
|--------|-------|----------|
| Administrador | `admin@myhotels.local` | `Admin#2026!` |
| Funcionário | `funcionario@myhotels.local` | `Staff#2026!` |
| Cliente | `cliente@myhotels.local` | `Cliente#2026!` |
| Fornecedor | `fornecedor@myhotels.local` | `Fornecedor#2026!` |

## Executar

1. Abrir `MyHotels.slnx`
2. Criar/atualizar a BD (ou deixar o arranque do Admin/API fazer `Migrate` + seed):

```powershell
dotnet ef migrations add InitialCreate --project src\MyHotels.Infrastructure --startup-project src\MyHotels.Api
dotnet ef database update --project src\MyHotels.Infrastructure --startup-project src\MyHotels.Api
```

3. Arrancar em simultâneo: `MyHotels.Api`, `MyHotels.Admin`, `MyHotels.Frontend`
4. Swagger: `https://localhost:7280/swagger`
5. Para MAUI ↔ API em device/emulador, criar um **Dev Tunnel** para a API e definir a URL:

```powershell
$env:MYHOTELS_API_URL = "https://<tunnel>.devtunnels.ms/"
```

O frontend Web usa `ApiBaseUrl` em `src\MyHotels.Frontend\MyHotels.Frontend\appsettings.json`.

O fluxo público completo é: consultar oferta sem login, criar conta de Cliente,
aguardar ativação no Admin, iniciar sessão, selecionar alojamentos do mesmo hotel,
criar a reserva, simular pagamento e consultar o histórico em `minhas-reservas`.

## Alinhamento com as fichas

- Ficha 2: RCL partilhada + Blazor Web + Blazor Hybrid (MAUI)
- Fichas 3–5: Admin com Identity, entidades, migrations, seed, NavMenu e CRUD (categorias, modos, alojamentos/hotéis)
- Enunciado TP: API JWT, roles, gestão de reservas/clientes, frontend com frisos de hotéis/categorias/alojamentos
