using ClickerGame.Core;

static void Check(bool condition, string message)
{
    if (!condition) throw new Exception(message);
}

static void Expect<T>(Action action, string message) where T : Exception
{
    try { action(); }
    catch (T) { return; }
    throw new Exception(message);
}

BigNumber a = new BigNumber("999999999999999999");
BigNumber b = new BigNumber("1");
Check((a + b).ToString() == "1000000000000000000", "Перенос при сложении");
Check(a.ToString() == "999999999999999999", "Исходное число изменилось");
Check((new BigNumber("1000000") - b).ToString() == "999999", "Заём при вычитании");
Check(new BigNumber(int.MaxValue).ToString() == "2147483647", "Создание из int.MaxValue");
Check((a * a).ToString() == "999999999999999998000000000000000001", "Поразрядное умножение");
Check(((a * a) / a) == a, "Деление на большое число");
Check((b / a).IsZero, "Деление меньшего числа на большее");
Check((new BigNumber("1000000000000000000000000000000") / new BigNumber(3)).ToString()
    == "333333333333333333333333333333", "Деление с переносом остатка");
Check(new BigNumber("000019005017").ToString() == "19005017", "Форматирование блоков");
Expect<DivideByZeroException>(() => _ = a / BigNumber.Zero, "Деление на ноль");
Expect<ArgumentOutOfRangeException>(() => _ = b - a, "Отрицательное значение");

EnemyTemplate never = new EnemyTemplate { Name = "Не выбирается", Health = 1, SpawnChance = 0 };
EnemyTemplate chosen = new EnemyTemplate
{
    Name = "Выбирается", Health = 2, Reward = 10, SpawnChance = 5,
    HealthModifier = 2, RewardModifier = 3
};
GameSession game = new GameSession(new[] { never, chosen });
Check(game.CurrentEnemy.Name == "Выбирается", "Выбор по шансам");
Check(!game.ClickEnemy() && game.CurrentEnemy.CurrentHealth == BigNumber.One, "Первый удар");
Check(game.ClickEnemy() && game.VictoryCount == BigNumber.One, "Победа");
Check(game.Player.Gold == new BigNumber(10), "Выдача награды");
Check(game.CurrentEnemy.MaxHealth == new BigNumber(4) && game.CurrentEnemy.Reward == new BigNumber(30),
    "Масштабирование после победы");
Check(game.TryUpgradeDamage() && game.Player.Damage == new BigNumber(2), "Покупка улучшения");
Check(!game.TryUpgradeDamage(), "Покупка без золота");
Check(!game.ClickEnemy() && game.ClickEnemy(), "Второй бой");
Check(game.Player.Gold == new BigNumber(30), "Награда второго боя");
Check(game.CurrentEnemy.MaxHealth == new BigNumber(8) && game.CurrentEnemy.Reward == new BigNumber(90),
    "Масштабирование второго боя");

string path = Path.GetTempFileName();
try
{
    File.WriteAllText(path, "[{\"Name\":\"Старый JSON\",\"Health\":3,\"Damage\":1,\"Speed\":1}]");
    GameSession oldSave = GameSession.LoadFromJson(path);
    Check(oldSave.CurrentEnemy.Reward == BigNumber.One, "Значения по умолчанию для JSON Лабы 1");
}
finally { File.Delete(path); }

Console.WriteLine("Проверки бэкенда пройдены.");
