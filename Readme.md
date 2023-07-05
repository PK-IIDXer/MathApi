# 主な参考サイト
* [Vagrant AlmaLinux 9 MySQL 8.0インストール&Sequel Ace接続](https://kanlab.net/vagrant-almalinux9-install-mysql8-connect-sequelace/)
* [チュートリアル: ASP.NET Core で Web API を作成する](https://learn.microsoft.com/ja-jp/aspnet/core/tutorials/first-web-api?view=aspnetcore-7.0&tabs=visual-studio-code)
* [【C#】Entity Framework Coreを使ってMySQLを操作する](https://hirahira.blog/efcore-mysql/)
* [EntityFrameworkCoreを使ってみた](https://zenn.dev/neko3cs/articles/try-using-entity-framework-core)
* [データのシード処理](https://learn.microsoft.com/ja-jp/ef/core/modeling/data-seeding)
* [Code First Data Annotations (Code First のデータ注釈)](https://learn.microsoft.com/ja-jp/ef/ef6/modeling/code-first/data-annotations)

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
