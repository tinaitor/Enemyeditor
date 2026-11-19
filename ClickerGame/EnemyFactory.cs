using System;
using System.Reflection;

namespace ClickerGame
{
    public static class EnemyFactory
    {
        public static IEnemy Create(string typeName, params object[] args)
        {
            if (string.IsNullOrWhiteSpace(typeName))
                throw new ArgumentException("Имя типа противника не задано.");

            Type? type = Type.GetType(typeName)
                         ?? Assembly.GetExecutingAssembly().GetType("ClickerGame." + typeName);

            if (type == null)
                throw new ArgumentException($"Неизвестный тип противника: {typeName}");
            if (!typeof(IEnemy).IsAssignableFrom(type))
                throw new ArgumentException($"Тип {typeName} не реализует IEnemy.");

            try
            {
                object? instance = Activator.CreateInstance(type, args);
                return instance as IEnemy
                       ?? throw new ArgumentException($"Не удалось привести {typeName} к IEnemy.");
            }
            catch (TargetInvocationException ex)
            {
                throw new ArgumentException(
                    $"Ошибка в конструкторе {typeName}: {ex.InnerException?.Message}", ex);
            }
            catch (MissingMethodException ex)
            {
                throw new ArgumentException(
                    $"У типа {typeName} нет конструктора с такими аргументами.", ex);
            }
        }
    }
}