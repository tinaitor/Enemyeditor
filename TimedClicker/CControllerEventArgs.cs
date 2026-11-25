using System;

namespace TimedClicker
{
    // Делегат: описывает, какой формы должен быть метод-обработчик события контроллера
    public delegate void ControllerEventHandler(object sender, CControllerEventArgs e);

    // Аргументы события: какой объект добавили/удалили со сцены
    public class CControllerEventArgs : EventArgs
    {
        public CCollectable Object { get; }

        public CControllerEventArgs(CCollectable obj)
        {
            Object = obj;
        }
    }
}
