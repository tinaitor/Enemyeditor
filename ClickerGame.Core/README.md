# Игровое ядро Лабы 2

`ClickerGame.Core` — библиотека без зависимости от WPF. В игре добавьте ссылку на
`ClickerGame.Core.csproj` и пространство имён `ClickerGame.Core`.

```csharp
GameSession game = GameSession.LoadFromJson(pathToEnemiesJson);
bool defeated = game.ClickEnemy();
bool upgraded = game.TryUpgradeDamage();

string health = game.CurrentEnemy.CurrentHealth.ToString();
string gold = game.Player.Gold.ToString();
```

Окно только вызывает методы и перечитывает свойства. Награда начисляется один
раз при победе. Сразу после победы создаётся новый противник.

JSON Лабы 1 загружается без новых полей: `Reward` и `SpawnChance` равны 1,
модификаторы равны 1. Для новых шаблонов можно задать `Reward`, `SpawnChance`,
`HealthModifier`, `DamageModifier`, `SpeedModifier`, `RewardModifier`. Шансы —
неотрицательные веса, которые игровое ядро нормализует до суммы 1. После `n`
побед каждая характеристика равна базовой характеристике, умноженной на
соответствующий модификатор в степени `n`. Модификаторы — целые числа не меньше 1.

Цена первого улучшения — 10 золота. Каждое улучшение удваивает урон и утраивает
цену следующего улучшения. Для проверки без WPF выполните из корня репозитория:

```bash
dotnet run --project BackendTests/BackendTests.csproj
```
