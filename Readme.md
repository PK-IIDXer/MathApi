# よく使うコマンド

## マイグレーション
```
dotnet ef migrations add ○○
dotnet ef database update
```

## Controllerのスキャフォールディング
※サンプル

```
dotnet-aspnet-codegenerator controller -name SymbolController -async -api -m Symbol -dc MathDbContext -outDir Controllers
```
